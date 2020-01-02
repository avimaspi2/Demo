CxList fields = Find_Field_Decl(); // i.e. not final (otherwise it would be ConstantDecl)
// Find thepublic static fields. No need to check for not-final, since "final-fields" are actually ConstantDscl.
CxList staticFields = fields.FindByFieldAttributes(Modifiers.Static);
CxList publicStaticFields = staticFields.FindByFieldAttributes(Modifiers.Public);

result = publicStaticFields;