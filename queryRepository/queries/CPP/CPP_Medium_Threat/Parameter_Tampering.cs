CxList input = Find_Interactive_Inputs();
CxList db = Find_DB();

CxList strings = Find_Strings();
CxList Select = strings.FindByName("*select*", false);
CxList Where = strings.FindByName("*where*", false);
CxList And = strings.FindByName("*And *", false) + 
			 strings.FindByName("* And*", false);

db  = db.DataInfluencedBy(Select).DataInfluencedBy(Where);
db -= db.DataInfluencedBy(And);

result = db.DataInfluencedBy(input);