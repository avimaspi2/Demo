//Find DB in for parameter tampering.
//Find all the DB in that is influenced by Select/Update/Delete sql statement that had a "Where" part 
//and doesn't include an "And" inside the "Where".

CxList db = Find_DB_In();
CxList strings = Find_Strings();

CxList dbStmt = strings.FindByName("*Delete*", false);
dbStmt.Add( strings.FindByName("*Update*", false));
dbStmt.Add( strings.FindByName("*Select*", false));
CxList whereStmt = strings.FindByName("*where*", false);
CxList andStmt = strings.FindByName("*And *", false); 
andStmt.Add(strings.FindByName("* And*", false));

db = db.DataInfluencedBy(dbStmt).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
db = db.DataInfluencedBy(whereStmt).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
db -= db.DataInfluencedBy(andStmt);
result = db;