/** Construct CxList that we use to eliminate the uses of variables in the following cases:
/** 1. Right side of assignment;
/** 2. Right side of initialization.
/** 3. Variable member access
/** 4. Variable evaluation Inside conditions
/** 5. Variable use as parameter;
/** 6. Variable that is returned by a fucntion.
**/
CxList assignRef = Find_AssignRefNotInfluencing();

CxList assignRefInsideConditions = assignRef.GetByAncs(Find_Conditions());
//Find all assignRef entities that have any member access(this potentially alter the system behaviour.
CxList assignRefMemberAccess = assignRef.GetTargetsWithMembers();
//Find All assignRef entities that are refereced as parameters
CxList paramVariables = assignRef.GetByAncs(assignRef.GetAncOfType(typeof(Param)));
//Find all right side references of assignRef
CxList assignRefRightSide = assignRef.GetByAncs(All.FindByAssignmentSide(CxList.AssignmentSide.Right));

//Find all declarators that are directly influenced by assignRef
CxList declInfluencedByAssignRef = assignRef.GetByAncs(assignRef.GetAncOfType(typeof(Declarator)));

CxList assignRefReturnVal = assignRef.GetByAncs(assignRef.GetAncOfType(typeof(ReturnStmt)));

CxList usedAssignRef = All.NewCxList();

usedAssignRef.Add(assignRefInsideConditions);
usedAssignRef.Add(assignRefMemberAccess);
usedAssignRef.Add(paramVariables);
usedAssignRef.Add(assignRefRightSide);
usedAssignRef.Add(declInfluencedByAssignRef);
usedAssignRef.Add(assignRefReturnVal);

result = usedAssignRef;