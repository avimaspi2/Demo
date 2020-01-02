/*This query will look for all storage/db out for XS, XSDS and for SecureStore*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	result = XSDS_Find_DB_Out();
	result.Add(XS_Find_DB_Out());
	result.Add(XS_Find_Secure_Store_Read());
}