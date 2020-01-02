/* This query finds PhoneGap's Camera API usages.
   Reference: http://docs.phonegap.com/en/edge/cordova_camera_camera.md.html */

CxList methods = Find_Methods();
CxList navigatorCamera = All.FindByName("navigator.camera");
List <string> cameraAPI = new List<string> {"getPicture", "cleanup"};

result.Add(navigatorCamera.GetMembersOfTarget().FindByShortNames(cameraAPI));
result.Add(methods.FindByShortNames(cameraAPI).DataInfluencedBy(navigatorCamera).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));