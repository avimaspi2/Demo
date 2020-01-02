/* References:
	http://rails-sqli.org/
	http://blog.phusion.nl/2013/01/03/rails-sql-injection-vulnerability-hold-your-horses-here-are-the-facts/
	http://guides.rubyonrails.org/active_record_querying.html
	http://guides.rubyonrails.org/security.html#sql-injection
*/
CxList activeRecord = Find_DB_In_ActiveRecord();

//Usage of new
CxList modelCreateStmt = Find_ObjectCreations();
modelCreateStmt = Find_TypeRef().FindByFathers(modelCreateStmt);	
CxList model = Find_ClassDecl().InheritsFrom("ActiveRecord.Base");
result.Add(modelCreateStmt.FindAllReferences(model).GetFathers());


// Dynamic .find_by* queries can negate sanitization
CxList binaryExprs = Find_BinaryExpr();
CxList findBy = activeRecord.FindByShortName("find_by*") - activeRecord.FindByShortName("find_by_sql");
CxList unsanitizedFindBy = findBy * binaryExprs.GetByAncs(findBy).GetAncOfType(typeof(MethodInvokeExpr));
result.Add(findBy - unsanitizedFindBy);
	
//first parameter of find is an integer and is a sanitizer:
CxList finds = activeRecord.FindByShortName("find");
CxList findFirstParameter = All.GetParameters(finds, 0);
result.Add(All.GetByAncs(findFirstParameter));

CxList findAllParameters = All.GetParameters(finds);


CxList findAllParametersExceptFirst = findAllParameters - findFirstParameter;
findAllParametersExceptFirst -= findAllParametersExceptFirst.FindByAbstractValue(x => x is IntegerIntervalAbstractValue);
findAllParametersExceptFirst -= findAllParametersExceptFirst.FindByType(typeof(ArrayInitializer));

CxList findsWithOneParameter = finds.FindByParameters(findFirstParameter);
CxList findsWithMoreParameters = finds.FindByParameters(findAllParametersExceptFirst);
result.Add(findsWithOneParameter - findsWithMoreParameters);


CxList whereMethod = activeRecord.FindByShortName("where");
CxList zeroParamSanitizerMethods = activeRecord.FindByShortName("destroy_all");
zeroParamSanitizerMethods.Add(activeRecord.FindByShortName("delete_all"));
zeroParamSanitizerMethods.Add(whereMethod);
//all parameters other than first are parameterized query
CxList ZeroParam = All.GetByAncs(All.GetParameters(zeroParamSanitizerMethods, 0));
CxList StmtSanitizers = (All.GetByAncs(All.GetParameters(zeroParamSanitizerMethods)) - ZeroParam);
result.Add(StmtSanitizers);
CxList unknownLeft = Find_UnknownReference().FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList cxHashes = unknownLeft.FindByFathers(All.FindByType("CxHash").GetFathers());
CxList UnknownParamRef = All.FindAllReferences(cxHashes);

//cases like User.where(id: params[:referral])
CxList whereFirstParams = All.GetParameters(whereMethod, 0);
CxList activeRecordParameterized = whereFirstParams.FindByType(typeof(AssignExpr));
activeRecordParameterized = All.FindByFathers(activeRecordParameterized);
CxList parameterKeyValue = All.FindByFathers(activeRecordParameterized);
result.Add(parameterKeyValue);
result.Add(UnknownParamRef * whereFirstParams); 
//cases like User.where(id: (foo>1)? params[:referral] : params[:name])
result.Add(All.FindByFathers(parameterKeyValue.FindByType(typeof(IndexerRef))));

//add all hashes to sanitizers
CxList conditionsParam = All.FindByFathers(All.GetParameters(finds));	
CxList irrelevantAssign = conditionsParam.FindByShortName("conditions").GetFathers();
conditionsParam = ((All - irrelevantAssign).GetByAncs(irrelevantAssign));
CxList problematicParams = All.NewCxList();
problematicParams.Add(ZeroParam);
problematicParams.Add(conditionsParam);
CxList assignExprSanitizer = problematicParams.FindByType(typeof(AssignExpr));
result.Add(assignExprSanitizer);
result.Add(assignExprSanitizer.GetFathers().FindByType(typeof(AssignExpr)));
//using an array as a parameter without parameterized query:
CxList arrayInit = problematicParams.FindByType(typeof(ArrayInitializer));


CxList temp = All.NewCxList();

foreach(CxList init in arrayInit)
{
	ArrayInitializer ai = init.TryGetCSharpGraph<ArrayInitializer>();
	
	if (ai != null && ai.InitialValues != null)
	{
		ExpressionCollection parameters = ai.InitialValues;
		bool first = true;
		foreach (Expression e in parameters)
		{
			if(first)
			{
				first = false;
				continue;
			}
			temp.Add(All.FindById(e.NodeId));
		}
	}
}
result.Add(All.GetByAncs(temp));