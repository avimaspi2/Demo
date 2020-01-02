CxList methods = Find_Methods();

CxList useOfKey = methods.FindByMemberAccess("SQLiteDatabase.openOrCreateDatabase");
CxList keys = All.GetParameters(useOfKey, 1).FindByType(typeof(UnknownReference));

CxList useOfKeyCypher = methods.FindByMemberAccess("Cipher.init");
keys.Add (All.GetParameters(useOfKeyCypher, 1).FindByType(typeof(UnknownReference)));
useOfKey.Add(useOfKeyCypher);

CxList keyDefs = All.FindDefinition(keys);
keyDefs.Add(All.GetAncOfType(typeof(FieldDecl)));

CxList keyRefs = All.FindAllReferences(keyDefs) - keyDefs;
keyRefs = keyRefs.Contained(keyRefs.InfluencingOn(useOfKey), CxList.GetStartEndNodesType.StartNodesOnly);

CxList statementCollections = keyRefs.GetAncOfType(typeof(StatementCollection));

foreach (CxList statementCollection in statementCollections)
{
	CxList keyRefInSC = keyRefs.GetByAncs(statementCollection);
	Dictionary<CxList,CxList> dic = new Dictionary<CxList,CxList>();
	foreach (CxList keyRef in keyRefInSC)
	{
		CxList keyDef = keyDefs.FindDefinition(keyRef);
		if (!dic.ContainsKey(keyDef))
		{
			CxList keyRefClone = All.NewCxList();
			keyRefClone.Add(keyRef);
			dic.Add(keyDef, keyRefClone);
			continue;
		}
		CxList otherKeyRef = dic[keyDef];
		CxList relevantDef = keyDef.FindDefinition(otherKeyRef);
		CxList keyRefFlow = useOfKey.InfluencedBy(keyRef);
		CxList otherKeyRefFlow = useOfKey.InfluencedBy(otherKeyRef);
		if (keyRefFlow.Contained(keyRefFlow,CxList.GetStartEndNodesType.EndNodesOnly) == 
			otherKeyRefFlow.Contained(otherKeyRefFlow,CxList.GetStartEndNodesType.EndNodesOnly))
		{
			continue;
		}
		result.Add(relevantDef.ConcatenatePath(otherKeyRefFlow).
			ConcatenatePath(keyRefFlow));
	}
}