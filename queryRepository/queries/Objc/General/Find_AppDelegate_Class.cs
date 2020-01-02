CxList classes = Find_Classes();
CxList interfaces = Find_InterfaceDecl();

CxList classesDelegates = classes.InheritsFrom("UIApplicationDelegate");
CxList interfacesDelegates = interfaces.InheritsFrom("UIApplicationDelegate");

foreach (CxList item in interfacesDelegates)
{
	string interfacename = item.GetName();
	InterfaceDecl cs = item.TryGetCSharpGraph<InterfaceDecl>();
	if (cs != null)
	{
		LinePragma lp = cs.LinePragma;
		string fileName = lp.FileName;
		if (fileName.EndsWith(".h"))
		{
			fileName = fileName.Substring(0, fileName.Length - 1) + "m";
		}
		string className = interfacename.Substring(1);
		result.Add(classes.FindByFileName(fileName).FindByShortName(className));
	}
}

result.Add(classesDelegates);