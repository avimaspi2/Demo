/* This query finds PhoneGap's Capture API usages.
   Reference: http://docs.phonegap.com/en/edge/cordova_media_capture_capture.md.html */

CxList methods = Find_Methods();
CxList navigatorDeviceCapture = All.FindByName("navigator.device.capture");
List<string> captures = new List<string> {"captureAudio", "captureImage", "captureVideo"};

result.Add(navigatorDeviceCapture.GetMembersOfTarget().FindByShortNames(captures));
result.Add(methods
	.FindByShortNames(captures)
	.DataInfluencedBy(navigatorDeviceCapture)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));