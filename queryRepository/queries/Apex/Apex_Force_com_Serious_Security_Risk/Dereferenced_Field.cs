CxList apexFiles = Find_Apex_Files() - Find_Triggers_Code() - Find_Test_Code();
CxList sObjectsInApex = Find_sObjects() * apexFiles;
CxList methods = Find_Methods();
CxList strings = Find_Strings();
CxList ifNotStmt = Find_If_Not_Stmt();
CxList ifYesStmt = Find_If_Yes_Stmt();
CxList ifReturn = Find_If_With_Return_or_Create();
sObjectsInApex -= sObjectsInApex.GetByAncs(sObjectsInApex.GetFathers().FindByType(typeof(MethodDecl)));
sObjectsInApex.Add((apexFiles * methods).FindByShortName("select", false));
CxList sObjectMembers = sObjectsInApex.GetMembersOfTarget();
sObjectMembers -= sObjectMembers.FindByShortName("id"); // Id is unimportant

CxList returnMembers = sObjectMembers.GetByAncs(All.FindByType(typeof(ReturnStmt)));

CxList isAccessible = apexFiles.FindByMemberAccess("getdescribe.isaccessible");

if (isAccessible.Count > 0)
{
	// Remove the case where we only check CRUD (not FLS)
	CxList nonSchemaDescribe = apexFiles.FindByName("*sobjecttype.getdescribe").GetMembersOfTarget();
	nonSchemaDescribe -= apexFiles.FindByName("*schema.sobjecttype.getdescribe").GetMembersOfTarget();
	isAccessible -= nonSchemaDescribe;
}

CxList returnValues = returnMembers - methods;
returnValues -= returnValues.FindByShortName("getid"); // id's are not interesting

if ((isAccessible + Find_General_Permissions()).Count > 0)
{
	// Remove field-level check
	sObjectMembers = returnValues;

	System.Collections.SortedList permissionsList = new System.Collections.SortedList();

	foreach (CxList member in sObjectMembers)
	{
		CSharpGraph g = member.data.GetByIndex(0) as CSharpGraph;
		string name = g.ShortName;
		CxList tar = member.GetTargetOfMembers();
		CSharpGraph gTar = tar.data.GetByIndex(0) as CSharpGraph;
		string typeTar = gTar.TypeName;
	
		if (typeTar == "")
		{
			CxList sel = member.GetTargetOfMembers();
			CxList parameters = strings.GetByAncs(apexFiles.GetParameters(sel));
			CSharpGraph gg = parameters.data.GetByIndex(0) as CSharpGraph;
			typeTar = gg.ShortName.ToLower();
			if (typeTar.IndexOf("from") >= 0)
			{
				typeTar = typeTar.Substring(typeTar.IndexOf("from") + 4).Trim();
				if (typeTar.IndexOf(" ") >= 0)
				{
					typeTar = typeTar.Substring(0, typeTar.IndexOf(" ")).Trim();
				}
			}
			else
			{
				typeTar = "";
			}
		}	

		if (typeTar != "")
		{
			string key = typeTar + "." + name;
			CxList isAccessibleField = All.NewCxList();
			if (permissionsList.ContainsKey(key))
			{
				isAccessibleField = permissionsList[key] as CxList;
			}
			else
			{
				isAccessibleField = Get_Permission("isAccessible", typeTar, name);
				permissionsList[key] = isAccessibleField;
			}

			if (isAccessibleField.Count > 0)
			{
				// Clean check in the beginning of the method
				CxList ifStmt = isAccessibleField.GetAncOfType(typeof(IfStmt));
				CxList ifNot = ifNotStmt * ifStmt;
				CxList ifToIgnore = ifReturn * ifNot;
				ifToIgnore -= returnValues.GetByAncs(ifToIgnore).GetAncOfType(typeof(IfStmt));
				CxList ifNotMethod = apexFiles.GetMethod(ifToIgnore);
				returnValues -= apexFiles.GetByAncs(ifNotMethod);
	
				// Clean check inside if statement
				CxList ifYes = ifYesStmt * ifStmt;
				foreach (CxList stmt in ifYes)
				{
					IfStmt ifGrph = stmt.data.GetByIndex(0) as IfStmt;
					StatementCollection trueS = ifGrph.TrueStatements;
					if (trueS != null)
					{
						returnValues -= returnMembers.GetByAncs(apexFiles.FindById(trueS.NodeId));
					}
				}

				// Clean when the return is in the "else" of the if statement
				foreach (CxList stmt in ifNot)
				{
					IfStmt ifGrph = stmt.data.GetByIndex(0) as IfStmt;
					StatementCollection falseS = ifGrph.FalseStatements;
					if (falseS != null)
					{
						returnValues -= returnMembers.GetByAncs(apexFiles.FindById(falseS.NodeId));
					}
				}
			}
		}
	}
}

CxList VF = Find_VF_Pages();
CxList rendered = VF.FindByShortName("rendered");
rendered = VF.GetByAncs(rendered);

CxList sanitize = rendered + 
	VF.FindByShortName("datatable") + 
	VF.FindByShortName("pageblocktable") +
	VF.FindByShortName("pageblocklist") +
	VF.FindByShortName("datalist") +
	VF.FindByShortName("variable") +
	VF.FindByShortName("repeat");
	
VF -= VF.GetMembersOfTarget().GetTargetOfMembers(); // make sure we get only the rightmost part of the variable
VF -= VF.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList globals = Find_Globals();
VF.Add(globals * methods);
result = VF.InfluencedByAndNotSanitized(returnValues, sanitize) + 
	returnValues * globals;

result -= result.DataInfluencedBy(result);