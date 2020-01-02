CxList allFields = Find_Field_Decl();
CxList allPublicFields = allFields.FindByFieldAttributes(Modifiers.Public);
//CxList allProtectedFields = allFields.FindByFieldAttributes(Modifiers.Protected);
CxList allConstFields = allFields.FindByFieldAttributes(Modifiers.Sealed);

result = //allProtectedFields + 
	allPublicFields - allConstFields;