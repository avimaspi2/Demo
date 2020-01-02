CxList methods = Find_Methods();
CxList socket = methods.FindByShortName("socket");
CxList socketIsOnTheRight = socket.FindByAssignmentSide(CxList.AssignmentSide.Right);
CxList allOnTheLeft = All.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList aliasOnTheLeft = allOnTheLeft.GetByAncs(socketIsOnTheRight.GetFathers());

List<string> revNames = new List<string>{"recv","recvfrom"};
CxList recv = methods.FindByShortNames(revNames);

CxList receiver = recv.GetTargetOfMembers();
result.Add(receiver.FindAllReferences(aliasOnTheLeft).GetMembersOfTarget());
 
CxList accept = methods.FindByShortName("accept");
CxList connection = accept.GetTargetOfMembers();
CxList relevantAccept = connection.FindAllReferences(aliasOnTheLeft);
CxList backToAccept = relevantAccept.GetMembersOfTarget().GetFathers();
CxList onTheLeft = allOnTheLeft.GetByAncs(backToAccept.GetFathers());
result.Add(receiver.FindAllReferences(onTheLeft).GetMembersOfTarget());