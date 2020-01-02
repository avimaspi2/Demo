CxList trueValues = All.FindByAbstractValue(absInt => {
	return (absInt is StringAbstractValue stringAbstractValue && stringAbstractValue.Content.Equals("True", StringComparison.InvariantCultureIgnoreCase)) ||
		absInt is TrueAbstractValue;
	});

CxList persistSecurity = All.FindByAbstractValue(absInt => 
	absInt is StringAbstractValue stringAbstractValue && stringAbstractValue.Content.Equals("Persist Security Info", StringComparison.InvariantCultureIgnoreCase)
).GetFathers();


// Get all the builder.PersistSecurityInfo = true
CxList buildersPersistSecurity = All.FindByMemberAccess("*ConnectionStringBuilder.PersistSecurityInfo");

// Get all builder["Persist Security Info"] = "True"
string[] connectionStrings = new String[]{"SqlConnectionStringBuilder",
	"OdbcConnectionStringBuilder",
	"OleDbConnectionStringBuilder",
	"OracleConnectionStringBuilder"};

buildersPersistSecurity.Add(persistSecurity.FindByTypes(connectionStrings).FindByType(typeof(IndexerRef)));

CxList AssingedToPersistSecurity = buildersPersistSecurity.GetAssigner(All);

// Get All builder.Add("Persist Security Info", true)
CxList buildersAddPersistSecurity = All.FindByMemberAccess("*ConnectionStringBuilder.Add");
CxList buildersAddVuln = buildersAddPersistSecurity.FindByParameterValue(0, "Persist Security Info", BinaryOperator.IdentityEquality);

AssingedToPersistSecurity.Add(All.GetParameters(buildersAddVuln, 1));

//Vulnerable ConnectionStrings
CxList strings = Find_Strings();
result = strings.FindByRegex(@"Persist Security Info\s*=")
	- strings.FindByRegex(@"Persist Security Info\s*=\s*[Ff]alse");

result.Add(AssingedToPersistSecurity * trueValues);
AssingedToPersistSecurity -= trueValues;
AssingedToPersistSecurity = All.FindByFathers(AssingedToPersistSecurity);

CxList inputs = Find_Inputs();
result.Add(AssingedToPersistSecurity.InfluencedBy(inputs).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow));