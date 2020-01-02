CxList instantiations = Find_Object_Create();
CxList strings = Find_String_Literal();

List <string> names = new List<string> {"BasicAWSCredentials","BasicSessionCredentials"};

CxList awsConstructor = instantiations.FindByShortNames(names);

CxList keyParams = All.GetParameters(awsConstructor, 1);

// Flows from String literals to parameters 
result = keyParams.InfluencedBy(strings);

// Hardcoded strings in the parameter
result.Add(keyParams * strings);