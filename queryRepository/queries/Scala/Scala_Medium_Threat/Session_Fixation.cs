CxList methods = Find_Methods();
CxList sessions = methods.FindByMemberAccess("HttpServletRequest.getSession");
CxList requests = Find_UnknownReference().FindByType("HttpServletRequest");
CxList authentication = methods.FindByMemberAccess("HttpSession.setAttribute");
CxList invalidateInvocations = methods.FindByShortName("invalidate");

//Sessions with one boolean parameter valued "true". These are safe because the flag forces the session id to be renewed.
CxList safeSessions = sessions.FindByParameters(All.FindByType(typeof(BooleanLiteral)).FindByShortName("true"));

CxList invalidatedSessionRequest = requests.DataInfluencingOn(invalidateInvocations);
//If previous sessions are invalidated, the new one's id will be renewed and therefore, safe.
safeSessions.Add(sessions.DataInfluencedBy(invalidatedSessionRequest));

CxList possiblyUnsafeSessions = sessions - safeSessions;

//Find sessions that are authenticated (associated with some data using setAttribute) and are not reassigned.
result = possiblyUnsafeSessions.DataInfluencingOn(authentication);