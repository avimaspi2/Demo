CxList classDecl = All.FindByType(typeof(ClassDecl));
CxList db = Find_DB();
CxList strings = Find_Strings();

CxList protect = All.FindByShortName("protect_from_forgery");
protect.Add(All.FindByShortName("verify"));

CxList inheritFromActionController = classDecl.InheritsFrom("ActionController.Base");

// Identify protected controller classes
CxList protectedClasses = inheritFromActionController * protect.GetAncOfType(typeof(ClassDecl));;

protectedClasses.Add(classDecl.InheritsFrom(protectedClasses));

CxList protectedContent = classDecl.FindByName(protectedClasses);

//
CxList requests = Find_Interactive_Inputs() - protectedContent;

CxList dbUpdates = 
	db.FindByShortName("*update*") +
	db.FindByShortName("*delete*") +
	db.FindByShortName("*insert*") +
	db.FindByShortName("*save*");

CxList write = strings.FindByName("*update*", StringComparison.OrdinalIgnoreCase) +
	strings.FindByName("*delete*", StringComparison.OrdinalIgnoreCase) +
	strings.FindByName("*insert*", StringComparison.OrdinalIgnoreCase) + 
	strings.FindByName("*save*", StringComparison.OrdinalIgnoreCase);

result = 
	db.DataInfluencedBy(write).DataInfluencedBy(requests) + 
	dbUpdates.DataInfluencedBy(requests);