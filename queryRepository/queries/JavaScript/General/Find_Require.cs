/// <summary>
///  This query searches for require statements and return the references of relevant objects which refer to the module required:
///	 param[0] (string) is the name of the module
///  param[1] (int) is the depth to search for. For example:
///			depth = 1: result = 'require("module")' in 'require("module").member...' and 'object' in 'var object = 'require("module")'
///			depth = 2: result = 'require("module")' in 'require("module").member...' and
///				 				references of 'object' in 'var object = require("module")' and 'object' in 'object.member...'
///								references of 'other' in  'var other = object()' etc... 
///	 param[2] (list of strings) hashing algorithms to remove
/// </summary>
///

int? depth = 1;
List<string> excludeList = null;

if(param.Length > 0)
{
	//Given a "*" pattern based in parameter "s1", it will check in "s2" if "s1" fits "s2"
	//Ex:
	// "a*" in "abc" => true
	// "b*" in "abc" => false
	// "*b*" in "abc" => true
	// "*c" in "abc" => true
	// "abc" in "abc" => true
	Func<string, string, bool> Comparer = (s1, s2) =>
		{
		s1 = s1.ToLower();
		s2 = s2.ToLower();
		string ex = s1.Trim('*');
		return (s1.StartsWith("*") && s2.EndsWith(ex)) ||
			(s1.EndsWith("*") && s2.StartsWith(ex)) ||
			(s1.StartsWith("*") && s1.EndsWith("*") && s2.Contains(ex)) ||
			(!s1.Contains("*") && s2.Equals(ex)) ||
			((s2.StartsWith("./") || s2.StartsWith("../")) && s2.Contains(ex));
		};
	
	try
	{
		CxList assignLeft = Find_Assign_Lefts();
		CxList methods = Find_Methods();
		CxList references = Find_UnknownReference();
		CxList declarators = Find_Declarators();
		CxList methodDecls = Find_MethodDecls();
		CxList memberAccess = Find_MemberAccesses();
		CxList objects = Find_ObjectCreations();
		CxList imports = Find_Import();
		
		string moduleName = param[0] as string;
		if (param.Length > 1)
		{
			depth = param[1] as int?;	// get param[1] as nullable integer
			if (depth == null || depth <= 0 )
			{
				depth = 1;
			}
			if (depth >= 10)
			{
				depth = 10;
			}
			
			if(param.Length == 3) {
				excludeList = param[2] as List<string>; // get param[2] as List
			}
		}
		
		references.Add(methods);
		references.Add(declarators);
		references.Add(methodDecls);
		references.Add(memberAccess);
		
		
		CxList assign = All.NewCxList();
		foreach (CxList imp in imports)
		{
			Import import = imp.TryGetCSharpGraph<Import>();
			
			bool toProcess = true; 
			// Remove the imports that are on the exclude list
			if (excludeList != null && excludeList.Count > 0)
			{
				foreach (string exclude in excludeList)
				{
					if (Comparer(exclude, import.ImportedFilename))
					{
						toProcess = false;
						break;
					}
				}
			}
			
			if(toProcess && Comparer(moduleName, import.ImportedFilename))
			{
				result.Add(imp);
				CxList importAssign = All.NewCxList();
				
				if (import.Symbols != null && import.Symbols.Count > 0)
				{	
					string symbol = import.Symbols[0];
					if (import.NamespaceAlias != null) 
					{
						importAssign.Add(references.FindByName("*" + import.NamespaceAlias + "." + symbol));
					}
					if (import.Namespace != null) 
					{
						importAssign.Add(references.FindByName("*" + import.Namespace + "." + symbol));
					}
				}
				
				if (import.NamespaceAlias != null) 
				{
					importAssign.Add(references.FindByShortName(import.NamespaceAlias));
				}
		
				if (import.Namespace != null) 
				{
					importAssign.Add(references.FindByShortName(import.Namespace));
				}
			
				CxList refs = importAssign.GetAssignee();
				if (import.NamespaceAlias != null) 
				{
					refs.Add(objects.FindByType(import.NamespaceAlias + ".*").GetAssignee());
				}
				assign.Add(refs);
				result.Add(references.FindAllReferences(refs));
			}		
		}
		
		CxList moduleRef = All.NewCxList();
		CxList moduleAssigned = All.NewCxList();
		for (int i = 0; i < depth && assign.Count > 0; i++)
		{
			moduleRef = references.FindAllReferences(assign);
			result.Add((moduleRef - assign) - assignLeft);
			moduleAssigned = All.FindByFathers(moduleRef.GetAncOfType(typeof(AssignExpr))).GetAssigner(); 
			assign = assignLeft.FindByFathers(moduleAssigned.GetFathers());
			assign.Add(moduleRef.GetFathers().FindByType(typeof(Declarator)));
			result -= (moduleRef * moduleAssigned);
		}
	}
	catch(Exception e)
	{
		cxLog.WriteDebugMessage(e.Message);	
	}
}