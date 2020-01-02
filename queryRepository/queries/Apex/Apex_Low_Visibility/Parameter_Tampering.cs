CxList input = Find_Interactive_Inputs();
CxList db = Find_DB_Output();
CxList methods = Find_Methods();

CxList sObjDB = methods.FindByShortName("insert") +
	methods.FindByShortName("update") +
	methods.FindByShortName("delete") +
	methods.FindByShortName("merge") +
	methods.FindByShortName("upsert") +
	methods.FindByShortName("undelete") +
	methods.FindByShortName("convertlead");

db -= sObjDB;

CxList strings = Find_Strings();
CxList Select = strings.FindByName("*select*", false);
CxList Where = strings.FindByName("*where*", false);
CxList And = strings.FindByName("*and *", false) + 
	strings.FindByName("* and*", false);

CxList db1 = db.DataInfluencedBy(Select).DataInfluencedBy(Where);
db1 -= db1.DataInfluencedBy(And);
db = db1 + (Select * Where - And).GetByAncs(All.GetParameters(db));

result = db.InfluencedByAndNotSanitized(input, Find_Test_Code());

result -= Find_Test_Code();

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);