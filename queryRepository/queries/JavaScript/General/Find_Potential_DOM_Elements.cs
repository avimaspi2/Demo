CxList vars = Find_UnknownReference();
vars.Add(Find_Declarators());
CxList memberAccesses = Find_MemberAccesses();
vars.Add(memberAccesses);

// Add default DOM Elements
CxList domElements = Find_DOM_Elements();
result.Add(domElements);

CxList getters = memberAccesses.DataInfluencedBy(domElements).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly).FindByShortNames(new List<String>{
		"value"
		});
   
//Search for 
//document.getElementById();
result.Add(vars.FindAllReferences(getters.GetTargetOfMembers()));
CxList varDecl = Find_Assign_Lefts();
                
//Search for
//a = document.getElementById();
//or
//var a = document.getElementById();
//or
//a = document.getElementById().getElementById();
//or
//var a = document.getElementById().getElementById();
CxList getterFathers = getters.GetFathers();
CxList fathersDecl = getterFathers.FindByType(typeof(Declarator));
CxList fathersAssign = getterFathers.FindByType(typeof(AssignExpr));

CxList leftSideGetter = All.NewCxList();
leftSideGetter.Add(fathersDecl);
leftSideGetter.Add(varDecl.FindByFathers(fathersAssign));
result.Add(vars.FindAllReferences(leftSideGetter) - leftSideGetter);

//Search for
//document.getElementById().getElementById();
//and
//document.getElementById();
CxList allFathers = All.NewCxList();
allFathers.Add(fathersDecl);
allFathers.Add(fathersAssign);

CxList fathersWithoutAssign = getterFathers - allFathers;
result.Add(getters.FindByFathers(fathersWithoutAssign));
result.Add(getters.FindByFathers(fathersAssign).FindByAssignmentSide(CxList.AssignmentSide.Right));