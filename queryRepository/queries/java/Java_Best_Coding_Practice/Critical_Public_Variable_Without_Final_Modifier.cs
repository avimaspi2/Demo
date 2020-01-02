CxList classDecl = Find_Class_Decl();
CxList cl = classDecl.InheritsFrom("Applet");
cl.Add(classDecl.InheritsFrom("JApplet"));

CxList fields = Find_Field_Decl();

fields = fields.GetByAncs(cl);
CxList publicFields = fields.FindByFieldAttributes(Checkmarx.Dom.Modifiers.Public);
CxList staticFields =  fields.FindByFieldAttributes(Checkmarx.Dom.Modifiers.Static);

result = publicFields - staticFields;