CxList allDataInfluencedByConn = Find_DB_Conn_Sybase();
CxList target = allDataInfluencedByConn.GetMembersOfTarget();

//Explicit fetch functions 
result.Add(target.FindByShortName("fetch*"));

//Implicit fecth
result.Add(allDataInfluencedByConn.GetFathers().FindByType(typeof(ForEachStmt)));

//Explicit bulkcopy with 'out=1' as argument - CS_BLK_OUT
result.Add(target.FindByShortName("bulkcopy").FindByParameterValue(1, "ArrayInitializer", BinaryOperator.IdentityEquality));