CxList leftIndexer = Find_IndexerRefs().FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList session = All.FindByFathers(leftIndexer);
session.Add(Find_Declarators());
session.Add(Find_UnknownReference());

CxList membAccSess = Find_MemberAccesses();

CxList target = session.FindByShortName("Session*", false);
target.Add(session.FindByShortName("sid", false));
target.Add(session.FindByShortName("s_id", false));
target.Add(membAccSess.FindByName("*Session*.*", false));
target.Add(membAccSess.FindByName("*.Session*", false));
// remove xml data, except the xml data used as the data of a session
CxList xmlBody = target.FindByShortName("*xml*");
result = target - xmlBody;
CxList methods = Find_Methods();
CxList httpMethods = methods.FindByShortNames(new List<string>{"request", "get", "post", "put", "*session*" }, false);
result.Add(xmlBody.DataInfluencingOn(httpMethods));