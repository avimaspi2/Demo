//This query is not taking into account With Statement

CxList methods = Find_Methods();

CxList allOpens = methods.FindByShortName("open");
allOpens.Add(methods.FindByShortName("file"));
allOpens.Add(Find_Methods_By_Import("io", new string[]{"FileIO", "open", "BufferedReader", "BufferedRandom",
	"BufferedRWPair", "TextIOBase", "TextIOWrapper"}));
allOpens.Add(Find_Methods_By_Import("codecs", new string[]{"StreamReader", "StreamReaderWriter", "StreamRecoder"}));
allOpens.Add(Find_Methods_By_Import("fileinput", new string[]{"input", "FileInput"}));
allOpens.Add(Find_Methods_By_Import("socket", new string[]{"*.bind", "*.create_connection", "*.connect", "*.connect_ex"}));
allOpens.Add(Find_Methods_By_Import("urllib2", new string[]{"urlopen", "urlretrieve"}));
allOpens.Add(Find_Methods_By_Import("urllib", new string[]{"request.urlopen", "request.urlretrieve", "parse.urlparse"}));
allOpens.Add(Find_Methods_By_Import("httplib", new string[]{"HTTPConnection", "HTTPSConnection"}));
allOpens.Add(Find_Methods_By_Import("http", new string[]{"client.HTTPConnection", "client.HTTPSConnection"}));
allOpens.Add(Find_Methods_By_Import("httplib2", new string[]{"*request"}));

CxList wsgiHandlers = Find_Methods_By_Import("django.core.handlers", new string[]{"wsgi.WSGIHandler"});
CxList wsgiHandlerParameters  = Find_ParamDecl().DataInfluencedBy(wsgiHandlers);
allOpens.Add(methods.FindAllReferences(wsgiHandlerParameters));


CxList assigneesOfOpens = allOpens.GetAssignee();
CxList opensWithoutAssignee = allOpens - allOpens.GetByAncs(assigneesOfOpens.GetAncOfType(typeof(AssignExpr)));
CxList targets = opensWithoutAssignee.GetTargetOfMembers();
allOpens.Add(targets);

CxList allClose = methods.FindByShortName("close");

//Get closes that are inside a finally block
CxList tryStatements = Find_TryCatchFinallyStmt();
CxList finallyStatements = All.GetFinallyClause(tryStatements); 
CxList closesNotInFinally = allClose - allClose.GetByAncs(finallyStatements);

CxList openWithClose = allOpens.InfluencingOn(allClose);
CxList opensWithInvalidCloses = openWithClose.InfluencingOn(closesNotInFinally);
CxList opensWithoutCloses = allOpens - openWithClose;

targets -= openWithClose;

//Get Flow for opens without closes
CxList assignees = opensWithoutCloses.GetAssignee();
assignees.Add(targets);
CxList references = All.FindAllReferences(assignees);
result = assignees.InfluencingOn(references).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);

result.Add(opensWithInvalidCloses.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow));