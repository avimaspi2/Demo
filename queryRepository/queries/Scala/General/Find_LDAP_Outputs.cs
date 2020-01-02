List<string> relevantTypes = new List<string> {
		"*.Context",
		"*.DirContext",
		"*.InitialContext",
		"*.InitialDirContext",
		"*.InitialLdapContext",
		"*.LdapContext",
		"Context",
		"DirContext",
		"InitialContext",
		"InitialDirContext",
		"InitialLdapContext",
		"LdapContext"
		};

List<string> relevantMethods = new List<string> {
		"getAttributes",
		"list",
		"listBindings",
		"lookup",
		"lookupLink",
		"search"
		};

CxList methods = Find_Methods();

//In case the relevantType is inside a GenericTypeRef like Option
CxList genTypeRef = All.FindByType(typeof(GenericTypeRef)).FindByShortNames(relevantTypes).GetAncOfType(typeof(ParamDecl));

//Get the UnknownReference
CxList unknownRefs = Find_UnknownReference().FindAllReferences(genTypeRef);

//find the relevant MethodInvoke
CxList relevantMethodsInvoke = methods.FindByShortNames(relevantMethods);

//Find UnknownReferences using relevantMethods
CxList relevantUnknownRefs = relevantMethodsInvoke.GetLeftmostTarget();


//Use relevantMethods that are invoked only on the relevant UnknownReferences
foreach(CxList res in relevantMethodsInvoke)
{
	CxList temp = res.GetLeftmostTarget() * relevantUnknownRefs;
	if(temp.Count > 0)
	{
		result.Add(res);
	}
}