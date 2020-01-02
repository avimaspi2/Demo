CxList ClientValidators = 	All.FindByType("requiredfieldvalidator") + 
					All.FindByType("rangevalidator") + 
					All.FindByType("regularexpressionvalidator") + 
					All.FindByType("comparevalidator") + 
					All.FindByType("customvalidator");

CxList PagesWithClientValidators = All.GetClass(ClientValidators);

CxList ServerValidators = All.FindByName("*page.isvalid") + 
	All.FindByName("*page.validators.isvalid") + 
	All.FindByName("*page.validate");
CxList PagesWithServerValidators = All.GetClass(ServerValidators);

foreach(CxList curPagesWithServerValidators in PagesWithServerValidators)
{
	CSharpGraph gr = curPagesWithServerValidators.data.GetByIndex(0) as CSharpGraph;
	PagesWithServerValidators.Add(All.FindByName(gr.FullName));
}

result = PagesWithClientValidators - PagesWithServerValidators;