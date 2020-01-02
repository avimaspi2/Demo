CxList declarators = Find_Declarators();
CxList intentDecl = declarators.FindByType("Intent");
CxList intentCtor = Find_ObjectCreations().FindByType("Intent");

CxList intentCtorParam = All.GetParameters(intentCtor, 1);
intentCtorParam.Add(All.GetParameters(intentCtor, 2));

CxList intentCtorComponent = intentCtorParam.FindByType("Class");
intentCtorComponent.Add(intentCtorParam.FindByMemberAccess("class.java"));

CxList explicitIntentCtor = intentCtorComponent.GetAncOfType(typeof(ObjectCreateExpr));
CxList implicitIntentDecl = intentDecl.FindByInitialization(intentCtor - explicitIntentCtor);

CxList methods = Find_Methods();

CxList intentMemberAccess = methods.FindByMemberAccess("Intent.*");

// Find explicit intent
CxList component = All.NewCxList();
component.Add(intentMemberAccess.FindByShortNames(new List<string>{"setComponent","setClass","setClassName","setPackage","setSelector"}));

CxList explicitIntent = All.FindDefinition(component.GetTargetOfMembers());

CxList implicitIntent = implicitIntentDecl - explicitIntent;

CxList extras = (intentMemberAccess.FindByShortNames(new List<string>{"putExtra","putStringArrayListExtra","putExtras","replaceExtras"}));
extras = (All.FindAllReferences(implicitIntent).GetMembersOfTarget()) * extras;

// Get sanitizers
CxList encrypted = Find_Encrypt();
CxList sanitizers = All.NewCxList();
sanitizers.Add(encrypted);

CxList values = declarators.GetAssigner();
values -= values.FindByType("Intent");

CxList extrasParams = All.GetParameters(extras, 1);
CxList extrasParamsInfluencedByValues = extrasParams.InfluencedBy(values);
CxList sanitizedExtrasParams = extrasParamsInfluencedByValues - extrasParams.InfluencedByAndNotSanitized(values, All.FindAllReferences(encrypted));

extrasParams -= sanitizedExtrasParams;

// Find where the intent is used
CxList activities = All.NewCxList();
List < string > methodNames = new List<string>{"startActivity", "startActivities", "startActivityForResult", "startActivityFromChild", "startActivityFromFragment", "startActivityIfNeeded", "startNextMatchingActivity", "startService", "startForegroundService", "startIntentSender", "startIntentSenderForResult","startIntentSenderFromChild", "sendBroadcast"};
activities.Add(methods.FindByShortNames(methodNames));

result = extrasParams.InfluencingOn(activities).ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);