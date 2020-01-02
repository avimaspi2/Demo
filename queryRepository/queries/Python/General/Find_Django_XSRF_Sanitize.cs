/*
	This Query Finds the Sanitizers for Django 
	which are protected by the CsrfMiddleWare
	It finds the Middleware by decorators, and
	by application Dependency Injection
	It uses the Settings > Routings > Controllers 
	Flow To search for sanitized XSRF
*/
if (Find_Django().Count != 0)
{
	CxList strings = Find_Strings();

	CxList DomainSafetyOrigin = strings.FindByShortName("*django.middleware.csrf.CsrfViewMiddleware*");

	// Detect Django CsrfMiddleWare
	CxList DomainSafety = DomainSafetyOrigin.GetAncOfType(typeof(Declarator))
		.FindByShortName("MIDDLEWARE_CLASSES");

	DomainSafety.Add(All.FindByShortName("MIDDLEWARE_CLASSES")
		.GetByAncs(DomainSafetyOrigin.GetAncOfType(typeof(AssignExpr))));

	// Find Affected Applications
	CxList ProtectedFiles = All.NewCxList();
	foreach(CxList Middleware in DomainSafety){
		CSharpGraph midWare = Middleware.GetFirstGraph();
	
		CxList urlConf = All.FindByFileName(midWare.LinePragma.FileName)
			.FindByShortName("ROOT_URLCONF").ReduceFlowByPragma();
	
		CxList auxUrlConf = All.NewCxList();
		foreach(CxList Lurl in urlConf){
			CSharpGraph url = Lurl.GetFirstGraph();
		
			if(url is UnknownReference){
				auxUrlConf.Add(Lurl
					.FindByAssignmentSide(CxList.AssignmentSide.Left)
					.GetAncOfType(typeof(AssignExpr))
					);
			}
			if(url is Declarator || url is FieldDecl){
				auxUrlConf.Add(Lurl);
			}
		}
	
		ProtectedFiles.Add(strings.GetByAncs(auxUrlConf));
	}

	List<string> pfiles = new List<string>();
	foreach (CxList file in ProtectedFiles){
		CSharpGraph fle = file.GetFirstGraph();
		string filename = fle.Text.Replace("'", "").Replace("\"", "").Replace(".", "\\");
		if (!pfiles.Contains(filename)) pfiles.Add(filename);
	}

	// Find Django Routings
	CxList ProtectedURL = All.NewCxList();
	foreach (string file in pfiles){
		string ExtendedName = "*" + file + ".py";
		ProtectedURL.Add(All.FindByFileName(ExtendedName));
	}

	CxList URLInvokes = ProtectedURL.FindByType(typeof(MethodInvokeExpr)).FindByShortName("url");

	CxList ParamList = All.GetParameters(URLInvokes, 1).FindByType(typeof(StringLiteral));

	foreach(CxList end in ParamList){
		StringLiteral endFileName = end.TryGetCSharpGraph<StringLiteral>();
	
		string endFile = endFileName.Text;
		endFile = endFile.Substring(1, endFile.Length - 2);
		int methodPosition = endFile.LastIndexOf(".");
		if (methodPosition < 0)
		{
			continue;
		}
		string methodName = "*" + endFile.Substring(methodPosition + 1);
		string nendFile = endFile.Substring(0, methodPosition).Replace(".", "\\");
	
		string file = "*" + nendFile + ".py";
	
		// Add all elements from the methods protected
		CxList safeMethods = Find_MethodDecls()
			.FindByShortName(methodName)
			.FindByFileName(file);
	
		result.Add(All.GetByAncs(safeMethods));
	}

	CxList decorator = Find_Methods().FindByShortName("csrf_protect");
	CxList decorated = All.GetParameters(decorator, 0).FindByType(typeof(UnknownReference));		
	CxList DecoratedMethodElems = All.GetByAncs(All.FindAllReferences(decorated).FindByType(typeof(MethodDecl)));
		
	result.Add(DecoratedMethodElems);

	result = result.ReduceFlowByPragma();
}
else
{
	result = All.NewCxList();
}