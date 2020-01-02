CxList stringLiteral = Find_String_Literal();
CxList methodList = Find_Methods();
CxList unknownRef = Find_UnknownReference();
CxList paramList = Find_Param();
CxList binaryExpr = Find_BinaryExpr();

//script.substring(11) as parameter
CxList parameterInvokeExpr = methodList.FindByFathers(paramList);
paramList.Add(parameterInvokeExpr.GetLeftmostTarget());

//inputs that are influence by tostring
CxList myInputs = Find_Android_Interactive_Inputs();
CxList toStringList = methodList.FindByShortName("toString");
CxList inputString = toStringList.InfluencedBy(myInputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);

//loadUrl
CxList loadUrls = methodList.FindByShortName("loadUrl");
//string sanitizer
CxList stringSanitizer = stringLiteral - stringLiteral.FindByShortName("javascript*");
CxList binarySanitizer = All.FindByFathers(stringSanitizer.GetAncOfType(typeof(BinaryExpr)));
result.Add(loadUrls.InfluencedByAndNotSanitized(inputString, binarySanitizer).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

//loadDataWithBaseURL
//this means that if mimeType is "text/html" or any other mime type that renders HTML
//"data" is influenced by user input, an attacker 
CxList myStrings = stringLiteral.FindByShortName("text/html");
CxList renderHtml = paramList.FindByName("*text/html*");
renderHtml.Add(unknownRef.FindAllReferences(myStrings.GetAssignee()));
CxList loadDataWithBaseURL = methodList.FindByShortName("loadDataWithBaseURL");

CxList loadDataWithBaseURLInfluencedByString = loadDataWithBaseURL.FindByParameters(renderHtml);

CxList loadDataWithBaseURLParam = paramList.InfluencedBy(inputString).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
loadDataWithBaseURLParam.Add(methodList.FindByParameters(loadDataWithBaseURLParam.FindByType(typeof(Param))));
loadDataWithBaseURLParam.Add(loadDataWithBaseURLParam.GetLeftmostTarget());
//when we have concacts as parameters
loadDataWithBaseURLParam.Add(binaryExpr.GetByAncs(loadDataWithBaseURLParam.GetAncOfType(typeof(ExprStmt))));
loadDataWithBaseURLParam.Add(loadDataWithBaseURLParam.GetAncOfType(typeof(BinaryExpr)));
result.Add(loadDataWithBaseURLInfluencedByString.FindByParameters(loadDataWithBaseURLParam.GetParameters(loadDataWithBaseURLInfluencedByString, 1)));

//evaluateJavascript
CxList evaluateJavascript = methodList.FindByShortName("evaluateJavascript");
result.Add(evaluateJavascript.InfluencedBy(inputString).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));