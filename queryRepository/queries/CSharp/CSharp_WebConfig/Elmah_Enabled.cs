//get everything from files named "*web.config"
CxList webConfig = All.FindByFileName("*Web.config");

//get the tag that specifies if ELMAH remote access is enabled
CxList remoteAccess = webConfig.FindByName("CONFIGURATION.ELMAH.SECURITY.ALLOWREMOTEACCESS",false);
String ara = remoteAccess.GetAssigner().GetName();
if(ara.Equals("true") || ara.Equals("1")){
	CxList allowRoles = webConfig.FindByName("CONFIGURATION.LOCATION.SYSTEM.WEB.AUTHORIZATION.ALLOW.ROLES",false);
	CxList allowUsers = webConfig.FindByName("CONFIGURATION.LOCATION.SYSTEM.WEB.AUTHORIZATION.ALLOW.USERS",false);
	CxList denyRoles = webConfig.FindByName("CONFIGURATION.LOCATION.SYSTEM.WEB.AUTHORIZATION.DENY.ROLES",false);
	CxList denyUsers = webConfig.FindByName("CONFIGURATION.LOCATION.SYSTEM.WEB.AUTHORIZATION.DENY.USERS",false);
	
	// It is unsafe to allow all roles (*) to remote access
	if(allowRoles.Count > 0 && allowRoles.GetAssigner().GetName().Equals("*")){
		result.Add(allowRoles);
	}
	
	// It is unsafe to allow all users (*) to remote access
	if(allowUsers.Count > 0 && allowUsers.GetAssigner().GetName().Equals("*")){
		result.Add(allowUsers);
	}
	
	// There must be at least one tag denying a set of users or roles
	if(denyRoles.Count == 0 && denyUsers.Count == 0){
		result.Add(webConfig.FindByName("CONFIGURATION.LOCATION.SYSTEM.WEB.AUTHORIZATION",false));	
	}

	// It is unsafe to deny a set of users that doesn't correspond to the set of all users (*)
	if(denyUsers.Count > 0 && !denyUsers.GetAssigner().GetName().Equals("*")){
		result.Add(denyUsers);	
	}	
}