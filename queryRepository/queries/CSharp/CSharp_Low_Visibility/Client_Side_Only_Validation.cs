CxList classDecls = Find_ClassDecl();
CxList methods = Find_Methods();
CxList memberAccess = Find_MemberAccesses();

var validatorsList = new String[] {
	"RequiredFieldValidator",
	"RangeValidator",
	"RegularExpressionValidator",
	"CompareValidator",
	"CustomValidator"};

CxList ClientValidators = All.FindByTypes(validatorsList);

CxList ClientValidatorsDeclarations = classDecls.FindByShortNames(validatorsList.ToList());	

CxList PagesWithClientValidators = classDecls.GetClass(ClientValidators) - ClientValidatorsDeclarations;

CxList ServerValidators = memberAccess.FindByMemberAccess("Page.IsValid");
ServerValidators.Add(memberAccess.FindByMemberAccess("Page.Validators.IsValid"));
ServerValidators.Add(methods.FindByMemberAccess("Page.Validate"));
CxList PagesWithServerValidators = classDecls.GetClass(ServerValidators);

CxList relevantPages = PagesWithServerValidators.Clone();

foreach(CxList curPagesWithServerValidators in relevantPages)
{
	CSharpGraph gr = curPagesWithServerValidators.data.GetByIndex(0) as CSharpGraph;
	PagesWithServerValidators.Add(classDecls.FindByShortName(gr.FullName));
}

result = PagesWithClientValidators - PagesWithServerValidators;