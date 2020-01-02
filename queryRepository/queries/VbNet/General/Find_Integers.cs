List<string> strType = new List<string>(){          

		"int", "int16", "int32", "int64", "long","decimal", "float","double", "cdate", "bool"};

CxList ints = All.FindByTypes(strType.ToArray());

CxList methods = Find_Methods();
CxList convertMethods = methods.FindByName("Convert.*", false);

List < String > conversionMethods = new List<String>(new string[] {
	"tobyte", "tosbyte", "todatetime", "toint16", "toint32", "toint64", 
	"todecimal", "todouble", "tosingle", "touint16", "touint32", "touint64"});

CxList converted = methods.FindByName("convert.toboolean", false);
converted.Add(methods.FindByMemberAccess("datetime.parse", false));
converted.Add(methods.FindByMemberAccess("bool.parse", false));
converted.Add(methods.FindByMemberAccess("int.parse", false));
converted.Add(methods.FindByMemberAccess("double.parse", false));
converted.Add(methods.FindByMemberAccess("guid.parse", false));

converted.Add(convertMethods.FindByShortNames(conversionMethods, false));

converted.Add(methods.FindByName("cDate", StringComparison.OrdinalIgnoreCase)); 
converted.Add(methods.FindByName("cInt", StringComparison.OrdinalIgnoreCase)); 
converted.Add(methods.FindByName("cByte", StringComparison.OrdinalIgnoreCase)); 
converted.Add(methods.FindByName("cDbl", StringComparison.OrdinalIgnoreCase)); 
converted.Add(methods.FindByName("cDec", StringComparison.OrdinalIgnoreCase)); 
converted.Add(methods.FindByName("cLng", StringComparison.OrdinalIgnoreCase)); 
converted.Add(methods.FindByName("csByte", StringComparison.OrdinalIgnoreCase)); 
converted.Add(methods.FindByName("csShort", StringComparison.OrdinalIgnoreCase)); 
converted.Add(methods.FindByName("cuInt", StringComparison.OrdinalIgnoreCase)); 
converted.Add(methods.FindByName("cuLongt", StringComparison.OrdinalIgnoreCase)); 
converted.Add(methods.FindByName("cuShort", StringComparison.OrdinalIgnoreCase)); 

converted.Add(methods.FindByName("int.parse", StringComparison.OrdinalIgnoreCase)); 
converted.Add(methods.FindByName("int16.parse", StringComparison.OrdinalIgnoreCase)); 
converted.Add(methods.FindByName("int32.parse", StringComparison.OrdinalIgnoreCase)); 
converted.Add(methods.FindByName("int64.parse", StringComparison.OrdinalIgnoreCase)); 
converted.Add(methods.FindByName("long.parse", StringComparison.OrdinalIgnoreCase)); 
converted.Add(methods.FindByName("decimal.parse", StringComparison.OrdinalIgnoreCase)); 
converted.Add(methods.FindByName("double.parse", StringComparison.OrdinalIgnoreCase)); 
converted.Add(methods.FindByName("float.parse", StringComparison.OrdinalIgnoreCase));
converted.Add(methods.FindByName("enum.parse", StringComparison.OrdinalIgnoreCase));

List < String > byNamesStrings = new List<String>(new string[] {"len", "*length*","*index*"});
CxList byNames = All.FindByShortNames(byNamesStrings, false);
byNamesStrings = new List<String>(new string[] {"request.mapimagecoordinates", "request.contentlength", "request.totalbytes"});
byNames.Add(All.FindByShortNames(byNamesStrings, false));
byNames.Add(
	(All.FindByMemberAccess("request.mapimagecoordinates", false) +
	All.FindByMemberAccess("request.contentlength", false) +
	All.FindByMemberAccess("request.totalbytes", false)).GetTargetOfMembers());
byNames.Add(Find_Integers_Components());

CxList binary = All.FindByType(typeof(BinaryExpr));
List < String > booleanConditionsStrings = new List<String>(new string[] {"<",">","==","!=","<>","<=",">=","||","&&"});
CxList booleanConditions = binary.FindByShortNames(booleanConditionsStrings);
booleanConditions.Add(All.FindByType(typeof(UnaryExpr)).FindByShortName("Not", false));


CxList nonSanitizer = All.FindByType(typeof(AssignExpr)).GetByAncs(booleanConditions);
booleanConditions -= nonSanitizer.GetAncOfType(typeof(BinaryExpr));

// Enums
CxList enumDecl = All.FindByType(typeof(EnumDecl)); // all the enum declarations
CxList enumRef = All.FindAllReferences(enumDecl); // all the enums appearing in the program
CxList decl = All.FindByType(typeof(Declarator)); // all declarators

CxList enumRefDecl = enumRef.GetAncOfType(typeof(FieldDecl)); 
enumRefDecl.Add(enumRef.GetAncOfType(typeof(VariableDeclStmt))); // declarations that are of type Enum

//CxList enumTypeDecls = enumRef.GetAncOfType(typeof(ParamDecl)) + decl.GetByAncs(enumRefDecl); // All declarations of type enum
CxList enumTypeDecls = enumRef.GetAncOfType(typeof(ParamDecl));
enumTypeDecls.Add(decl.GetByAncs(enumRefDecl)); // All declarations of type enum

CxList enums = All.FindAllReferences(enumTypeDecls); // All references of enums' declarations
	
CxList dbMethods = Find_DB_DataAdapter_Fill();
dbMethods.Add(Find_DB_DataAdapter_Update());
dbMethods.Add(Find_DB_EntLib_ExecuteNonQuery());
dbMethods.Add(Find_DB_Command_ExecuteNonQuery());
CxList commandDSource = Find_DB_Command_DataSource_QSqlQuery();
dbMethods.Add(commandDSource.FindByShortName("update",false));
dbMethods.Add(commandDSource.FindByShortName("insert",false));
dbMethods.Add(commandDSource.FindByShortName("delete",false));
dbMethods.Add(Find_DB_EntLib_Update());


//Casts
CxList allCastExpr = All.FindByType(typeof(CastExpr));

CxList castings = All.NewCxList();

foreach(CxList ace in allCastExpr)
{
	try
	{
		CastExpr ce = ace.data.GetByIndex(0) as CastExpr;

		if(ce != null)
		{
			string typeName = ce.TargetType.TypeName; 

			if(strType.Contains(typeName))
			{
				castings.Add(ace);
			}
			// also consider Enums used as castings
			else if(All.FindDefinition(All.FindById(ce.TargetType.NodeId)) 
				.FindByType(typeof(EnumDecl)).Count > 0)
			{
				castings.Add(ace); 
			}
		}
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

CxList paramsCast = All.FindByType(typeof(UnknownReference));

paramsCast.Add(methods);

paramsCast.Add(All.FindByType(typeof(MemberAccess)));

castings = paramsCast.FindByFathers(castings);


//result = ints + converted + byNames + booleanConditions + enums/*+ methodInts*/;
result = ints;
result.Add(converted);
result.Add(byNames);
result.Add(booleanConditions);
result.Add(enums);
result.Add(dbMethods);
result.Add(castings);

//this method return a boolean
result.Add(methods.FindByMemberAccess("RequestExtensions.IsUrlLocalToHost", false));
result.Add(methods.FindByMemberAccess("Request.IsUrlLocalToHost", false));