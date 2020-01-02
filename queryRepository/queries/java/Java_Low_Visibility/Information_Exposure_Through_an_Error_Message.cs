///////////////////////////////////////////////////////////////////////////
// Query: Information_Exposure_Through_an_Error_Message
// Purpose: Find exception data that goes to output
///////////////////////////////////////////////////////////////////////////

CxList deadCode = Find_Dead_Code_Contents();
CxList ctch = base.Find_Catch();

// Find all exceptions
CxList exception = Find_Declarators().GetByAncs(ctch);
CxList excDecl = All.FindByFathers(ctch).FindByType(typeof(VariableDeclStmt));
CxList exp = exception.GetByAncs(excDecl);
//Exceptions outside of catchs
CxList exceptionsNoCatch = All.FindByMemberAccess("Exception.*").GetTargetOfMembers();
exceptionsNoCatch -= exceptionsNoCatch.GetByAncs(ctch);

//find the outputs that are influenced by an Exception as a cast 
//example: ((Exception)r.GetException()).printStackTrace();
CxList allExceptions = All.FindByTypes(new String[]{"*Exception","Throwable"});
allExceptions = allExceptions.GetAncOfType(typeof(CastExpr));

//the cast to exception alone doesn't influence the output so we have to go get the ancs that are included in allExceptions 
//example: ((Exception)r.GetException()).printStackTrace();
CxList allExceptionsInflu = All.GetByAncs(allExceptions).FindByShortName(allExceptions);
exp.Add(exceptionsNoCatch);
exp.Add(allExceptionsInflu);

CxList outputs = Find_Outputs();
//A log is not an output for an error message, and logging exceptions is correct behavior.
//Even if sensitive information is being logged, there's a different query for that.
outputs -= Find_Log_Outputs();
CxList sanitize = Find_Integers();
sanitize.Add(deadCode);

result = outputs.InfluencedByAndNotSanitized(exp, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);