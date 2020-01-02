CxList methods = Find_Methods();
CxList inputs = Find_Interactive_Inputs();

CxList header = methods.FindByName("header");

//Having a Content-Disposition header will generally act as sanitizer, look for them
CxList header_params = All.GetParameters(header);
header_params.Add(All.GetByAncs(header_params));
header_params = header_params.FindByType(typeof(StringLiteral));

CxList cont_disp = header_params.FindByShortName("Content-Disposition*");

//List of files that have the Content-Disposition header
List<string> files_with_cd = new List<string>();
foreach (CxList elem in cont_disp)
{
	CSharpGraph fName = elem.data.GetByIndex(0) as CSharpGraph;
	files_with_cd.Add(fName.LinePragma.FileName);
}

//Find urls with known RFD code smells
CxList strings = Find_Strings();
CxList suspects = strings.FindByShortName("*jsonp*");
suspects.Add(strings.FindByShortName("*callback=*"));
suspects.Add(strings.FindByShortName("*cb=*"));
suspects.Add(strings.FindByShortName("*accept=jsonp*"));
suspects.Add(strings.FindByShortName("*format=json2*"));

CxList get_methods = methods.FindByShortNames(new List<string>(){"file_get_contents", "curl_*"});

//Check if those urls are passed to get methods
CxList inf_on_get_methods = get_methods.InfluencedBy(suspects);

//DoubleQuotedStrings (MethodInvokeExpr) of suspects strings
CxList dqsMethods = methods.FindByParameters(suspects).FindByShortName("$_DoubleQuotedString");
//Inputs influencing on those strings
CxList inputs_inf_on_urls = inputs.InfluencingOn(dqsMethods);
//Inputs influencing on suspect strings through concatenation ('.')
inputs_inf_on_urls.Add(suspects.GetFathers().InfluencedBy(inputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly));

//Add Inputs influencing on url APIs and then on get methods
inf_on_get_methods.Add(get_methods.InfluencedBy(inputs_inf_on_urls));

//Check if the files that have the vulnerability are protected by the Content-Disposition header
CxList vuln = All.NewCxList();
foreach (CxList paramFlow in inf_on_get_methods.GetCxListByPath())
{
	CxList last = paramFlow.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	CSharpGraph fName = last.data.GetByIndex(0) as CSharpGraph;
	string fileName = fName.LinePragma.FileName;
	if (!files_with_cd.Contains(fileName))
	{
		vuln.Add(paramFlow);
	}
}

result = vuln;