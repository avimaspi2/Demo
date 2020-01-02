String[] ldapInitStrings = new string[] {
	"initialize",
	"open"
	};

CxList imports = All.NewCxList();
if (param.Length == 1)
{
	try
	{
		imports = param[0] as CxList;
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
} else {
	imports = Find_Imports();
}

CxList ldap_init_methods = Find_Methods_By_Import("ldap", ldapInitStrings, imports);
CxList ldapobjs = ldap_init_methods.GetAssignee();

ldapobjs = All.FindAllReferences(ldapobjs);

CxList ldap_methods = ldapobjs.GetMembersOfTarget().FindByType(typeof(MethodInvokeExpr)); 
CxList ldapMethodsInjectable = ldap_methods.FindByShortNames(new List<string>
	{"add", "add_s", "add_ext", "add_ext_s", "compare", "compare_s", "compare_ext", "compare_ext_s",
		"delete", "delete_s", "delete_ext", "delete_ext_s", "modify", "modify_s", "modify_ext",
		"modify_ext_s", "modrdn", "modrdn_s", "rename", "rename_s", "search", "search_s", "search_ext", "search_ext_s"});
result = All.GetParameters(ldapMethodsInjectable);