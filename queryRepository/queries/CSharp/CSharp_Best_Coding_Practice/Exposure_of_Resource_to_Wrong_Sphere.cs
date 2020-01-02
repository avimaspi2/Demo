CxList allFields = All.FindByType(typeof(FieldDecl));
CxList allPublicFields = allFields.FindByFieldAttributes(Modifiers.Public);
CxList allConstFields = allFields.FindByFieldAttributes(Modifiers.Readonly);
//CxList allProtectedFields = allFields.FindByFieldAttributes(Modifiers.Protected);
result = //allProtectedFields +
	allPublicFields - allConstFields;