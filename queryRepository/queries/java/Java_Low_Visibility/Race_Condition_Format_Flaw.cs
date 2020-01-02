// this quey should find all using of 'format' and 'parse' where implementation
// of those fuction imported from the WEB

CxList methodsInvoke = Find_Methods();
CxList methodsDecl = Find_MethodDeclaration();
 
List < string > classList  = new List<string>(
	new string [] {"SimpleDateFormat", "DecimalFormat", "ChoiceFormat", "MessageFormat" , "NumberFormat", "DateFomat" });
List < string > methodList = new List<string>(new string [] {"format", "parse"});

CxList directCall = All.NewCxList();

// find direct call
foreach (string cl in classList)
{
	CxList methodAccessInvoke = methodsInvoke.FindByMemberAccess(cl + ".*");
	foreach (string md in methodList)
	{
		directCall.Add(methodAccessInvoke.FindByMemberAccess(cl + "." + md));
	}
}


CxList classes = Find_Class_Decl();
CxList inherit = All.NewCxList();

// find all classes that inherit from one of classes in class list
foreach (string cl in classList)
{
	inherit.Add(classes.InheritsFrom(cl));
}

CxList inheritCall = All.NewCxList();

// for each inherit class
foreach (KeyValuePair<int, IGraph> pd in inherit.data)
{
	ClassDecl p = pd.Value as ClassDecl;

	if (p != null)
	{
		// find class name
		string className = p.FullName;
		string classShortName = p.Name;
		CxList implementedFormatAll = methodsDecl.FindByName(className + ".*");
		// for each method
		foreach (string md in methodList)
		{
			// find if inherit class implement function
			CxList implementedFormat = implementedFormatAll.FindByName(className + "." + md);
			if (implementedFormat.Count == 0) // if inherint class NOT implement function add it
				inheritCall.Add(methodsInvoke.FindByMemberAccess(classShortName + "." + md));
			else
			{
				directCall = directCall - methodsInvoke.FindByMemberAccess(classShortName + "." + md);
			}
		}
	}
}

// find the following calling NumberFormat.getInstance().format(25);
CxList callWithgetInstance = All.NewCxList();
foreach (string cl in classList)
{
	CxList callBygetInstanceTemp = methodsInvoke.FindByMemberAccess(cl + ".getInstance").GetMembersOfTarget();
	foreach (string md in methodList)
	{
		CxList callBygetInstance = callBygetInstanceTemp.FindByShortName(md);
		if (callBygetInstance.Count > 0)
			callWithgetInstance.Add(callBygetInstance);
	}
}

result = directCall + inheritCall + callWithgetInstance;
//Remove all local variables from the results  
CxList LocalVars = All.NewCxList(); 
CxList tempReselts = All.NewCxList(); tempReselts.Add(directCall);
foreach(CxList curentvar in tempReselts)
{
	CxList target = curentvar.GetLeftmostTarget();
	CxList varDefinition = All.FindDefinition(target);
	CxList ParamDec = varDefinition.FindByType(typeof(ParamDecl));
	CxList varAnks = varDefinition.GetAncOfType(typeof(MethodDecl));
	if ((varAnks.Count > 0) && (ParamDec.Count == 0))  
	{
		LocalVars.Add(curentvar);	
	}
}	
result -= LocalVars;