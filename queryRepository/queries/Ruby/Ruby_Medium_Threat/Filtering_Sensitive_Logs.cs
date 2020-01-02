CxList personal = Find_Personal_Info();
CxList paramsPersonal = personal.GetAncOfType(typeof(IndexerRef)).FindByShortName("params");

personal = personal.GetByAncs(paramsPersonal);

CxList filterParameterLogging = Find_Methods().FindByShortName("filter_parameter_logging");
CxList filterParameters = All.FindByMemberAccess("config.filter_parameters");
filterParameters = All.DataInfluencingOn(filterParameters);

CxList filtered = All.GetParameters(filterParameterLogging) + filterParameters;

CxList nonFiltered = All.NewCxList();
if (filtered.Count == 0)
{
	nonFiltered = personal;
}
else
{
	foreach (CxList p in personal)
	{
		CSharpGraph g = p.data.GetByIndex(0) as CSharpGraph;
		string name = g.ShortName.Trim(new char[] {'"'});
		if (filtered.FindByShortName(name.Trim(new char[] {'"'})).Count == 0)
		{
			nonFiltered.Add(p);
		}
	}
}


// clean double results
foreach (CxList nf in nonFiltered)
{
	CSharpGraph g = nf.data.GetByIndex(0) as CSharpGraph;
	string name = g.ShortName.Trim(new char[] {'"'});
	if (result.FindByShortName(name).Count == 0)
	{
		result.Add(nf);
	}
}