//Non_Encrypted_Data_Storage
//---------------------------
//This query finds non-encrypted data saved to shared storage resources

CxList outputs = All.FindByMemberAccess("Editor.putString");
outputs.Add(All.FindByMemberAccess("Editor.putStringSet"));
outputs.Add(All.FindByMemberAccess("OutputStream.write"));

CxList sharedPreferencessEditor = All.FindByMemberAccess("Context.getSharedPreferences").GetMembersOfTarget().FindByShortName("edit");
sharedPreferencessEditor.Add( All.FindByMemberAccess("SharedPreferences.edit"));


sharedPreferencessEditor = sharedPreferencessEditor.GetMembersOfTarget();

outputs.Add(sharedPreferencessEditor.FindByShortName("putString"));
outputs.Add(sharedPreferencessEditor.FindByShortName("putStringSet"));

CxList sanitizers = All.GetParameters(outputs, 0);
sanitizers -= All.GetParameters(All.FindByMemberAccess("OutputStream.write"), 0);// new 

sanitizers.Add(outputs.GetTargetOfMembers());
CxList encrypt = Find_Encrypt();
sanitizers.Add(encrypt);

CxList values = Find_UnknownReference();
values -= values.GetMembersOfTarget().GetTargetOfMembers();
values -= values.DataInfluencedBy(encrypt) * values;
result = values.InfluencingOnAndNotSanitized(outputs, sanitizers).ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);