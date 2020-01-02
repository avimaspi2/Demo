CxList columnEncryption = Find_Strings()
	.FindByShortName("Column Encryption Setting")
	.GetFathers();

CxList enables = All.FindByMemberAccess("SqlConnectionColumnEncryptionSetting.Enabled");
enables.Add(Find_Strings().FindByShortName("enabled", false));

CxList abstractStrings = All.FindByAbstractValue(_ => _ is StringAbstractValue);
IAbstractValue validEnabledUpper = new StringAbstractValue("Enabled");
IAbstractValue validEnabledLower = new StringAbstractValue("enabled");
CxList abstractEnables = abstractStrings.FindByAbstractValue(_ => _.Contains(validEnabledUpper) || _.Contains(validEnabledLower));
enables.Add(abstractEnables);

// Get all the builder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled
CxList builders = All.FindByMemberAccess("SqlConnectionStringBuilder.ColumnEncryptionSetting");
 
CxList buildersEnabled = builders.GetAssigner(enables).GetAssignee().GetTargetOfMembers();
CxList buildersEnabledRefs = All.FindAllReferences(buildersEnabled);

// Get all builder["Column Encryption Setting"] = "Enabled"
CxList indexers = All.FindByType("SqlConnectionStringBuilder").FindByType(typeof(IndexerRef));

CxList indexersEnabled = (columnEncryption * indexers).GetAssigner(enables);
indexersEnabled.Add(All.FindAllReferences(indexersEnabled.GetAssignee()).GetByAncs(All.FindByType(typeof(IndexerRef)))); 

// Get All builder.Add("Column Encryption Setting", "Enabled")
CxList buildersAdd = All.FindByMemberAccess("SqlConnectionStringBuilder.Add");

CxList buildersAddEnabled = buildersAdd.FindByParameters(columnEncryption)
	.FindByParameters(enables);

//Safe ConnectionStrings
CxList connStringSafe = All.FindByType(typeof(StringLiteral))
	.FindByRegex(@"Column Encryption Setting\s*=\s*[Ee]nabled");
connStringSafe.Add(connStringSafe.GetAssignee());

CxList safes = All.NewCxList();
safes.Add(buildersEnabled);
safes.Add(buildersEnabledRefs);
safes.Add(indexersEnabled);
safes.Add(connStringSafe);

//Find all methods connection.open
CxList sqlConnections = All.FindByType("SqlConnection");
CxList sqlCommandConnections = All.FindByMemberAccess("SqlCommand.Connection");
CxList connections = sqlConnections;
connections.Add(All.FindByMemberAccess("SqlCommand.Connection"));
CxList opens = connections.GetMembersOfTarget().FindByShortName("Open");
//Find all first parameters of SqlConnection
CxList paramsConnection = All.GetParameters(sqlConnections, 0);
paramsConnection.Add(Find_Connection_String_Concat_Value());
paramsConnection.Add(Find_Connection_String_Value());

//Remove all opens that are influenced by safes
opens -= opens.InfluencedBy(safes); 
//The result is a flow from any first param of sqlconnection to the actual openning of a connection
result = opens.InfluencedBy(paramsConnection);