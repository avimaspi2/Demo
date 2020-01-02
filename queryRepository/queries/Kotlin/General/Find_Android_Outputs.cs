/////////////////////////////////////////////////////
// Query Find_Android_Outputs
// Purpose: Find outputs of Android apps
/////////////////////////////////////////////////////

// Intents
CxList methods = Find_Methods();
CxList activities = methods.FindByShortNames(new List<string> {
		"bindService","sendBroadcast*","sendOrderedBroadcast","sendStickyBroadcast","sendStickyOrderedBroadcast",
		"startActivity*","startActivities","startNextMatchingActivity","startService"});
CxList intents = activities.GetMembersWithTargets();
CxList cd = Find_ClassDecl();

// A list of all classes derived from ContextWrapper (where startActivity is declared)
List < string > contextWrapperList = new List<string> {"Activity","Service", "Application", "BackupAgent", "IsolatedContext",
		"MutableContextWrapper", "RenamingDelegatingContext", "AbstractInputMethodService", "AccessibilityService",
		"AccountAuthenticatorActivity", "ActionBarActivity", "ActivityGroup", "AliasActivity", "AppCompatActivity",
		"BackupAgentHelper", "CarrierMessagingService", "DreamService", "ExpandableListActivity", "FragmentActivity",
		"HostApduService", "InputMethodService", "IntentService", "JobService", "LauncherActivity", "ListActivity",
		"MediaBrowserService", "MediaRouteProviderService", "MockApplication", "MultiDexApplication", "NativeActivity",
		"NotificationCompatSideChannelService", "NotificationListenerService", "OffHostApduService", "PreferenceActivity",
		"PrintService", "RecognitionService", "RemoteViewsService", "SettingInjectorService", "SpellCheckerService",
		"TabActivity", "TextToSpeechService", "TvInputService", "VoiceInteractionService", "VoiceInteractionSessionService",
		"VpnService", "WallpaperService"};
CxList contextWrappers = All.NewCxList();
foreach(string ctx in contextWrapperList)
{
	contextWrappers.Add(cd.InheritsFrom(ctx));
}

CxList activityClasses = cd.FindByShortNames(contextWrapperList);
activityClasses.Add(contextWrappers);

intents.Add(activities.GetByAncs(activityClasses));


intents -= All.FindByMemberAccess("LocalBroadcastManager.sendBroadcast*");

intents = All.GetParameters(intents, 0);

// Methods where the Intent is the second parameter
CxList intents2 = All.FindByMemberAccess("*.startIntentSender");
intents2.Add(All.FindByMemberAccess("*.setResult"));
intents.Add(All.GetParameters(intents2, 1));

// Views
CxList views = All.FindByMemberAccess("EditText.setText");
views.Add(All.FindByMemberAccess("TextView.setText"));

// Clipboard
CxList clipboard = All.FindByMemberAccess("ClipboardManager.setText");

// Email
CxList mail = All.FindByMemberAccess("Transport.send");

// SMS
CxList smsSendMessage = All.FindByMemberAccess("SMSManager.sendTextMessage");

// Logging
CxList log = All.FindByMemberAccess("Log.*");
log.Add(All.FindByShortName("Log").GetMembersOfTarget());

// World readable files
CxList unsafeFileMode = All.FindByShortName("MODE_WORLD_READABLE");
unsafeFileMode.Add(All.FindByShortName("MODE_WORLD_WRITEABLE"));
CxList unsafeFileOpen = unsafeFileMode.GetAncOfType(typeof(MethodInvokeExpr)).FindByShortName("openFileOutput");

// Array bound to adapter (MVC)
// Find ArrayAdaper constructor
CxList adapter = All.FindByName("ArrayAdapter").FindByType(typeof(ObjectCreateExpr));
// Find 3rd parameter - the bounded array
CxList adapterParam = All.GetParameters(adapter, 2);
// Find all references of the array, and then find the "add" method
CxList array = All.FindAllReferences(adapterParam).GetMembersOfTarget().FindByShortName("add");

result = intents;
result.Add(views);
result.Add(clipboard);
result.Add(mail);
result.Add(smsSendMessage);
result.Add(log);
result.Add(unsafeFileOpen);
result.Add(array);

result.Add(All.FindByMemberAccess("TextView.text").FindByAssignmentSide(CxList.AssignmentSide.Left));
result.Add(All.FindByMemberAccess("Toast.makeText"));
result.Add(All.FindByMemberAccess("Button.text"));
result.Add(All.FindByMemberAccess("CheckBox.text"));