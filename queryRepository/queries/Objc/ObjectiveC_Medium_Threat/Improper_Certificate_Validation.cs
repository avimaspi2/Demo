// Improper_Certificate_Validation
// //////////////////////-
// The purpose of the query is as to find applications that allow the following:
//		1. When connecting with SSL/TLS, do not validate certificates (forged, self-signed).
//		2. Disable CA chain validation
// 

CxList methods = Find_Methods();
CxList methodDecls = Find_MethodDecls();

CxList memberAccess = Find_MemberAccesses();

List<string> SSLConnectionMethods = new List<string>() {
		"connection:didReceiveAuthenticationChallenge:",
		"connection:willSendRequestForAuthenticationChallenge:",
		"URLSession:didReceiveChallenge:completionHandler:",
		"URLSession:task:didReceiveChallenge:completionHandler:",		
		"URLSession:didReceive:completionHandler:",
		// swift
		"URLSession:task:didReceive:completionHandler:",
		"useCredential:forAuthenticationChallenge:"	
		};

CxList SSLConnection = methodDecls.FindByShortNames(SSLConnectionMethods);

CxList indicators0 = methods.FindByShortName("useCredential:forAuthenticationChallenge:");
// swift
List<string> allMethods = new List<string>() {
		"use:for:",
		"useCredential:for:"	
		};
indicators0.Add(All.FindByMemberAccess("URLAuthenticationChallenge.sender").GetMembersOfTarget().FindByShortNames(allMethods));

CxList indicators1 = methods.FindByShortName("completionHandler");

CxList credentials0 = indicators0.GetByAncs(SSLConnection);
CxList credentials1 = indicators1.GetByAncs(SSLConnection);

CxList credentialForTrust = All.FindByMemberAccess("NSURLCredential.credentialForTrust:");
// swift
credentialForTrust.Add(methods.FindByShortName("NSURLCredential:"));
credentialForTrust.Add(methods.FindByShortName("URLCredential:"));

CxList credentialsParam = All.GetParameters(credentials0, 0);
credentialsParam.Add(All.GetParameters(credentials1, 1));

CxList allCredentials = 
	credentialsParam.DataInfluencedBy(credentialForTrust) + 
	credentialsParam * credentialForTrust;

CxList serverTrust = All.FindByName("*challenge.protectionSpace.serverTrust");
CxList conditions = Find_Condition();
CxList secTrustEvaluate = methods.FindByShortName("SecTrustEvaluate");

foreach (CxList cred in allCredentials.GetCxListByPath())
{
	CxList method = cred.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly)
		.GetAncOfType(typeof(MethodDecl));
	CxList conditionsInMethod = conditions.GetByAncs(method);
	CxList serverTrustInMethod = serverTrust.GetByAncs(conditionsInMethod);
	CxList localSecTrustEvaluate = secTrustEvaluate.GetByAncs(method);
	if (serverTrustInMethod.Count == 0 && localSecTrustEvaluate.Count == 0)
	{
		result.Add(cred);
	}
}

// Any override of allowsAnyHTTPSCertificateForHost:
result.Add(methodDecls.FindByShortName("allowsAnyHTTPSCertificateForHost:"));

// AFSecurityPolicy vulnerabilities
CxList securityViolations = All.NewCxList();
CxList allowInvalidCertificates = memberAccess.FindByName("*.allowInvalidCertificates");
CxList positiveallowInvalidCertificates = All.DataInfluencingOn(allowInvalidCertificates).FindByName("true");
securityViolations.Add(positiveallowInvalidCertificates.DataInfluencingOn(allowInvalidCertificates));
CxList setAllowInvalidCertificates = methods.FindByShortName("setAllowInvalidCertificates:");
setAllowInvalidCertificates = All.GetParameters(setAllowInvalidCertificates, 0).FindByName("true").DataInfluencingOn(setAllowInvalidCertificates);
securityViolations.Add(setAllowInvalidCertificates);

CxList validatesCertificateChain = memberAccess.FindByName("*.validatesCertificateChain");
CxList negativeValidatesCertificateChain = All.DataInfluencingOn(validatesCertificateChain).FindByName("false");
securityViolations.Add(negativeValidatesCertificateChain.DataInfluencingOn(validatesCertificateChain));
CxList setValidatesCertificateChain = methods.FindByShortName("setValidatesCertificateChain:");
setValidatesCertificateChain = All.GetParameters(setValidatesCertificateChain, 0).FindByName("false").DataInfluencingOn(setValidatesCertificateChain);
securityViolations.Add(setValidatesCertificateChain);

CxList validatesDomainName = memberAccess.FindByName("*.validatesDomainName");
CxList negativeValidatesDomainName = All.DataInfluencingOn(validatesDomainName).FindByName("false");
securityViolations.Add(negativeValidatesDomainName.DataInfluencingOn(validatesDomainName));
CxList setValidatesDomainName = methods.FindByShortName("setValidatesDomainName:");
setValidatesDomainName = All.GetParameters(setValidatesDomainName, 0).FindByName("false").DataInfluencingOn(setValidatesDomainName);
securityViolations.Add(setValidatesDomainName);

result.Add(securityViolations);