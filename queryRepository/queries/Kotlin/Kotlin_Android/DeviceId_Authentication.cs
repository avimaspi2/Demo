List<string> idMembers = new List<string>() {"deviceId", "imei", "meid", "getDeviceId", "getImei", "getMeid"};

CxList telephonyMembers = All.GetMembersWithTargets(All.FindByType("TelephonyManager"));
telephonyMembers = telephonyMembers.FindByShortNames(idMembers);

CxList remoteOutputs = Find_Remote_Requests();
result = remoteOutputs.DataInfluencedBy(telephonyMembers)
	.ReduceFlow(Checkmarx.DataCollections.CxQueryProvidersInterface.CxList.ReduceFlowType.ReduceBigFlow);