//Passing_Non_Encrypted_Data_Between_Activities
//---------------------------------------------
//This query finds non-encrypted data transmission between activities via intents

CxList outputs = All.FindByMemberAccess("Intent.putExtra");	
outputs.Add(All.FindByMemberAccess("Intent.putStringArrayListExtra"));

CxList sanitizers = All.GetParameters(outputs, 0);

CxList extras = All.FindByMemberAccess("Intent.putExtras");
extras.Add(All.FindByMemberAccess("Intent.replaceExtras"));

outputs.Add(extras);

sanitizers.Add(All.GetParameters(extras).FindByType("Intent"));//we already find non encrypted intents in this query 
sanitizers.Add(outputs.GetTargetOfMembers());
CxList encrypt = Find_Encrypt();
sanitizers.Add(encrypt);

CxList integers = Find_Integers();

CxList values = Find_UnknownReference();
values.Add(Find_Methods()); // Add data returned from methods
values -= values.GetMembersOfTarget().GetTargetOfMembers();
values -= values.GetParameters(outputs);
values -= values.DataInfluencedBy(encrypt) * values;
values -= integers;
result = values.InfluencingOnAndNotSanitized(outputs, sanitizers).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);