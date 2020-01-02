/*
In this query we look for input and output streams that are opened and never closed.
There are 2 scenarios that we deal with:
1. Open with no close.
2. Open with a close statement in the end of the mehtod, but with also a return statement 
   in the middle of the code, without closing.

We do not deal with more complex cases, such as closing in another method. We assume that
the close should be in the same method.
*/

CxList relevantStreams = All.FindByMemberAccess("NSInputStream.*");
relevantStreams.Add(All.FindByMemberAccess("NSOutputStream.*"));
relevantStreams.Add(All.FindByMemberAccess("InputStream.*"));
relevantStreams.Add(All.FindByMemberAccess("OutputStream.*"));

// Remove streams that are of properties, since we don't need to look for these
CxList relevantStreamsTargets = relevantStreams.GetTargetOfMembers();
CxList propStreams = All.FindDefinition(relevantStreamsTargets).FindByType(typeof(PropertyDecl));
relevantStreamsTargets -= relevantStreamsTargets.FindAllReferences(propStreams);
relevantStreams *= relevantStreamsTargets.GetMembersOfTarget();

relevantStreams = relevantStreams.FindByShortName("open");

CxList close = All.FindByShortName("close*");
CxList closeTargets = close.GetTargetOfMembers();
CxList relevantTargets = relevantStreams.GetTargetOfMembers();
CxList closed = closeTargets.FindAllReferences(relevantTargets);

result = relevantTargets - relevantTargets.FindAllReferences(closed).DataInfluencingOn(closed);

CxList openStreams = relevantTargets - relevantTargets.FindAllReferences(closed).DataInfluencingOn(close);
CxList goodStreams =  relevantStreams - openStreams.GetMembersOfTarget();
CxList ret = Find_ReturnStmt();
foreach (CxList str in goodStreams)
{
	CxList method = str.GetAncOfType(typeof(MethodDecl));
	CxList ret1 = ret.GetByAncs(method);
	foreach (CxList r in ret1)
	{
		CxList retStmt = r.GetAncOfType(typeof(StatementCollection));
		CxList correctClose = closeTargets.FindAllReferences(str.GetTargetOfMembers()).GetByAncs(retStmt);
		if (correctClose.Count == 0)
		{
			CSharpGraph strG = str.TryGetCSharpGraph<CSharpGraph>();
			CSharpGraph rG = r.TryGetCSharpGraph<CSharpGraph>();
			if (strG.NodeId < rG.NodeId)
			{
				result.Add(str.Concatenate(r));
			}
		}
	}
}