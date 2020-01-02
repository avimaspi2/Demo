CxList input = Find_Potential_Inputs();
CxList db = Find_DB_In();
CxList strings = Find_Strings();

CxList Select = strings.FindByName("*select*", false);
CxList Where = strings.FindByName("*where*", false);
CxList And = strings.FindByName("*And *", false); 
And.Add(strings.FindByName("* And*", false));

db  = db.DataInfluencedBy(Select).DataInfluencedBy(Where);
db -= db.DataInfluencedBy(And);

result = db.DataInfluencedBy(input);