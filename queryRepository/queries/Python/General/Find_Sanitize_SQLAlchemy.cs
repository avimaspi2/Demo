//SQLAlchemy SQL Expression Language Reference
//http://docs.sqlalchemy.org/en/rel_0_9/
//Sanitizing methods
//// .execute(*)
//// .values(...)
//// .params {...}
//// bindparam(...)
//// xxx.c.yyy OP zzz
//// .like(...)
//* the number of arguments vary. 
//If the number of arguments is 1, then SQL Injection may be exploited

CxList methods = Find_DB_In_SQLAlchemy();
methods.Add(Find_DB_Out_SQLAlchemy());

CxList executeMthds = methods.FindByName("*.execute");
CxList allParams = Find_Param();

CxList otherMethods = methods.FindByName("*.values");
otherMethods.Add(methods.FindByName("*.params")); 
otherMethods.Add(methods.FindByName("bindparam"));
otherMethods.Add(methods.FindByName(".like"));

//Find binary expressions where one of the sides use member access 'c'
//Using binary expressions this way makes user defined input on the other side
//of the expression to be automatically set as a bound parameter.
CxList cAccess = All.FindByShortName("c").FindByType(typeof(MemberAccess));
CxList cExpressions = cAccess.GetAncOfType(typeof(BinaryExpr));

CxList sanitizers = All.NewCxList();
sanitizers.Add(otherMethods);
sanitizers.Add(cExpressions);

//only the second parameter of query is sanitized
result.Add(sanitizers);
result.Add(allParams.GetParameters(executeMthds, 1));