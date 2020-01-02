CxList session = All.FindByShortName("session");
CxList assignSession = session.GetAncOfType(typeof(AssignExpr));

CxList expiration = All.FindByShortName("expire_after").FindByType(typeof(UnknownReference));
expiration = expiration.GetByAncs(assignSession);
CxList assignExpiration = expiration.GetAncOfType(typeof(AssignExpr));
assignExpiration = assignExpiration.GetFathers().GetAncOfType(typeof(AssignExpr));

CxList secret = All.FindByShortName("secret").FindByType(typeof(UnknownReference));
secret = secret.GetByAncs(assignSession);
CxList assignSecret = secret.GetAncOfType(typeof(AssignExpr));
assignSecret = assignSecret.GetFathers().GetAncOfType(typeof(AssignExpr));

result = session.GetByAncs(assignSecret - assignExpiration);