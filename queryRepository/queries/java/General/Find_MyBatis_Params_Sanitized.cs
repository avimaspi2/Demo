// Finds parameters sanitized by the use of MyBatis syntax '#{ }'
// http://www.mybatis.org/mybatis-3/sqlmap-xml.html

// For Java Mappers:
var operations = new List<string>{"Select","Insert","Delete","Update"};
CxList customAtts = Find_CustomAttribute().FindByShortNames(operations);
CxList strings = Find_String_Literal();
CxList invokes = Find_Methods();

CxList stringsInOperation = strings.GetByAncs(customAtts);
stringsInOperation = stringsInOperation.FindByShortName(@"*#{*");

CxList paramsOfMethodsWithCA = Find_ParamDecl().GetParameters(stringsInOperation.GetAncOfType(typeof(MethodDecl)));
CxList sanitizers = All.NewCxList();
foreach(CxList parameter in paramsOfMethodsWithCA){
	
	int position = parameter.GetIndexOfParameter(); 
	var paramName = parameter.GetName();
	var list = new List<string>{"*#{" + paramName + "}*","*#{" + position + "}*"};
	
	CxList method = parameter.GetAncOfType(typeof(MethodDecl));
	CxList safeStringsOfMethod = stringsInOperation.GetByAncs(method);	
	CxList safeParam = safeStringsOfMethod.FindByShortNames(list);
	if(safeParam.Count > 0){
		CxList methodCalls = invokes.FindAllReferences(method);
		sanitizers.Add(All.GetParameters(methodCalls, position));
	}
}
result = All.GetByAncs(sanitizers);

// For Xml Mappers:
CxList sqlCmdStrings = strings.GetByAncs(All.FindByShortName("sqlCmdString"));
Regex rx = new Regex(@"#{\s*(\w+?)\s*,");
CxList santizedMethods = All.NewCxList();
foreach(CxList cmdStr in sqlCmdStrings)
{
	string parentMethodName = cmdStr.GetAncOfType(typeof(MethodDecl)).TryGetCSharpGraph<MethodDecl>().Name;
	StringLiteral strLit = cmdStr.TryGetCSharpGraph<StringLiteral>();
	foreach(Match match in rx.Matches(strLit.Text))
	{
		// Currently only works for Maps
		string santizedParam = match.Groups[1].Value;
		CxList sanitizedParamKey = strings.FindByShortName(santizedParam);
		CxList flowToCall = invokes.FindByShortName(parentMethodName).DataInfluencedBy(sanitizedParamKey);
		if (flowToCall.Count > 0)
		{
			// The key of the query must influence the call to the query
			santizedMethods.Add(sanitizedParamKey.GetAncOfType(typeof(MethodInvokeExpr)));
		}
	}
}
result.Add(santizedMethods);