CxList emptyString = Find_Empty_Strings();
CxList NULL = All.FindByName("null");
CxList keys = All.FindByShortName("*ENC*", false).FindByShortName("*KEY*", false);

CxList key_in_lSide = keys.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList strLiterals = All.FindByType(typeof(PrimitiveExpr)) - emptyString - NULL;
CxList lit_in_rSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);

result = key_in_lSide.FindByFathers(key_in_lSide.GetFathers() * lit_in_rSide.GetFathers());
CxList key_in_Field = All.GetByAncs(All.FindByType(typeof(FieldDecl)) + All.FindByType(typeof(ConstantDecl)))*keys;
result.Add(key_in_Field.DataInfluencedBy(strLiterals));

//netcore
//System.Security.Cryptography.AesCcm
//System.Security.Cryptography.AesGcm
CxList objAesGcmCcm = Find_ObjectCreations().FindByTypes(new String[]{"AesGcm","AesCcm"});
CxList paramObjKey = All.GetParameters(objAesGcmCcm);
CxList stringsToTest = Find_Strings();
result.Add( paramObjKey.DataInfluencedBy(stringsToTest) );