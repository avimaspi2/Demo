CxList linq = Find_DB_Linq_Full();

CxList linqSanitize = All.GetByAncs(All.GetParameters(linq));


CxList linqBinary = linqSanitize.FindByType(typeof(BinaryExpr));


CxList booleanConditions = linqBinary.FindByShortNames(new List<string> {
		"<", ">",
		"==", "!=", "<>",
		"<=", ">=",
		"||", "&&"});
booleanConditions.Add(linqSanitize.FindByType(typeof(UnaryExpr)).FindByShortName("Not"));
CxList assign = linqSanitize.FindByType(typeof(AssignExpr));


CxList ampercent = linqSanitize.FindByShortName("*@*");
ampercent = linqSanitize.GetByAncs(linq.FindByParameters(ampercent));

CxList linqMethodInvokes = linqSanitize.FindByType(typeof(MethodInvokeExpr));

linqSanitize = linqSanitize.GetByAncs(assign + booleanConditions);
linqSanitize.Add(linqMethodInvokes);

CxList sqlParameter = All.FindByType(typeof(ObjectCreateExpr))
	.FindByShortName("SqlParameter", true);


CxList notInjectable = linq.FindByShortNames(new List<string> {
		"DeleteOnSubmit",
		"InsertOnSubmit",
		"UpdateOnSubmit",
		"OnSubmit",
		"SubmitChanges"});

CxList guid = All.FindByType(typeof(ObjectCreateExpr)).FindByShortName("Guid", true);
 
// Get the EF methods sanitize
CxList ef = Find_DB_EF_Sanitize();

CxList methods = Find_Methods();

CxList temp = methods.FindByMemberAccess("SqlMapper.*");
CxList ibatis = temp.FindByMemberAccess("SqlMapper.queryForObject"); 
ibatis.Add(temp.FindByMemberAccess("SqlMapper.queryForList"));
ibatis.Add(temp.FindByMemberAccess("SqlMapper.queryWithRowHandler"));
ibatis.Add(temp.FindByMemberAccess("SqlMapper.queryForPaginatedList"));
ibatis.Add(temp.FindByMemberAccess("SqlMapper.queryForMap"));
ibatis.Add(temp.FindByMemberAccess("SqlMapper.insert")); 
ibatis.Add(temp.FindByMemberAccess("SqlMapper.update")); 
ibatis.Add(temp.FindByMemberAccess("SqlMapper.delete"));

CxList qSQL1 = methods.FindByMemberAccess("QSqlQuery.bindValue");
CxList qSQL2 = methods.FindByMemberAccess("QSqlQuery.addBindValue");

result.Add(All.GetParameters(qSQL1, 1));
result.Add(All.GetParameters(qSQL2, 0));
result.Add(ibatis);
result.Add(Find_Sanitize());
result.Add(linqSanitize);
result.Add(notInjectable);
result.Add(ampercent);
result.Add(ef);
result.Add(guid);
result.Add(sqlParameter);
result.Add(Find_Linq_to_SQL_Sanitizers());

//add OleDbDataAdapter constructor's second parameter to sanitizers
CxList oce = All.FindByType(typeof(ObjectCreateExpr)).FindByType("OleDbDataAdapter");
CxList secondSanitizer = All.GetParameters(oce, 1);
result.Add(secondSanitizer);

CxList POfOceVul = All.GetParameters(oce, 0).DataInfluencedBy(Find_Interactive_Inputs());
CxList oceVul = oce.FindByParameters(POfOceVul);
result.Add(oce - oceVul);

//Add  Parameters.AddWithValue method
CxList createCommand = All.FindByMemberAccess("connection.CreateCommand");
CxList sqlCommand = createCommand.GetAssignee();
sqlCommand.Add(All.FindAllReferences(sqlCommand));
sqlCommand.Add(All.FindByType("SqlCommand"));
sqlCommand.Add(All.FindByType("SqlCeCommand"));

CxList SqlCeCommandParameters = sqlCommand.GetMembersOfTarget().FindByShortName("Parameters"); 
result.Add(SqlCeCommandParameters.GetMembersOfTarget().FindByShortNames(new List<string> {"AddWithValue", "Add", "AddRange"}));


//Add SqlDataAdapter.Update
CxList SqlDataAdapterParameters = All.FindByMemberAccess("SqlDataAdapter.Update");
result.Add(All.GetParameters(SqlDataAdapterParameters));


result.Add(Find_Encode() - Find_HTML_Encode());

//Add connectionstring related objects
result.Add(Find_Connection_String());