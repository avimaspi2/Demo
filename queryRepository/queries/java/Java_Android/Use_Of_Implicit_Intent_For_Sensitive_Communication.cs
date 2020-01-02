//Query Use_Of_Implicit_Intent_For_Sensitive_Communication
//--------------------------------------------------------
//This query finds implicit intents which have no specified destination component or application

CxList intent = All.FindByType("Intent");
CxList intentDecl = intent.FindByType(typeof(Declarator));
CxList intentCtor = intent.FindByType(typeof(ObjectCreateExpr));
CxList sensitiveInfo = Find_Personal_Info();
CxList methods = Find_Methods();

CxList intentCtorParam = All.GetParameters(intentCtor, 1);
intentCtorParam.Add(All.GetParameters(intentCtor, 2));

CxList intentCtorComponent = intentCtorParam.FindByType("Class");
intentCtorComponent.Add(intentCtorParam.FindByShortName("*class*", false));

CxList explicitIntentCtor = intentCtorComponent.GetAncOfType(typeof(ObjectCreateExpr));
CxList implicitIntentDecl = intentDecl.FindByInitialization(intentCtor - explicitIntentCtor);

// Find explicit intent
CxList component = All.NewCxList();
component.Add(All.FindByMemberAccess("Intent.setComponent"));
component.Add(All.FindByMemberAccess("Intent.setClass"));
component.Add(All.FindByMemberAccess("Intent.setClassName"));
component.Add(All.FindByMemberAccess("Intent.setPackage")); 
component.Add(All.FindByMemberAccess("Intent.setSelector"));

CxList explicitIntnet = All.FindDefinition(component.GetTargetOfMembers());

CxList implicitIntent = implicitIntentDecl - explicitIntnet;

// Find where the intent is used
CxList activity = All.NewCxList();
activity.Add(methods.FindByName("startActivity"));
activity.Add(methods.FindByName("startActivities"));
activity.Add(methods.FindByName("startActivityForResult"));
activity.Add(methods.FindByName("startActivityFromChild"));
activity.Add(methods.FindByName("startActivityFromFragment"));
activity.Add(methods.FindByName("startActivityIfNeeded"));
activity.Add(methods.FindByName("startService"));

// Find the call to the activity using the implicit intent
CxList sentIntent = activity.InfluencedBy(implicitIntent).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

result = sentIntent.InfluencedBy(sensitiveInfo);