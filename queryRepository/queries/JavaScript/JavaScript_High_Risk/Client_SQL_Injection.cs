CxList inputs = Find_Inputs_NoWindowLocation();
CxList dbIn = Find_DB_In();
CxList Sanitize = Find_SQL_Sanitize();
result.Add(inputs.InfluencingOnAndNotSanitized(dbIn, Sanitize));

//flow solution
CxList unknownRef = Find_UnknownReference();
dbIn -= Sanitize;
CxList sqlParameter = unknownRef.GetByAncs(All.GetParameters(dbIn, 0));
CxList potentialVul = All.FindByShortName(sqlParameter).DataInfluencedBy(inputs - result);
result.Add(Find_Flows(sqlParameter, potentialVul));
result -= result.DataInfluencedBy(result);