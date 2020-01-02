//SQLObject (+ SQLBuilder)
//http://www.sqlobject.org/SQLObject.html
//sanitizing methods
//// xxx.q.yyy bin_op zzz
//// .sqlrepr()

CxList methods = Find_DB_In_SqlObject();
methods.Add(Find_DB_Out_SqlObject());

CxList sqlreprMethods = methods.FindByName("*.sqlrepr");

//Find binary expressions where one of the sides use memeber access 'q'
//Using binary expressions this automatically escapes .
CxList qAccess = All.FindByShortName("q").FindByType(typeof(MemberAccess));
CxList qExpressions = qAccess.GetAncOfType(typeof(BinaryExpr));

result = sqlreprMethods;
result.Add(qExpressions);