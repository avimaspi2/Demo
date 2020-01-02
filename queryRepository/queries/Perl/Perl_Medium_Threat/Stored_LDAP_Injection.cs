/*
	Find all Ldap queries that inluenced by unsinitized inputs.
	Ldap queries: "search", "modify", "moddn", "add", "bind", "delete" or "compare".
*/
CxList allMethodInvoke = Find_Methods();
CxList inputs = Find_Read();
CxList sanitize = Find_General_Sanitize();
//find new operator
CxList newMethodInvoke = allMethodInvoke.FindByShortName("new");
//find Net::LDAP and Net::LDAPS
CxList netLdapNew = All.GetByAncs(newMethodInvoke);
/*	new operator in "= ...->new (...)" format 
	for example $self->{ldap} = Net::LDAP->new ();*/
CxList otherNew = allMethodInvoke.FindByName("Net::LDAP.new") + allMethodInvoke.FindByName("Net::LDAPS.new");

netLdapNew = (netLdapNew.FindByShortName("Net::LDAP") + netLdapNew.FindByShortName("Net::LDAPS")).FindByType(typeof(Param)) + 
	otherNew; 

CxList newObj = netLdapNew.GetAncOfType(typeof (AssignExpr));	//get "=" of new operator
newObj = All.GetByAncs(newObj).FindByType(typeof (MemberAccess));	//new object of Net::LDAP or Net::LDAPS types

CxList execMethods = allMethodInvoke.FindByShortName("search") +
	allMethodInvoke.FindByShortName("modify") +
	allMethodInvoke.FindByShortName("moddn") +
	allMethodInvoke.FindByShortName("add") +
	allMethodInvoke.FindByShortName("bind") +
	allMethodInvoke.FindByShortName("delete") +
	allMethodInvoke.FindByShortName("compare");

//all objects of Net::LDAP or Net::LDAPS that influencing on "search", "modify", "moddn", "add", "bind", "delete" or "compare".

execMethods = newObj.DataInfluencingOn(execMethods).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

result = execMethods.InfluencedByAndNotSanitized(inputs, sanitize);