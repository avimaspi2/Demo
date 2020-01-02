CxList androidSettings = Find_Android_Settings();
CxList falseLiteralsManifest = androidSettings.FindByShortName("\"false\"");
CxList clearTextTraffic = androidSettings.FindByMemberAccess("*.ANDROID_USESCLEARTEXTTRAFFIC");
result = (clearTextTraffic.GetAssigner() * falseLiteralsManifest);