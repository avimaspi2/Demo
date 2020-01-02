CxList methods = Find_Methods();

// dbi
CxList dbi = methods.FindByMemberAccess("DBI", "connect") + methods.FindByMemberAccess("DBI", "connect_cached");

// oracle
CxList ora = methods.FindByShortName("ora_login");

// mysql
CxList mysql = methods.FindByMemberAccess("Mysql", "connect");

CxList inputs = Find_Interactive_Inputs();
CxList sanitize = Find_Sanitize() + Find_Integers();

result = (dbi + ora + mysql).InfluencedByAndNotSanitized(inputs, sanitize);