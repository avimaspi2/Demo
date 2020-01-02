CxList Inputs = Find_Read()+Find_DB();
CxList Log = Find_Methods().FindByShortName("syslog");
Log.Add(Find_Write().FindByName("*.clog.*"));

CxList sanitize = Find_Sanitize();

result = Log.InfluencedByAndNotSanitized(Inputs, sanitize);