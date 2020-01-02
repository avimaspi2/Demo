CxList methods = Find_Methods();

result = methods.FindByMemberAccess("MessageQueue.Receive");
result.Add(methods.FindByMemberAccess("MessageQueue.GetAllMessages"));