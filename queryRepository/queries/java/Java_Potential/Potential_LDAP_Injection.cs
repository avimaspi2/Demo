CxList objCreate = Find_Object_Create();

CxList de = objCreate.FindByName("*InitialDirContext");
de.Add(objCreate.FindByName("*InitialContext"));
de.Add(objCreate.FindByName("*InitialLdapContext"));

CxList ds = All.FindByType("*SearchControls"); 
ds.Add(All.FindByMemberAccess("DirContext.search"));
ds.Add(All.FindByMemberAccess("LdapContext.search"));
ds.Add(All.FindByMemberAccess("Context.lookup"));
ds.Add(All.FindByMemberAccess("Context.extendedOperation"));
ds.Add(All.FindByMemberAccess("Context.destroySubcontext"));
ds.Add(All.FindByMemberAccess("Context.list"));
ds.Add(All.FindByMemberAccess("Context.listBindings"));
ds.Add(All.FindByMemberAccess("Context.rebind"));
ds.Add(All.FindByMemberAccess("Context.removeFromEnvironment"));
ds.Add(All.FindByMemberAccess("Context.rename"));
ds.Add(All.FindByMemberAccess("Context.unbind"));
ds.Add(All.FindByMemberAccess("Context.getAttributes"));
ds.Add(All.FindByMemberAccess("Context.modifyAttributes"));

CxList inputs = Find_Potential_Inputs();
CxList deParams = All.GetByAncs(de); 

CxList sanitize = Find_LDAP_Sanitize();	

CxList deParamsDs = All.NewCxList();
deParamsDs.Add(deParams);
deParamsDs.Add(ds);

result = deParamsDs.InfluencedByAndNotSanitized(inputs, sanitize);