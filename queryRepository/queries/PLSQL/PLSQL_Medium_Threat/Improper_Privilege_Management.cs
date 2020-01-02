CxList method_call = Find_Methods();

CxList allow_all = All.FindByMemberAccess("DBMS_DISTRIBUTED_TRUST_ADMIN.ALLOW_ALL", false);

CxList grant = method_call.FindByShortName("GRANT", false);
CxList priviliges = All.FindByShortName("ALL", false) + All.FindByShortName("PUBLIC", false);
CxList grant_public_all = priviliges.GetByAncs(grant);


result = allow_all + grant_public_all;