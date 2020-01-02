CxList reference = All.FindByShortName("pagereference") + 
	All.FindByShortName("currentpage");
reference = All.GetByAncs(reference) - reference;

CxList referencePath = reference + All.FindAllReferences(reference);
referencePath = All.GetByAncs(referencePath) + referencePath.GetAncOfType(typeof(AssignExpr));

CxList inputs = All * Find_Url_Current_Page() * Find_Interactive_Inputs();
CxList sanitize = Find_Integers() + Find_Id_Sanitizers() + Find_Test_Code() + Find_NonLeft_Binary(referencePath);

reference -= reference.GetByAncs(Find_Boolean_Conditions());

CxList refInputs = inputs * reference - sanitize;
sanitize -= reference;
reference -= Find_NonLeft_Binary(reference) + reference.FindByType(typeof(BinaryExpr));
result = inputs.InfluencingOnAndNotSanitized(reference, sanitize) + refInputs;

result -= Find_Test_Code();
result -= result.DataInfluencedBy(result);