result = Find_Member_Access("xmlhttprequest.open");

CxList httpRequest = All.FindByMemberAccess("window.createrequest");
result.Add(Find_Command_From_Http(httpRequest, "open"));

CxList createObj = All.FindByType(typeof(ObjectCreateExpr));
httpRequest = createObj.FindByShortName("activexobject");
CxList xmlhttp = All.FindByShortName("*.xmlhttp*");
httpRequest = httpRequest.FindByParameters(xmlhttp);
result.Add(Find_Command_From_Http(httpRequest, "open"));