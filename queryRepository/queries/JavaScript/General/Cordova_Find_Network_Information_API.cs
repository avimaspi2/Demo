/* This query finds Apache Cordova's Network Connection API usages.
   Reference: https://cordova.apache.org/docs/en/latest/reference/cordova-plugin-network-information/index.html */

CxList methods = Find_Methods();
CxList navigatorConnection = All.FindByName("navigator.connection");
List <string> networkConnectionAPI = new List<string> {"type"};

result = navigatorConnection.GetMembersOfTarget().FindByShortNames(networkConnectionAPI);
result.Add(methods.FindByShortNames(networkConnectionAPI).DataInfluencedBy(navigatorConnection).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));