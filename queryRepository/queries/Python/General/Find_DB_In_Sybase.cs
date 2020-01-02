CxList allDataInfluencedByConn = Find_DB_Conn_Sybase();
CxList target = allDataInfluencedByConn.GetMembersOfTarget();

//Explicit execute functions
result.Add(target.FindByShortName("execute*"));

//Explicit bulkcopy with one argument - CS_BLK_IN is default
CxList bulkcopy = target.FindByShortName("bulkcopy");
CxList cs_bulk_out = bulkcopy.FindByParameterValue(1, "ArrayInitializer", BinaryOperator.IdentityEquality);
result.Add(bulkcopy - cs_bulk_out);