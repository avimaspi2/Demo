CxList psw = All.FindByType(typeof(StringLiteral));

CxList openConnection = Find_Connection_DB();

result = openConnection.DataInfluencedBy(psw);