// - find classes that implement MessageListener
// - from those classes, get the declaration of the method onMessage(Message m)
// inside the onMessage method, it is unsafe to access any member of the 
// message given as parameter unless it has been already cast to a trusted type object
CxList outputs = All.FindByMemberAccess("Message.*");
CxList inputs = Find_MethodDecls().FindByShortName("onMessage");
inputs = inputs.GetByAncs(All.InheritsFrom("MessageListener"));
inputs = Find_ParamDecl().GetByAncs(inputs);

//Find cases where conditions use instanceof (two cases)
//1. x instanceof y, where y is not of type ObjectMessage
//2. !(x instanceof ObjectMessage)
CxList objectMessages = All.FindByShortName("ObjectMessage");

CxList instanceOfs = All.GetByBinaryOperator(BinaryOperator.InstanceOf);

CxList nots = Find_Unarys().FindByName("Not");

CxList instanceOfObjectMessage =
	objectMessages.GetByAncs(instanceOfs).GetAncOfType(typeof(BinaryExpr)).GetByBinaryOperator(BinaryOperator.InstanceOf);

CxList ifSanitizers = instanceOfObjectMessage.GetByAncs(nots).GetAncOfType(typeof(IfStmt));

CxList instanceOfsAll = All.NewCxList();
instanceOfsAll.Add(instanceOfs);
instanceOfsAll -= instanceOfObjectMessage;

ifSanitizers.Add(instanceOfsAll.GetAncOfType(typeof(IfStmt)));

CxList sanitizers = All.GetByAncs(ifSanitizers);

result = inputs.InfluencingOnAndNotSanitized(outputs, sanitizers);