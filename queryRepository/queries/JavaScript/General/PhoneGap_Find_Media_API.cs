/* This query finds PhoneGap's Media API usages.
   Reference: http://docs.phonegap.com/en/edge/cordova_media_media.md.html */

CxList methods = Find_Methods();
CxList newObjects = Find_ObjectCreations();
CxList newMedia = newObjects.FindByShortName("Media");

List <string> mediaMethodsNames = new List<string> {
		"getCurrentPosition",
		"getDuration",
		"play",
		"pause",
		"release",
		"seekTo",
		"setVolume",
		"startRecord",
		"stopRecord",
		"stop",
		"getCurrentAmplitude",
		"pauseRecord",
		"resumeRecord"};

CxList mediaMethods = methods
	.FindByShortNames(mediaMethodsNames)
	.DataInfluencedBy(newMedia)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	
result.Add(newMedia);
result.Add(mediaMethods);