CxList conditions = Find_Conditions();
CxList sanitizers = All.NewCxList();
sanitizers.Add(
	Find_Members("JailMonkey.isJailBroken"),
	Find_Members("JailMonkey.trustFall")
);

/*
1.
 var isRooted = JailMonkey.trustFall();
 if(isRooted){

2.
if(JM.isJailBroken() == true)
*/
result = conditions.DataInfluencedBy(sanitizers);

/*
if(JM.isJailBroken())
*/
result.Add(conditions * sanitizers);