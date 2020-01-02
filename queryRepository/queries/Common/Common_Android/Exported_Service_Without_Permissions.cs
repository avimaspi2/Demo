///////////////////////////////////////////////////////////////////////
// CERT DRD07-J Query: Exported_Service_Without_Permissions
// An exported service must be protected with strong permissions.
// This query looks for exported services with no permission.
// A service is exported if property "exported = true" exists or
// if it has an intent-filter.
///////////////////////////////////////////////////////////////////////

CxList settings = Find_Android_Settings();
CxList strings = settings * Find_Strings();

// Find in the manifest "exported = true" of service
CxList _true = strings.FindByShortName("true");
CxList _false = strings.FindByShortName("false");
CxList exportedObjects = settings.FindByName("MANIFEST.APPLICATION.SERVICE.ANDROID_EXPORTED");
exportedObjects.Add(settings.FindByName("MANIFEST.APPLICATION.ACTIVITY.ANDROID_EXPORTED"));
exportedObjects.Add(settings.FindByName("MANIFEST.APPLICATION.PROVIDER.ANDROID_EXPORTED"));
exportedObjects.Add(settings.FindByName("MANIFEST.APPLICATION.RECEIVER.ANDROID_EXPORTED"));
CxList serviceExportedTrue = exportedObjects.DataInfluencedBy(_true);
CxList serviceExportedFalse = exportedObjects.DataInfluencedBy(_false);

CxList objectNames = settings.FindByName("MANIFEST.APPLICATION.SERVICE.ANDROID_NAME");
objectNames.Add(settings.FindByName("MANIFEST.APPLICATION.ACTIVITY.ANDROID_NAME"));
objectNames.Add(settings.FindByName("MANIFEST.APPLICATION.PROVIDER.ANDROID_NAME"));
objectNames.Add(settings.FindByName("MANIFEST.APPLICATION.RECEIVER.ANDROID_NAME"));

// Find services with intent filters
CxList intentFilters = settings.FindByName("MANIFEST.APPLICATION.SERVICE.INTENT_FILTER.ACTION.ANDROID_NAME");
intentFilters.Add(settings.FindByName("MANIFEST.APPLICATION.ACTIVITY.INTENT_FILTER.ACTION.ANDROID_NAME"));
intentFilters.Add(settings.FindByName("MANIFEST.APPLICATION.RECEIVER.INTENT_FILTER.ACTION.ANDROID_NAME"));

// Find the service section of the intent filter by going 3 levels up.
CxList intentSections = intentFilters.GetAncOfType(typeof(IfStmt));
intentSections = intentSections.GetFathers().GetAncOfType(typeof(IfStmt));
intentSections = intentSections.GetFathers().GetAncOfType(typeof(IfStmt));

// Find the exported service section in the manifest 
CxList exportedObjectsSections = serviceExportedTrue.GetAncOfType(typeof(IfStmt));
exportedObjectsSections.Add(intentSections); 
exportedObjectsSections -= serviceExportedFalse.GetAncOfType(typeof(IfStmt));

// Find the permissions of a service in the manifest
CxList permission = settings.FindByName("MANIFEST.APPLICATION.SERVICE.ANDROID_PERMISSION");
permission.Add(settings.FindByName("MANIFEST.APPLICATION.ACTIVITY.ANDROID_PERMISSION"));
permission.Add(settings.FindByName("MANIFEST.APPLICATION.PROVIDER.ANDROID_PERMISSION"));
permission.Add(settings.FindByName("MANIFEST.APPLICATION.RECEIVER.ANDROID_PERMISSION"));

// Find if a permission is declared for this service
CxList protectedServices = permission.GetAncOfType(typeof(IfStmt));
CxList badServices = exportedObjectsSections - protectedServices;

// We want to point to the bad permission if we can, and the activity otherwise (if the bad permission is implicit) 
CxList implicits = All.NewCxList();
foreach(CxList badService in badServices){
	CxList servicePermissions = exportedObjects.GetByAncs(badService);
	if (servicePermissions.Count > 0)
		result.Add(servicePermissions);
	else
		implicits.Add(badService);		
}

// Ignore Main activities because they require exporting
CxList mainActivities = All.FindByShortName("android.intent.action.MAIN");
CxList mainActivitiesAncestors = mainActivities.GetFathers().GetAncOfType(typeof(IfStmt));
mainActivitiesAncestors = mainActivitiesAncestors.GetFathers().GetAncOfType(typeof(IfStmt));
mainActivitiesAncestors = mainActivitiesAncestors.GetFathers().GetAncOfType(typeof(IfStmt));

objectNames -= objectNames.GetByAncs(mainActivitiesAncestors);
result.Add(objectNames.GetByAncs(implicits));