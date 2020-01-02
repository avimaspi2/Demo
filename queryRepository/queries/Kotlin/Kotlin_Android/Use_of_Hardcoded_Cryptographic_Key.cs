CxList objectCreate = Find_ObjectCreations();
CxList secretKey = objectCreate.FindByShortName("SecretKeySpec");
CxList stringLiteral = Find_String_Literal();
CxList possibleParam = All.GetParameters(secretKey, 0);
result = possibleParam.DataInfluencedBy(stringLiteral);