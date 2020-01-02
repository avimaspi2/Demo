//use of hardcoded secret session token inside secret_token.rb or session_store.rb
CxList secret_tokenFile = All.FindByFileName("*secret_token.rb");
CxList secret = secret_tokenFile.FindByMemberAccess("config.secret_token");
result=secret.DataInfluencedBy(secret_tokenFile.FindByType(typeof(StringLiteral)));

CxList sessionStore = All.FindByFileName("*session_store.rb");
CxList baseSession = sessionStore.FindByMemberAccess("Base.session").GetFathers();
secret = sessionStore.FindByShortName("secret").GetByAncs(baseSession);
result.Add(secret.DataInfluencedBy(sessionStore.FindByType(typeof(StringLiteral))));


CxList acSession = All.FindByMemberAccess("action_controller.session").GetFathers();
secret = All.FindByShortName("secret").GetByAncs(acSession);
result.Add(secret.DataInfluencedBy(All.FindByType(typeof(StringLiteral))));