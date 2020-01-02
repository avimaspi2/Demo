CxList editTexts = All.FindByType("EditText");

CxList password = Find_All_Passwords();
password.Add(Find_Personal_Info());
CxList sensitiveEditTexts = editTexts * password;

//Remove EditText which set TYPE_TEXT_VARIATION_PASSWORD.
CxList memberAccesses = Find_MemberAccess();
CxList binaryExpression = Find_BinaryExpr();

CxList potentialParameters = All.NewCxList();
potentialParameters.Add(memberAccesses);
potentialParameters.Add(binaryExpression);

CxList passwordField = memberAccesses.FindByShortName("TYPE_TEXT_VARIATION_PASSWORD");
CxList setInputType = sensitiveEditTexts.GetMembersOfTarget().FindByShortName("setInputType");
CxList safeSetInputType = setInputType.FindByParameters
	(passwordField.GetByAncs(potentialParameters.GetParameters(setInputType)));
sensitiveEditTexts -= editTexts.FindAllReferences(safeSetInputType.GetTargetOfMembers());

//Remove EditText which defines Custom Selection Action Mode
CxList customSelectionCallBackMethod = sensitiveEditTexts.GetMembersOfTarget()
	.FindByShortName("setCustomSelectionActionModeCallback");
sensitiveEditTexts -= editTexts.FindAllReferences(customSelectionCallBackMethod.GetTargetOfMembers());

//Remove cache clearing methods when exiting the application or when going to the backgorund
CxList classes = Find_Class_Decl();
List<string> allActivities = new List<string> {"PreferenceActivity", "Activity", "FragmentActivity", "ListActivity",
		"AppCompatActivity"};
CxList activity = classes.FindByShortName("*Activity");
foreach(string activityType in allActivities)
{
	activity.Add(classes.InheritsFrom(activityType));
}

CxList activityMethods = Find_MethodDeclaration().GetByAncs(activity);
CxList closeMethods = activityMethods.FindByShortNames(new List<string> {"onDestroy", "onPause", "onStop"});
CxList methods = Find_Methods();
CxList setPrimaryClip = methods.FindByMemberAccess("ClipboardManager.setPrimaryClip");

CxList relevantItems = All.NewCxList(); ;
relevantItems.Add(sensitiveEditTexts);
relevantItems.Add(closeMethods);

foreach(CxList activityClass in activity)
{	
	CxList relevantInActivity = relevantItems.GetByAncs(activityClass);
	if(setPrimaryClip.GetByAncs(closeMethods * relevantInActivity).Count > 0)
	{
		sensitiveEditTexts -= sensitiveEditTexts * relevantInActivity;
	}
}
result = All.FindDefinition(sensitiveEditTexts);