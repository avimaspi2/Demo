/**
Find all the DB Outs related to the Sequelize ORM
**/ 

CxList callback = All.NewCxList();
CxList lambdaExprs = Find_LambdaExpr();

//list of promisses methods
List<string> promissesMethods = new List<string>{"then", "success", "spread", "catch"};

//get all the dbInInvokes of Sequelize ORM
CxList dbInInvokes = NodeJS_Find_Sequelize_DB_In();

//All the targets of the DbInvokes
CxList targetsDbInInvokes = dbInInvokes.GetMembersOfTarget();
targetsDbInInvokes.Add(targetsDbInInvokes.GetRightmostMember());

//get all the promisses functions
CxList promisses = targetsDbInInvokes.FindByShortNames(promissesMethods);

CxList promissesCallBackFunction = lambdaExprs.GetParameters(promisses);

callback.Add(promissesCallBackFunction);

result = All.GetParameters(callback);

result.Add(NodeJS_Find_Sequelize_DB_InOut());