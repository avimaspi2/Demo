string[] types = { 
	"CompareValidator", "CustomValidator", "RangeValidator", "RegularExpressionValidator", "RequiredFieldValidator"};

CxList ClientValidators = All.FindByTypes(types, false);

CxList PagesWithClientValidators = All.GetClass(ClientValidators);

CxList ServerValidators = All.FindByName("*Page.Isvalid", false);
ServerValidators.Add(All.FindByName("*Page.Validators.Isvalid", false));
ServerValidators.Add(All.FindByName("*Page.Validate", false));
CxList PagesWithServerValidators = All.GetClass(ServerValidators);

CxList relevantPages = PagesWithServerValidators.Clone();
foreach(CxList curPagesWithServerValidators in relevantPages)
{
	try{
		CSharpGraph gr = curPagesWithServerValidators.data.GetByIndex(0) as CSharpGraph;
		PagesWithServerValidators.Add(All.FindByName(gr.FullName));
	}
	catch (Exception ex){
		cxLog.WriteDebugMessage(ex);
	}
}

result = PagesWithClientValidators - PagesWithServerValidators;