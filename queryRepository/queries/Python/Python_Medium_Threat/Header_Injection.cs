CxList methods = Find_Methods();

CxList requestsPost = methods.FindByShortName("requests.post");
CxList requestsGet = methods.FindByShortName("requests.get");
CxList urllib2Request = Find_Methods_By_Import("urllib2", new string[]{"Request", "build_opener"});

CxList requestVars = urllib2Request.GetAncOfType(typeof(AssignExpr)); // Assign or declare
requestVars.Add(urllib2Request.GetAncOfType(typeof(Declarator)));

CxList requestobjs = All.GetByAncs(requestVars).FindByAssignmentSide(CxList.AssignmentSide.Left); // assigned to
requestobjs = All.FindAllReferences(requestobjs);

CxList requestMethods = requestobjs.GetMembersOfTarget(); // Everything of the form {requestobjs}.*

CxList requests = All.NewCxList();
requests.Add(All.GetParameters(urllib2Request, 2));
requests.Add(requestMethods.FindByShortName("*.add_header"));
requests.Add(requestMethods.FindByShortName("*.addheaders"));
requests.Add(requestsPost);
requests.Add(requestsGet);
requests.Add(Find_Header_Outputs());

CxList inputs = Find_Inputs();

result = requests.DataInfluencedBy(inputs);