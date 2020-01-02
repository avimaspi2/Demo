// DRD01-J. Limit the accessibility of an app's sensitive content provider
// In the AndroidManifest.xml, if the content provider is exported, then other apps can access it.
// From API Level 17 and later, a content provider is private if you do not specify the attribute explicitly

CxList strings = Find_Strings();
CxList settings = Find_Android_Settings();
CxList _false = strings.FindByShortName("false");
CxList _true = strings.FindByShortName("true");
CxList androidExported = settings.FindByName("MANIFEST.APPLICATION.PROVIDER.ANDROID_EXPORTED");

// Find sdk version
CxList sdkVersionVar = settings.GetByAncs(All.FindByName("MANIFEST.USES_SDK.ANDROID_TARGETSDKVERSION"));
CxList SdkVersionVal = strings.GetByAncs(sdkVersionVar.GetAncOfType(typeof(AssignExpr)));
int sdkVersion = 0;
bool res = int.TryParse(SdkVersionVal.GetName(), out sdkVersion);

if(res) // otherwise probably not an Android project
{
	CxList providerExportedFalse = androidExported.DataInfluencedBy(_false);
	CxList providerExportedTrue = androidExported.DataInfluencedBy(_true);

	//cxLog.WriteDebugMessage("sdkVersion=" + sdkVersion + " true=" + providerExportedTrue.Count + " false=" + providerExportedFalse.Count);

	if(providerExportedTrue.Count > 0)
	{
		result = providerExportedTrue;
	}
	else if (providerExportedFalse.Count == 0 && sdkVersion < 17)
	{
		result = settings.FindByName("MANIFEST.XMLNS_ANDROID");
	}
}