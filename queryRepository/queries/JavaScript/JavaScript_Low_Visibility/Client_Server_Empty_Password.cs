//Server_Empty_Password

CxList allPass = All_Passwords(); 
CxList members = Find_MemberAccesses();
CxList values = Find_String_Short_Name(members, "*value", false);

CxList targetOfValues = values.GetMembersOfTarget();		//find value.xxx
values -= targetOfValues.GetTargetOfMembers();				//ends with value

//finds xxx within password.xxx - password.value
CxList targetOfPass = allPass.GetMembersOfTarget();	
targetOfPass -= values;

//includes all permutations of password, .password, .password.value
allPass -= targetOfPass.GetTargetOfMembers();

CxList equalsEmpty = Find_String_Literal();
equalsEmpty = equalsEmpty.FindByShortName("");
equalsEmpty = equalsEmpty.FindByAssignmentSide(CxList.AssignmentSide.Right);
equalsEmpty = equalsEmpty.GetFathers();

result = All.GetByAncs(equalsEmpty) * allPass;