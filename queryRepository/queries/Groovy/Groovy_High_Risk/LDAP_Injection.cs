CxList objCreate = Find_Object_Create();

CxList de =
	objCreate.FindByName("*InitialDirContext") +
	objCreate.FindByName("*InitialContext") +
	objCreate.FindByName("*InitialLdapContext");

CxList ds = 
	All.FindByType("*SearchControls") + 
	All.FindByMemberAccess("DirContext.search") +
	All.FindByMemberAccess("LdapContext.search") +
	All.FindByMemberAccess("Context.lookup") +
	All.FindByMemberAccess("Context.extendedOperation") +
	All.FindByMemberAccess("Context.destroySubcontext") +
	All.FindByMemberAccess("Context.list") +
	All.FindByMemberAccess("Context.listBindings") +
	All.FindByMemberAccess("Context.rebind") +
	All.FindByMemberAccess("Context.removeFromEnvironment") +
	All.FindByMemberAccess("Context.rename") +
	All.FindByMemberAccess("Context.unbind") +
	All.FindByMemberAccess("Context.getAttributes") +
	All.FindByMemberAccess("Context.modifyAttributes");

CxList inputs = Find_Interactive_Inputs();
CxList deParams = All.GetByAncs(de); 

CxList sanitize = Find_LDAP_Injection_Sanitizer();
					

result = (deParams + ds).InfluencedByAndNotSanitized(inputs, sanitize);