if (param.Length == 1)
{
	CxList allInputs = param[0] as CxList;
	allInputs.Add(allInputs.GetFathers().FindByType(typeof(Param)));
	CxList methods = Find_Methods();
	CxList memcacheGetters = Find_memcache_Getters();
	CxList memcacheSetters = Find_memcache_Setters();
	
	//Get the correct methods influenced by allInputs
	CxList memcacheMethods = memcacheSetters.GetAncOfType(typeof(MethodInvokeExpr));
	memcacheMethods = All.GetParameters(memcacheMethods);
	memcacheMethods = memcacheMethods.DataInfluencedBy(allInputs) + (memcacheMethods * allInputs);
	memcacheMethods = memcacheMethods.GetAncOfType(typeof(MethodInvokeExpr));
	
	memcacheSetters = memcacheSetters.GetByAncs(memcacheMethods);
	
	CxList findStrings = Find_Strings();
	
	CxList setStrings = findStrings * memcacheSetters + findStrings.GetByAncs(memcacheSetters);
	CxList getStrings = findStrings * memcacheGetters + findStrings.GetByAncs(memcacheGetters);

	CxList setConstants = memcacheSetters.FindAllReferences(All.FindByType(typeof(ConstantDecl)));
	CxList getConstants = memcacheGetters.FindAllReferences(All.FindByType(typeof(ConstantDecl)));
	
	CxList decl = memcacheGetters.GetAncOfType(typeof(Declarator));
	CxList references = All.FindAllReferences(decl).FindByAssignmentSide(CxList.AssignmentSide.Left).DataInfluencedBy(allInputs);
	decl = decl.FindDefinition(references);
	setConstants.Add(All.GetParameters(memcacheGetters.GetByAncs(decl)));
	
	char[] trimChars = new char[6] {' ', '\t', '"', '(', '\r', '\n'};
	
	HashSet<string> str1 = new HashSet<string>();
//	ArrayList str1 = new ArrayList();
	foreach (CxList setA in setStrings)
	{
		CSharpGraph gr = setA.data.GetByIndex(0) as CSharpGraph;
		try
		{
			string name = gr.ShortName.Trim(trimChars);
			if (!str1.Contains(name))
			{
				str1.Add(name);
			}
		}
		catch (Exception ex)
		{
			cxLog.WriteDebugMessage(ex);
		}
	}

	HashSet<string> str2 = new HashSet<string>();
//	ArrayList str2 = new ArrayList();
	foreach (CxList setA in setConstants)
	{
		CSharpGraph gr = setA.data.GetByIndex(0) as CSharpGraph;
		try
		{
			string name = gr.ShortName.Trim(trimChars);
			if (!str2.Contains(name))
			{
				str2.Add(name);
			}
		}
		catch (Exception ex)
		{
			cxLog.WriteDebugMessage(ex);
		}
	}

	CxList attrInput = All.NewCxList();
	foreach (String s in str1)
	{
		attrInput.Add(getStrings.FindByShortName(s));
	}

	foreach (String s in str2)
	{
		attrInput.Add(getConstants.FindByShortName(s));
	}

	//Find fetch and fetchall that are associated to getDelayed.
	CxList getDelayed = attrInput.GetAncOfType(typeof(MethodInvokeExpr));
	getDelayed.Add(getDelayed.GetFathers().GetAncOfType(typeof(MethodInvokeExpr)));
	getDelayed = getDelayed.FindByShortName("getDelayed*");

	attrInput -= attrInput.GetByAncs(getDelayed);
	//Get the methods of the memcached objects.
	getDelayed = getDelayed.GetTargetOfMembers();
	getDelayed = All.FindByType(typeof(UnknownReference)).FindAllReferences(getDelayed);
	getDelayed = getDelayed.GetMembersOfTarget();

	CxList fetches = methods.FindByMemberAccess("Memcached.fetch*");
	attrInput.Add(getDelayed.FindByShortName("fetch*"));
	
	result = attrInput;
}