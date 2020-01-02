//Query Improper_Verification_Of_Intent_By_Broadcast_Receiver
//-----------------------------------------------------------
//This query finds intents which are received by a BroadcastReceiver and their source is not verified
CxList methodDecl = Find_MethodDecls();
CxList onReceive = methodDecl.FindByShortName("onReceive");
CxList onReceiveIntent = All.GetParameters(onReceive, 1).FindByType("Intent");

CxList conditions = Find_Conditions();

CxList intentActionCheck = All.FindByMemberAccess("Intent.getAction");
intentActionCheck.Add(All.FindByMemberAccess("Intent.cloneFilter"));
intentActionCheck.Add(All.FindByMemberAccess("Intent.filterEquals"));
intentActionCheck.Add(All.FindByMemberAccess("Intent.filterHashCode"));

CxList intentCheck = intentActionCheck.DataInfluencingOn(conditions) * intentActionCheck;
CxList onReceiveIntentChecked = onReceiveIntent.DataInfluencingOn(intentCheck) * onReceiveIntent;

result = onReceiveIntent - onReceiveIntentChecked;