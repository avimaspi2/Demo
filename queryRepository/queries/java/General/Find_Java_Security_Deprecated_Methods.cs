CxList methods = Find_Methods();

result.Add(methods.FindByMemberAccesses(new string[]{
	// java.security.acl.Acl
	"Acl.addEntry",
	"Acl.checkPermission",
	"Acl.entries",
	"Acl.getName",
	"Acl.getPermissions?",
	"Acl.removeEntry",
	"Acl.setName?",
	"Acl.toString",
	// java.security.acl.Acl
	"AclEntry.addPermission",
	"AclEntry.checkPermission",
	"AclEntry.clone",
	"AclEntry.getPrincipal",
	"AclEntry.isNegative",
	"AclEntry.permissions",
	"AclEntry.removePermission?",
	"AclEntry.setNegativePermissions",
	"AclEntry.setPrincipal",
	"AclEntry.toString",
	// java.security.acl.Acl
	"Group.addMember",
	"Group.isMember",
	"Group.members",
	"Group.removeMember",
	// java.security.acl.Acl
	"Owner.addOwner",
	"Owner.deleteOwner?",
	"Owner.isOwner?",
	// java.security.acl.Acl
	"Permission.?equals",
	"Permission.toString",
	// java.security.Certificate
	"Certificate.decode",
	"Certificate.encode",
	"Certificate.getFormat",
	"Certificate.getGuarantor",
	"Certificate.getPrincipal",
	"Certificate.getPublicKey",
	"Certificate.toString",
	// java.security.Identity
	"Identity.addCertificate",
	"Identity.certificates",
	"Identity.equals",
	"Identity.getInfo",
	"Identity.getName",
	"Identity.getPublicKey",
	"Identity.getScope",
	"Identity.hashCode",
	"Identity.identityEquals",
	"Identity.removeCertificate",
	"Identity.setInfo",
	"Identity.setPublicKey",
	"Identity.toString",
	// java.security.IdentityScope
	"IdentityScope.addIdentity",
	"IdentityScope.getIdentity",
	"IdentityScope.getSystemScope",
	"IdentityScope.identities",
	"IdentityScope.removeIdentity",
	"IdentityScope.setSystemScope",
	"IdentityScope.size",
	"IdentityScope.toString",
	// java.security.Principal
	"Principal.name",
	// java.security.Security
	"Security.getAlgorithmProperty",
	// java.security.Signature
	"Signature.getParameter",
	// java.security.Signer
	"Signer.getPrivateKey",
	"Signer.setKeyPair",
	"Signer.toString",
	// javax.security.auth.Policy
	"Policy.getPermissions",
	"Policy.getPolicy",
	"Policy.refresh",
	"Policy.setPolicy"}));

// java.security.Signature.setParameter(String, Object) is deprecated (was replaced with a 1-arg equivalent)
CxList setParameter = methods.FindByMemberAccess("Signature.setParameter");
CxList setParameter_2nd_Params = All.GetParameters(setParameter, 1);
result.Add(setParameter.FindByParameters(setParameter_2nd_Params));

// java.security.SignatureSpi
result.Add(methods.FindByMemberAccess("SignatureSpi.engineGetParameter"));

// java.security.SignatureSpi.engineSetParameter(string, Object) is deprecated
CxList engineSetParameter = methods.FindByMemberAccess("SignatureSpi.engineSetParameter");
CxList engineSetParameter_2nd_Param = All.GetParameters(engineSetParameter, 1); 
result.Add(engineSetParameter.FindByParameters(engineSetParameter_2nd_Param));