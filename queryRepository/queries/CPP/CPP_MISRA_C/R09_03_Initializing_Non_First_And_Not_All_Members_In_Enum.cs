/*
MISRA C RULE 9-3
------------------------------
This query searches for enums where the a non first member is initialized, yet not all of the members are initialized

	The Example below shows code with vulnerability: 

typedef enum
{
   mc2_0903_red_1 = 3,
   mc2_0903_blue_1,
   mc2_0903_green_1,
   mc2_0903_yellow_1 = 5   
} mc2_0903_colour_1;

*/

// find enums
CxList enumMembers = All.FindByType(typeof(EnumMemberDecl));
CxList initMembers = enumMembers * All.FindByType(typeof(AssignExpr)).GetFathers();
CxList enums = enumMembers.GetAncOfType(typeof(ClassDecl));


// go over enums
foreach(CxList cur in enums){
	CxList curMembers = enumMembers.GetByAncs(cur);
	CxList curInits = initMembers * curMembers;
	if (curInits.Count == 0){
		continue;
	}
	if (curMembers.Count == curInits.Count){
		continue;
	}
	if (curInits.Count == 1){
		CSharpGraph firstMember = curMembers.data.GetByIndex(0) as CSharpGraph;
		if (curInits.FindById(firstMember.NodeId).Count == 1){
			continue;
		}
	}
	
	// some (not all) of the members were initialized, and it wasn't just the first member
	result.Add(cur);
}