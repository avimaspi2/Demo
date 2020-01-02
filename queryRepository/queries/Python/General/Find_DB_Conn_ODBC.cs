string[] dbConnectionList = new string[] {
	"pyodbc.connect",
	"pypyodbc.connect",
	"pypyodbc.win_connect_mdb",
	"odbtpapi.connection",
	"odbtp.connect",
	"ceODBC.Connection",
	"ceODBC.connect"
	//	"Windows.DriverConnect",
	//	"iODBC.DriverConnect",
	//	"unixODBC.DriverConnect"
	};

foreach (string s in dbConnectionList)
{
	result.Add(All.FindAllReferences(All.InfluencedBy(All.FindByMemberAccess(s))
		.FindByAssignmentSide(CxList.AssignmentSide.Left)));
}

//mxODBC
result.Add(All.FindAllReferences(All.InfluencedBy(All.FindByRegex("( mx.ODBC.[^.]*. | Windows | iODBC | unixODBC | SybaseASA )?DriverConnect"))
	.FindByAssignmentSide(CxList.AssignmentSide.Left)));