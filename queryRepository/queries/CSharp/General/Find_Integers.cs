List<string> strType = new List<string> {"int", "Int16", "Int32", "Int64", "UInt64", "DateTime",
		"UInt32", "UInt16", "long", "ulong", "uint", "short", "decimal", "Decimal", "float", "Single", 
		"double", "Double", "bool", "Boolean", "Guid", "System.Int16", "System.Int32", "System.Int64",
		"System.UInt64", "System.UInt32", "System.UInt16", "System.Decimal", "System.Single", "System.Boolean", "System.Double" };

// use FindByTypes instead of several FindByType
CxList ints = All.FindByTypes(strType.ToArray());

CxList methods = Find_Methods(); 

CxList allCastExpr = All.FindByType(typeof(CastExpr));
CxList castInts = All.NewCxList();
foreach(CxList ace in allCastExpr)
{
	CastExpr ce = ace.data.GetByIndex(0) as CastExpr;
	if(ce != null)
	{
		string typeName = ce.TargetType.TypeName;	
		if(strType.Contains(typeName))
		{
			castInts.Add(ace);
		}
	}
}

castInts = (All.FindByType(typeof(UnknownReference)) + methods + All.FindByType(typeof(MemberAccess))).FindByFathers(castInts);
castInts -= castInts.GetMembersOfTarget().GetTargetOfMembers();

CxList converted = methods.FindByName("Convert.ToBoolean");
converted.Add(methods.FindByName("Convert.ToByte"));
converted.Add(methods.FindByName("Convert.ToSByte"));
converted.Add(methods.FindByName("Convert.ToDateTime"));
converted.Add(methods.FindByName("Convert.ToInt16"));
converted.Add(methods.FindByName("Convert.ToInt32"));
converted.Add(methods.FindByName("Convert.ToInt64"));
converted.Add(methods.FindByName("Convert.ToDecimal"));
converted.Add(methods.FindByName("Convert.ToDouble"));
converted.Add(methods.FindByName("Convert.ToSingle"));
converted.Add(methods.FindByName("Convert.ToUInt16"));
converted.Add(methods.FindByName("Convert.ToUInt32"));
converted.Add(methods.FindByName("Convert.ToUInt64"));
	
foreach (string type in strType)
{
	converted.Add(methods.FindByMemberAccess(type + ".Parse"));
}

converted.Add(methods.FindByMemberAccess("Enum.Parse"));
	
CxList temp = methods.FindByMemberAccess("Request.*");
CxList byNames = All.FindByShortNames(new List<string> {"*Length*","*Index*","*Contains*"}, false);
byNames.Add(temp.FindByMemberAccess("Request.MapImageCoordinates"));
byNames.Add(temp.FindByMemberAccess("Request.ContentLength"));
byNames.Add(temp.FindByMemberAccess("Request.TotalBytes"));
byNames.Add((temp.FindByMemberAccess("Request.MapImageCoordinates") +
	temp.FindByMemberAccess("Request.ContentLength") +
	temp.FindByMemberAccess("Request.TotalBytes")).GetTargetOfMembers());
byNames.Add(Find_Integers_Components());

CxList binary = All.FindByType(typeof(BinaryExpr));
CxList booleanConditions = binary.FindByShortNames(new List<string> {
		"<", ">",
		"==", "!=", "<>",
		"<=", ">=",
		"||", "&&"});
booleanConditions.Add(All.FindByType(typeof(UnaryExpr)).FindByShortName("Not"));
	
booleanConditions -= booleanConditions.FindByRegex(@"\?\?");

CxList nonSanitizer = All.FindByType(typeof(AssignExpr)).GetByAncs(booleanConditions);
booleanConditions -= nonSanitizer.GetAncOfType(typeof(BinaryExpr));

temp = methods.FindByMemberAccess("String.*");
CxList stringIntegers = temp.FindByMemberAccess("String.Compare*");
stringIntegers.Add(temp.FindByMemberAccess("String.Contains"));
stringIntegers.Add(temp.FindByMemberAccess("String.EndsWith"));
stringIntegers.Add(temp.FindByMemberAccess("String.Equals"));
stringIntegers.Add(temp.FindByMemberAccess("String.StartsWith"));
stringIntegers.Add(methods.FindByShortName("IsNumber"));

CxList dbMethods = Find_DB_DataAdapter_Fill();
dbMethods.Add(Find_DB_DataAdapter_Update());
CxList commandDSource = Find_DB_Command_DataSource_QSqlQuery();
dbMethods.Add(commandDSource.FindByShortNames(new List<string> {"delete","insert","update"}, false));
dbMethods.Add(Find_DB_Command_ExecuteNonQuery());
dbMethods.Add(Find_DB_Entlib_ExecuteNonQuery());
dbMethods.Add(Find_DB_Entlib_Update());
dbMethods.Add(Find_DB_EF_DBContext().GetMembersOfTarget().FindByShortName("ExecuteStoreCommand"));

CxList enums = All.FindByType(typeof(EnumDecl));
enums = All.FindByType(enums);

CxList MethodDecls = All.FindByType(typeof(MethodDecl));
CxList returnTypes = All.NewCxList();
foreach (CxList mdecl in MethodDecls){
	MethodDecl mdec = mdecl.TryGetCSharpGraph<MethodDecl>();
	if(mdec != null && mdec.ReturnType != null)
		returnTypes.Add(mdec.ReturnType.NodeId, mdec.ReturnType);
}

CxList ancOfTypeInts = (returnTypes * ints).GetAncOfType(typeof(MethodDecl));
CxList allRefMetInvExp = All.FindAllReferences(ancOfTypeInts).FindByType(typeof(MethodInvokeExpr));

result.Add(ints);
result.Add(converted);
result.Add(byNames);
result.Add(booleanConditions);
result.Add(stringIntegers);
result.Add(enums);
result.Add(dbMethods);
result.Add(castInts);
result.Add(allRefMetInvExp);
//this method return a boolean
result.Add(methods.FindByMemberAccess("RequestExtensions.IsUrlLocalToHost", true));
result.Add(methods.FindByMemberAccess("Request.IsUrlLocalToHost", true));