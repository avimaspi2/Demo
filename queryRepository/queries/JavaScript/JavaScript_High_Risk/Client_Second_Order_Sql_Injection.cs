CxList cookie = Find_Cookie();
CxList storageOut = Find_Storage_Outputs();

CxList outWay = Find_DB_Out();
outWay.Add(cookie);
outWay.Add(storageOut);

CxList dbIn = Find_DB_In();

CxList sanitize = Find_SQL_Sanitize();
//add to sanitizer third param of execute sql.
sanitize.Add(All.GetParameters(dbIn, 2));
sanitize.Add(All.GetParameters(dbIn, 3));
result.Add(outWay.InfluencingOnAndNotSanitized(dbIn, sanitize));

outWay -= result;
//flow solution

CxList unknownRef = Find_UnknownReference();
CxList decl = Find_Declarators();
CxList fieldDecls = Find_FieldDecls();
decl.Add(fieldDecls);

CxList candidates = Find_MemberAccesses();
candidates.Add(unknownRef);
candidates.Add(decl);
CxList potential = candidates.DataInfluencedBy(outWay);
dbIn -= sanitize;
CxList sqlParameter = unknownRef.GetByAncs(All.GetParameters(dbIn, 0));
result.Add(Find_Flows(sqlParameter, potential));
result -= result.InfluencedBy(result);