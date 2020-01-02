CxList On_Error = Find_Methods().FindByShortName("On_Error", false);
CxList gotoStmt = Find_Strings().FindByShortName("*GoTo 0*", false);
result = gotoStmt.GetByAncs(On_Error);