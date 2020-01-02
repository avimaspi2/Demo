//Query Improper_Verification_Of_Intent_By_Broadcast_Receiver
////////////////////////////////////////////////////////////
//This query finds intents which are received by a BroadcastReceiver and their source is not verified

CxList onReceive = Find_MethodDecls().FindByShortName("onReceive");

CxList paramsList =  Find_ParamDecl();

CxList unknownRef = Find_UnknownReference();

CxList switchExpr = Find_SwitchExpr();

CxList onReceiveIntentAux = paramsList.GetParameters(onReceive, 1).FindByType("Intent");

//Remove the cases that we only have a intent as parameter
CxList onReceiveIntent = All.NewCxList();
foreach(CxList aux in onReceiveIntentAux){
	if(All.FindAllReferences(aux).Count > 1){
		onReceiveIntent.Add(aux);
	}
}

CxList conditions = Find_Conditions();
//intent.action
CxList targetMembers = Find_MemberAccesses().FindByShortName("action").GetTargetOfMembers();
//intent.getAction()
targetMembers.Add(Find_Methods().FindByShortNames(new List<string>{"getAction" , "cloneFilter", 
		"filterEquals", "filterHashCode"}).GetTargetOfMembers());
//if(x == y)
conditions.Add(unknownRef.FindByFathers(Find_BinaryExpr().GetByAncs(conditions)));
//if(x.equals(y))
CxList intentCheck = conditions.DataInfluencedBy(targetMembers).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
//if (intent.action == x)
intentCheck.Add(paramsList.FindDefinition(targetMembers.GetByAncs(conditions)));
//when(intent.action)
intentCheck.Add(targetMembers.GetByAncs(switchExpr));

//val intentName = intent.action
//when(intentName)
intentCheck.Add(All.FindDefinition(unknownRef.FindByFathers(switchExpr)).DataInfluencedBy(targetMembers).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly));

result = onReceiveIntent - paramsList.FindDefinition(intentCheck);