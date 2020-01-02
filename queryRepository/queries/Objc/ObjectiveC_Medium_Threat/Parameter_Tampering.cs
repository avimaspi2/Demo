/////////////////////////////////////////////////////////////////
// 				Parameter_Tampering
// This query checks that each input from the user,
// that is used to get information from the database,
// is sanitized by:
//	1.In the "Select","Insert" and "Update" statements there is also a "And" part in the "Where".
//	2.The input is being checked by an if condition somewhere in the program.
/////////////////////////////////////////////////////////////////

CxList input = Find_Interactive_Inputs();
CxList db = Find_DB_For_Parameter_Tampering();
CxList sanitize = Find_Parameter_Tampering_Sanitize();

result = db.InfluencedByAndNotSanitized(input, sanitize);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);