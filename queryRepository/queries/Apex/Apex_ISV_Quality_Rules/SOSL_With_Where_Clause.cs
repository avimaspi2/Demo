//WHERE clause must not exist in SOSL
//We heuristically find SOSL statements that have the "WHERE" keyword following a "RETURNING" keyword.
CxList slStatements = All.FindByMemberAccess("Cx_VirtualDal.select");
CxList whereSOSL = (slStatements + Find_Strings()).FindByRegex(@"RETURNING\s[^;]*\WWHERE\s", false, true, false);
CxList searchQuery = All.FindByMemberAccess("search.query");

result = searchQuery.DataInfluencedBy(whereSOSL) + (slStatements * whereSOSL);