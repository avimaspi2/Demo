CxList methods = NodeJS_Find_ExpressMembers();
CxList callback = NodeJS_Get_CallBack_Method(All.GetParameters(methods));
CxList callbackParam = All.NewCxList();
CxList parameters = Find_ParamDecl();

if (param.Length > 0)
{
	try {
		parameters = param[0] as CxList;	
	}
	catch (Exception e) {
		cxLog.WriteDebugMessage(e);
	}
}

callbackParam = parameters.GetParameters(callback);

result.Add(callbackParam);