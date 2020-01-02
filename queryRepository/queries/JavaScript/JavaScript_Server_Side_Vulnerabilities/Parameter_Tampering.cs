CxList methodInv = Find_Methods();
CxList allParams = Find_Parameters();
CxList uRef = Find_UnknownReference();
CxList mA = Find_MemberAccesses();
CxList urMA = All.NewCxList();
urMA.Add(uRef);
urMA.Add(mA);

CxList input = NodeJS_Find_Interactive_Inputs();
CxList db = NodeJS_Find_DB_IN();
CxList strings = NodeJS_Find_Strings();
//Support for select query as a string
//select * from TABLE where a="some input"
CxList Select = strings.FindByShortName("*select*", false);
CxList Where = strings.FindByShortName("*where*", false);
CxList And = strings.FindByShortName("*And *", false);
And.Add(strings.FindByShortName("* And*", false));

//add query and execute methods of select and where
CxList methodsOfSelect = Select.GetAncOfType(typeof (MethodInvokeExpr));
methodsOfSelect.Add(Where.GetAncOfType(typeof (MethodInvokeExpr)));

//remove all statements that include And
methodsOfSelect = methodsOfSelect.DataInfluencedBy(Select).DataInfluencedBy(Where);
methodsOfSelect -= methodsOfSelect.DataInfluencedBy(And);
CxList methodWithoutAnd = methodsOfSelect.GetAncOfType(typeof (MethodInvokeExpr));

//support for select query as methods
//query().select('*').from('TABLE').where("a = ?" [some input]);
CxList selectMeth = NodeJS_DB_Output_Methods();
CxList whereMeth = methodInv.FindByShortName("where");
CxList andMeth = methodInv.FindByShortName("and");
CxList queryExecuteMethods = methodInv.FindByShortName("query");

whereMeth = whereMeth.DataInfluencedBy(selectMeth).DataInfluencedBy(queryExecuteMethods)*whereMeth;
whereMeth -= whereMeth.DataInfluencingOn(andMeth);
CxList whereParams = allParams.GetParameters(whereMeth);
whereParams.Add(urMA.GetByAncs(whereParams));
CxList whereWithInput = whereParams.DataInfluencedBy(input);
whereWithInput.Add(whereParams * input);

whereMeth = whereMeth.DataInfluencedBy(whereWithInput, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);


CxList dbInput = db.DataInfluencedBy(input);
result = methodWithoutAnd.DataInfluencedBy(dbInput, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
result.Add(whereMeth);