/// <summary>
/// Finds the Geolocation related Data acquired by PhoneGap framework
/// </summary>
CxList ur = Find_UnknownReference();

// api access (navigator.geolocation.getCurrentPosition/watchPosition)
CxList location = ur.FindByShortName("navigator", true).GetMembersOfTarget()
	.FindByShortName("geolocation", true).GetMembersOfTarget()
	.FindByShortNames(new List<string>{"getCurrentPosition", "watchPosition"});

// only success handler is relevant
CxList handlers = All.GetParameters(location, 0);

// collect callback handlers
CxList geolocationHandlerDefinition = All.FindDefinition(handlers);
CxList candidates = All.GetParameters(geolocationHandlerDefinition, 0);
candidates.Add(All.GetParameters(All.FindDefinition(All.FindAllReferences(geolocationHandlerDefinition).GetAssigner(Find_LambdaExpr())), 0));

// Consider passing the Position Object to another method
CxList coords = Find_MemberAccesses().FindByShortName("coords");
CxList relevantCoords = coords.DataInfluencedBy(candidates);
result.Add(relevantCoords);

// Add two levels of Members, avoids loss of flow on third access level 
// of the position object (e.g. position.coords.latitude)
candidates.Add(All.FindAllReferences(candidates));
candidates.Add(candidates.GetMembersOfTarget());
candidates.Add(candidates.GetMembersOfTarget());

// Remove targets (keep only the rightmost member to avoid duplicates)
CxList excludeCandidates = candidates.GetTargetOfMembers();
excludeCandidates.Add(relevantCoords.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly));
candidates -= excludeCandidates;
result.Add(candidates);