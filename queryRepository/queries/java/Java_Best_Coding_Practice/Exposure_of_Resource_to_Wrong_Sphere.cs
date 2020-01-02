CxList allFields = Find_Field_Decl();
//Public fields
CxList allPublicFields = allFields.FindByFieldAttributes(Modifiers.Public);
//Constant fields
CxList allConstFields = allPublicFields.FindByFieldAttributes(Modifiers.Sealed);
//Static fields 
CxList allStaticFields = allPublicFields.FindByFieldAttributes(Modifiers.Static);

//remove all constant and public static fields
CxList toRemove = All.NewCxList();
toRemove.Add(allConstFields);
toRemove.Add(allStaticFields);
result = allPublicFields - toRemove;