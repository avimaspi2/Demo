//Detects an outbound call from within batch apex, or schedulable apex.
CxList batchable = All.InheritsFrom(@"batchable") + All.InheritsFrom(@"schedulable");

batchable = All.GetByAncs(batchable);

CxList outboundCall = All.FindByMemberAccess("http.send") + 
	All.FindByMemberAccess("WebServiceCallout.invoke");

foreach(CxList call in outboundCall)
{
	foreach(CxList batch in batchable)
	{
		CxList influenced = call.DataInfluencedBy(batch);
		if(influenced.Count > 0)
		{
			result.Add(influenced);
			break;
		}
	}	
}