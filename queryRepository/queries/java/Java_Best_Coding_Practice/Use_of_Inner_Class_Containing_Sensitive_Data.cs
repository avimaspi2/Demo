CxList classes = Find_Class_Decl();
CxList appletClass = classes.InheritsFrom("Applet");
appletClass.Add(classes.InheritsFrom("JApplet"));

CxList innerClass = classes.GetByAncs(appletClass) - appletClass;
CxList staticInnerClass = innerClass.FindByFieldAttributes(Modifiers.Static);

result.Add(innerClass - staticInnerClass);