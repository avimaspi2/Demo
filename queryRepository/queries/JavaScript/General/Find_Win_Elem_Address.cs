CxList memberAccesses = Find_MemberAccesses(); 
CxList bigGroup = All.NewCxList(); 
CxList vars = All.NewCxList();
CxList expandedDomElements = All.NewCxList();
CxList target;

//We'll find all DOC's and window's elements
CxList domElements = Find_DOM_Elements();

// Add window's elements
CxList loc = memberAccesses.FindByShortName("location");
vars.Add(Find_UnknownReference());
vars.Add(memberAccesses);
expandedDomElements.Add(domElements);
expandedDomElements.Add(vars.FindAllReferences(loc.GetTargetOfMembers()));

// Add "location.xxx"
expandedDomElements.Add(Find_Members("location").GetTargetsWithMembers());

// Find all cases of address manipulations
if (expandedDomElements.Count > 0){
	List < string > ldocElemAddres = new List<string> {
			"href",
			"location",
			"pathname",
			"hostname",
			"hash"
			};
	CxList docElemAddres = memberAccesses.FindByShortNames(ldocElemAddres);

	foreach (CxList theAddress in docElemAddres)
	{
		target = theAddress.GetTargetOfMembers();
		while (target.Count > 0)
		{
			if ((target * expandedDomElements).Count > 0) 
			{
				bigGroup.Add(theAddress);
				break;
			}
			target = target.GetTargetOfMembers();
		}
	}
}
//"location = url;" this is address manipulation.
bigGroup.Add(vars.FindByShortName("location"));

//Add window to DOM element objects to avoid removing window and its aliases
CxList window = vars.FindByShortName("window");
window.Add(window.GetAssignee());
window.Add(All.FindAllReferences(window));

//Remove members which their targets has definition (not including DOM element objects)
CxList domElWindows = All.NewCxList();
domElWindows.Add(domElements);
domElWindows.Add(window);

CxList targets = bigGroup.GetTargetOfMembers() - domElWindows;
CxList otherTargets = targets.FindAllReferences(Find_Declarators());
bigGroup -= bigGroup.GetMembersWithTargets(otherTargets);

//find "this.get('href');" or "this.get('location');"
CxList thisRef = Find_ThisRef();
CxList thisRefGet = thisRef.GetMembersOfTarget();
bigGroup.Add(All.GetParameters(thisRefGet).FindByShortNames(new List<string>{"href","location"}));

// we don't want "LOCATION.href" and "location.HREF"
CxList twm = bigGroup.GetTargetsWithMembers();
CxList toRemove = All.NewCxList();
foreach(CxList t in twm)
{
	target = t.GetMembersOfTarget();
	if((target * bigGroup).Count > 0 )
		toRemove.Add(t);
}
result = (bigGroup - toRemove);

// remove from inputs sources, that they are Parameters of anonym.function 
CxList lambdaFunctions = Find_LambdaExpr();
CxList lambdaParameters = result.GetParameters(lambdaFunctions);
result -= lambdaParameters;