CxList DB_IN = Find_Doctrine_DB_In();




CxList whereStmt = DB_IN.FindByShortName("*where*", false);
result.Add(whereStmt);