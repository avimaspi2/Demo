// Find usage of NSXMLParser that allows shouldResolveExternalEntities.
CxList entitiesOn = All.FindByMemberAccess("NSXMLParser.shouldResolveExternalEntities");
entitiesOn.Add(All.FindByMemberAccess("XMLParser.shouldResolveExternalEntities"));

CxList booleanLiterals = Find_BooleanLiteral();

CxList trueStmts = booleanLiterals.FindByShortName("YES");	
trueStmts.Add(booleanLiterals.FindByShortName("true", false));

trueStmts = trueStmts.FindByFathers(entitiesOn.GetFathers()).GetFathers();
entitiesOn = entitiesOn.FindByFathers(trueStmts);
result = entitiesOn;

// Find an NSXMLParser that is influenced by user input.
String[] XMLParserObjects = new String[]{"NSXMLParser","XMLParser"};
CxList parsers = All.FindByTypes(XMLParserObjects);
CxList inputs = Find_Interactive_Inputs() - Find_Interactive_Inputs_User();
CxList sanitized = Find_General_Sanitize();
result.Add(parsers.InfluencedByAndNotSanitized(inputs, sanitized));
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);