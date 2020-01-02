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

CxList ldapInitMethods = Find_Methods_By_Import("ldap", ldapInitStrings, imports);
CxList ldapVars = ldapInitMethods.GetAncOfType(typeof(AssignExpr));	
ldapVars.Add(ldapInitMethods.GetAncOfType(typeof(Declarator))); // Assign or declare

CxList ldapobjs = All.GetByAncs(ldapVars).FindByAssignmentSide(CxList.AssignmentSide.Left); // assigned to

ldapobjs = All.FindAllReferences(ldapobjs);

CxList ldapMethods = ldapobjs.GetMembersOfTarget(); // Everything of the form {ldapobj}.* 
result = ldapMethods.FindByType(typeof(MethodInvokeExpr));
result.Add(ldapMethods.FindByType(typeof(MethodRef)));
result = All.GetByAncs(result);