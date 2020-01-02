//////////////////////////////////////////////////////
// Query Debuggable_App
//
// This query looks for debuggable apps 
// (android:debuggable="true" in the manifest).
// This is low risk because you can't upload a debuggable 
// app to the Play store.
//////////////////////////////////////////////////////

CxList strings = Find_Strings();
CxList settings = Find_Android_Settings();
CxList true_ = strings.FindByShortName("true");
CxList debuggable = settings.FindByName("MANIFEST.APPLICATION.ANDROID_DEBUGGABLE");

result = debuggable.DataInfluencedBy(true_);