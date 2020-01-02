/*
	This query tries to find any definition of the Content-Security-Policy header, either in configuration 
	files or set directly by code. If none is found, a result is added in the first web output (any query 
	can provide instead a better fix place).
*/

CxList cspInConfig = general.Find_CSP_Configuration_In_ConfigFiles();

if (cspInConfig.Count == 0) {
	
	CxList cspInCode = general.Find_CSP_Configuration_In_Code();

	if (cspInCode.Count == 0)
	{
		result = general.Get_Missing_CSP_Best_Fix_Location();
	}
}