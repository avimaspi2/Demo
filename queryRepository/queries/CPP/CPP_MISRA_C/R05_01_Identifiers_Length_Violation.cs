/*
MISRA C RULE 5-1
------------------------------
This query searches for identifier pairs that are different yet are the same on the first 31 characters

	The Example below shows code with vulnerability: 

int abcdefghijklmnopqrstuvwxyz01234A;
int abcdefghijklmnopqrstuvwxyz01234B;

*/

CxList identifiers = Find_Identifiers();

// remove extern delcarations since by definition they do not "declare" an identifier, just repeat it
identifiers -= identifiers.GetByAncs(All.FindByFieldAttributes(Dom.Modifiers.Extern));

// identifiers of length < 31 cannot fit our target
foreach(CxList curIdentifier in identifiers)
{
	CSharpGraph nodeGraph = curIdentifier.data.GetByIndex(0) as CSharpGraph;
	if (nodeGraph.ShortName != null && nodeGraph.ShortName.Length < 31){
		identifiers -= curIdentifier;
	}
}

// count definitions of each identifier and add to results accordingly
foreach(CxList curIdentifier in identifiers)
{
	CSharpGraph nodeGraph = curIdentifier.data.GetByIndex(0) as CSharpGraph;
	if (nodeGraph.ShortName == null) {
		continue;
	}
	string shortName = nodeGraph.ShortName.Substring(0, Math.Min(nodeGraph.ShortName.Length, 31)) + "*";
	CxList identAppearances = identifiers.FindByShortName(shortName);
	
	// we want to add all copies of a problematic identifier pair, but only if there was a triggering
	// similiar but not same identifier
	CxList curRemovedIdentAppearances = identAppearances - identAppearances.FindByShortName(curIdentifier);
	if(curRemovedIdentAppearances.Count > 0){
		result.Add(identAppearances);
	}
}