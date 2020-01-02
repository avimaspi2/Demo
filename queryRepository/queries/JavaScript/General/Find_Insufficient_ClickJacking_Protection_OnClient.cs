//checks wheter there is insufficient clickJacking protection implemented in Code.
/*
this is done as follows: we look for window.top in if condtion that is compared to something.
In case we find one we go inside the if statement and look for the assignment
 to window.top.location.* or window.parent.location.*
*/
//all possible window top access permutations
CxList windowTopTotal = Find_Members("window.top");

CxList parentWindow = Find_Members("window.parent");
CxList topLocation = Find_Members("top.location");
topLocation = windowTopTotal.GetMembersOfTarget() * topLocation;

//get only the elements inside if condition
CxList ifStmt = Find_Ifs();

CxList ifCond = All.FindByFathers(ifStmt).FindByType(typeof(Expression));

CxList allInCondition = All.GetByAncs(ifCond);

//get all elements under if that are not in condition

CxList conditions = allInCondition.Clone();
conditions.Add(ifStmt);

CxList allElements = All - conditions;

CxList allInStmt = allElements.GetByAncs(ifStmt);

//we check for expressions of type var x=window.top if(x.location!=...)
CxList influencedBy = allInCondition.DataInfluencedBy(windowTopTotal - allInCondition);

CxList topTotalLocation = All.NewCxList();
topTotalLocation.Add(windowTopTotal);
topTotalLocation.Add(topLocation);

CxList windowTop = (allInCondition * topTotalLocation);
windowTop.Add(influencedBy);

CxList nonEqual = windowTop.GetFathers().GetByBinaryOperator(Checkmarx.Dom.BinaryOperator.IdentityInequality);

//now we get inside the if statement
CxList allInRelevantStmt = allInStmt.GetByAncs(nonEqual.GetAncOfType(typeof(IfStmt)));
//we find the relevant assignments
CxList locationHref = allInRelevantStmt.FindByShortNames(new List<string> {"href",  "location"}).GetByAncs(Find_Assign_Lefts());


//this loop goes to the first target of target of members sequence
CxList hrefWithTargets = locationHref.GetMembersWithTargets();
CxList Window = locationHref - hrefWithTargets;
Window.Add(hrefWithTargets.GetLeftmostTarget());

CxList HTMLWindow = Window.FindByShortName("window", false);
Window -= HTMLWindow;
//to add those cases where var x=window.top if(x.location!=...)

CxList windowTopParent = All.NewCxList();
windowTopParent.Add(windowTopTotal);
windowTopParent.Add(parentWindow);

result.Add(All.FindAllReferences(Window).DataInfluencedBy(windowTopParent));

result.Add(HTMLWindow);

result -= result.FindByFiles(Find_SAP_ClickJacking_Sanitize());