//////////////////////////////////////////////////////
// Query AllowBackup_App
//
// This query looks for backable apps 
// (android:allowbackup="true" in the manifest).
//////////////////////////////////////////////////////

//Description:
//if android:allowBackup is not set, return a result for the parent element in the XML
//if android:allowBackup is true return a result for the element which sets it
//if android:allowBackup is false do not return results


//Adding backable true

//////////////////////////////////////////////////////
// Query AllowBackup_App
//
// This query looks for backable apps 
// (android:allowbackup="true" in the manifest).
//////////////////////////////////////////////////////

//Description:
//if android:allowBackup is not set, return a result for the parent element in the XML
//if android:allowBackup is true return a result for the element which sets it
//if android:allowBackup is false do not return results


//Adding backable true
CxList strings = Find_Strings();
CxList settings = Find_Android_Settings();
CxList true_ = strings.FindByShortName("true");
CxList manifestApp = settings.FindByName("MANIFEST.APPLICATION");
CxList backable = manifestApp.GetMembersOfTarget().FindByShortName("ANDROID_ALLOWBACKUP");
CxList backableTrue = backable.GetAssigner(true_);
result.Add(backable.DataInfluencedBy(backableTrue));
//Adding all application roots
CxList false_ = strings.FindByShortName("false");
manifestApp -= manifestApp.GetTargetsWithMembers();
manifestApp = manifestApp.GetAncOfType(typeof(IfStmt));
//Removing backable false and backable true (true was already added)
CxList backableFalse = backable.GetAssigner(false_);
manifestApp -= backableTrue.GetAncOfType(typeof(IfStmt));
manifestApp -= backableFalse.GetAncOfType(typeof(IfStmt));
result.Add(manifestApp);