CxList inputs = Find_Interactive_Inputs();
CxList methodInvokes = Find_Methods();

// to open a file
CxList files = Find_Open_Files_Methods() + methodInvokes.FindByShortName("unlink");

// only the first param of the method counts
CxList firstParam = All.GetByAncs(All.GetParameters(files, 0));

CxList streams = All.FindByTypes(new String[] {"ifstream", "filebuf", "fstream", "ofstream"});
CxList definition = All.FindDefinition(streams);

CxList prob = firstParam + definition;
CxList create = Find_ObjectCreations();
CxList par = All.GetByAncs(create).InfluencingOn(create);
CxList cond = Get_Conditions().FindAllReferences(par);
prob -= prob.GetByAncs(cond.GetAncOfType(typeof(IfStmt)));

CxList sanitize = Find_Sanitize();
result = inputs.InfluencingOnAndNotSanitized(prob, sanitize);