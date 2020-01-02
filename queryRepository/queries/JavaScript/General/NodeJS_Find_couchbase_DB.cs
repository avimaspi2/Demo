//heuristic. if a require("couchbase") and openbucket on the same file. it is assumed to be couchbase DB access.

CxList methods = Find_Methods();
CxList requireCB = Find_Require("couchbase");

CxList constructor = methods.FindByShortName("CxConstr*");
CxList definitions = All.FindDefinition(constructor);
CxList constructorDefinition = All.NewCxList();
foreach (CxList def in definitions)
{
	string filename = string.Empty;
	try
	{
		CSharpGraph csg = def.GetFirstGraph();
		if (csg.LinePragma != null && csg.LinePragma.FileName != null)
		{
			filename = csg.LinePragma.FileName;
			if (filename.ToLower().EndsWith("couchbase.js"))
				constructorDefinition.Add(def);
		}
	}
	catch(Exception e)
	{
		cxLog.WriteDebugMessage(e);
	}
}
requireCB.Add(constructor.FindAllReferences(constructorDefinition));

CxList openBucket = methods.FindByShortName("openBucket", false);
CxList relevantOpenBucket = All.NewCxList();


foreach (CxList cb in requireCB)
{
	try
	{
		int fileId = -1;
		CSharpGraph cbg = cb.GetFirstGraph();
		if (cbg.LinePragma != null)
		{
			fileId = cbg.LinePragma.GetFileId();
		} 
		relevantOpenBucket.Add(openBucket.FindByFileId(fileId));
	}
	catch (Exception e)
	{
		cxLog.WriteDebugMessage(e);
	}
}

CxList bucket = relevantOpenBucket.GetAssignee();
bucket = All.FindAllReferences(bucket);
result = relevantOpenBucket;
result.Add(bucket.GetMembersOfTarget());