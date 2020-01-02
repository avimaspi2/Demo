CxList requests = Find_Interactive_Inputs();//Get Requests

CxList php_xsrf_sanitize = Find_XSRF_Sanitize();//Get XSRF sanitizers
requests -= requests.GetByAncs(php_xsrf_sanitize);//Remove request that are ancestors of xsrf sanitizers

CxList strings = Find_Strings();//Get Strings
CxList binary = All.FindByType(typeof(BinaryExpr));//Get Binary expressions


CxList selectCommand = strings.FindByName("select *", StringComparison.OrdinalIgnoreCase);//Find select strings
selectCommand.Add(Find_Methods().FindByName("select *", StringComparison.OrdinalIgnoreCase));//Find select methods

CxList sanitizers = selectCommand;

CxList double_qs = All.FindByShortName("$_DoubleQuotedString", false);//Find All DoubleQuotedStrings
//Find All DoubleQuotedStrings 
double_qs = double_qs.FindByParameters((All.GetParameters(double_qs).FindByType(typeof(StringLiteral)) * selectCommand));
 //Get all select commands that are parameters
CxList select1 = All.FindByFathers(selectCommand.GetAncOfType(typeof(Param)));
//Get all select commands that are assigment expressions
CxList select2 = All.FindByFathers((selectCommand - select1).GetAncOfType(typeof(AssignExpr))); 
//Add binary that contain select commands
binary = selectCommand.GetAncOfType(typeof(BinaryExpr)) * binary;

//Add Sanitizers
sanitizers.Add(binary);
sanitizers.Add(select1);
sanitizers.Add(select2);
sanitizers.Add(double_qs);

CxList db = Find_XSRF_DB_In();
result = db.InfluencedByAndNotSanitized(requests, sanitizers);