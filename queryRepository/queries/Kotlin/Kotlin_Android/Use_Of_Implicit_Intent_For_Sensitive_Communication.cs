CxList intent = All.FindByType("Intent");
CxList intentDecl = intent.FindByType(typeof(Declarator));
CxList intentCtor = intent.FindByType(typeof(ObjectCreateExpr));
CxList sensitiveInfo = Find_Personal_Info() - Find_Strings();
CxList methods = Find_Methods();
CxList unknownRefs = Find_UnknownReference();
CxList parameters = Find_ParamDecl();

CxList intentCtorParam = parameters.GetParameters(intentCtor, 1);
intentCtorParam.Add(parameters.GetParameters(intentCtor, 2));

CxList intentCtorComponent = intentCtorParam.FindByType("Class");
intentCtorComponent.Add(intentCtorParam.FindByMemberAccess("class.java"));

CxList explicitIntentCtor = intentCtorComponent.GetAncOfType(typeof(ObjectCreateExpr));
CxList implicitIntentDecl = intentDecl.FindByInitialization(intentCtor - explicitIntentCtor);

CxList intentMemberAccess = methods.FindByMemberAccess("Intent.*");

// Find explicit intent
CxList component = All.NewCxList();
CxList explicitIntentMethods = intentMemberAccess.FindByShortNames(new List<string>{"setComponent", "setClass", 
		"setClassName", "setPackage", "setSelector"});
component.Add(explicitIntentMethods);

CxList explicitIntent = All.FindDefinition(component.GetTargetOfMembers());

CxList implicitIntent = implicitIntentDecl - explicitIntent;

CxList extras = intentMemberAccess.FindByShortNames(new List<string>{"putExtra","putStringArrayListExtra",
		"putExtras","replaceExtras"});
extras = extras.FindByParameters(unknownRefs.FindAllReferences(sensitiveInfo));

CxList implicitIntentRefs = unknownRefs.FindAllReferences(implicitIntent);

implicitIntent = implicitIntentRefs.GetTargetsWithMembers(extras);

// Find where the intent is used
CxList activities = All.NewCxList();
List < string > methodNames = new List<string>{"startActivity", "startActivities", "startActivityForResult", 
		"startActivityFromChild", "startActivityFromFragment", "startActivityIfNeeded", "startNextMatchingActivity",
		"startService", "startForegroundService", "startIntentSender", "startIntentSenderForResult","startIntentSenderFromChild"};

activities.Add(methods.FindByShortNames(methodNames));
activities = activities.FindByParameters(implicitIntentRefs);

result = activities.DataInfluencedBy(All.GetParameters(extras, 1));