CxList allFields = Find_FieldDecls();
CxList allPublicFields = allFields.FindByFieldAttributes(Modifiers.Public) - allFields.FindByFieldAttributes(Modifiers.Friend);
CxList allConstFields = allFields.FindByFieldAttributes(Modifiers.Readonly);
result = allPublicFields - allConstFields;