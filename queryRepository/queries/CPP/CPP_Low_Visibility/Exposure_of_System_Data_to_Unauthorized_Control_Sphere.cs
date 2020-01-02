//find all cerr and perror methods that exposure info about errors in the system
CxList methods = Find_Methods();
CxList errOutMethods = All.FindByShortNames(new List<string>{"perror"});
errOutMethods -= errOutMethods.FindByType(typeof (MethodRef));
result = errOutMethods;

//Find enviroment input
CxList envs = Find_Environment_Inputs();
CxList sanitize = Find_Integers();

//find outputs
CxList outputs = Find_Outputs() + Find_Write() + Find_Log_Outputs();

result.Add(envs.InfluencingOnAndNotSanitized(outputs, sanitize));

//find outputs that write to stderr and that write info about errno
CxList errnos = All.FindByShortNames(new List<string>{"errno"});

result.Add(outputs.InfluencedBy(errnos));

result.Add(errnos * outputs);

//find catch blocks that exposure the exception info to output
CxList ctch = Find_Catch();

//find all exceptions that goes to output
CxList exceptionVar = All.NewCxList();
foreach(CxList catchClause in ctch){
	Catch catchDOM = catchClause.TryGetCSharpGraph<Catch>();
	if (catchDOM.Statements.Count > 0 && catchDOM.LocalName != string.Empty) 
	{
		Statement firstStmt = catchDOM.Statements[0];
		exceptionVar.Add(firstStmt.DomId, firstStmt);
	}
}

CxList exceptionDeclarator = Find_Declarators().GetByAncs(exceptionVar);
CxList exception = Find_UnknownReference().FindAllReferences(exceptionDeclarator);

//add output that 
result.Add(outputs.InfluencedBy(exception).ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow));