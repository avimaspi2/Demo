CxList methods = Find_Methods();

result.Add(methods.FindByMemberAccesses(new string[]{
	// java.sql.DriverManager
	"DriverManager.getLogStream",
	"DriverManager.setLogStream",
	// java.sql.PreparedStatement
	"PreparedStatement.setUnicodeStream",
	// java.sql.ResultSet
	"ResultSet.getUnicodeStream"}));

// java.sql.CallableStatement.getBigDecimal(int, int) is deprecated
CxList getBigDecimal = methods.FindByMemberAccess("CallableStatement.getBigDecimal");
CxList getBigDecimal_2nd_Params = All.GetParameters(getBigDecimal, 1);
result.Add(getBigDecimal.FindByParameters(getBigDecimal_2nd_Params));

// java.sql.ResultSet.getBigDecimal(int,int) and (string, int) are deprecated
CxList RS_getBigDecimal = methods.FindByMemberAccess("ResultSet.getBigDecimal");
CxList RS_getBigDecimal_2nd_Param = All.GetParameters(RS_getBigDecimal, 1); 
result.Add(RS_getBigDecimal.FindByParameters(RS_getBigDecimal_2nd_Param));