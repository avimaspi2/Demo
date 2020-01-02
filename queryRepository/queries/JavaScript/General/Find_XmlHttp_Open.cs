result = Find_Members("XMLHttpRequest.open");

CxList createObj = Find_ObjectCreations();
CxList httpRequest = createObj.FindByShortName("ActiveXObject");
CxList xmlhttp = All.FindByShortName("*.XMLHTTP*");
httpRequest = httpRequest.FindByParameters(xmlhttp);

httpRequest.Add(Find_Members("window.createRequest"));
httpRequest.Add(createObj.FindByShortName("XMLHttpRequest"));
result.Add(Find_Command_From_Http(httpRequest, "open"));