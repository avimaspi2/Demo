CxList struts = All.FindByFileName("*.xml").FindByFileName("*struts*");
CxList strings = Find_Strings();
CxList actionMethods = struts.FindByMemberAccess("ACTION.METHOD").GetFathers();
CxList actionTypes = struts.FindByMemberAccess("ACTION.TYPE").GetFathers();
actionTypes.Add(struts.FindByMemberAccess("ACTION.CLASS").GetFathers());

actionMethods = strings.GetByAncs(actionMethods);
actionTypes = strings.GetByAncs(actionTypes);

CxList actions = actionMethods.GetAncOfType(typeof(StatementCollection));
actionTypes = actionTypes.GetByAncs(actions);
CxList methodDecl = Find_MethodDeclaration();

CxList actionClass = Find_Action_Classes();
CxList executes = methodDecl.FindByShortName("execute");
executes.Add(methodDecl.FindByShortName("perform"));

executes = executes.GetByAncs(actionClass);
CxList methodsInAction = methodDecl.GetByAncs(actionClass);

if (actionTypes.Count > 0)
{
	foreach (CxList actionMethod in actionMethods)
	{
		CxList action = actionMethod.GetAncOfType(typeof(StatementCollection));
		CxList actionType = actionTypes.GetByAncs(action);
		if (actionType != null)
		{
			try
			{
				CSharpGraph gType = actionType.TryGetCSharpGraph<CSharpGraph>();
				CxList typeName = actionClass.FindByType(gType.Text.Trim('"'));
				CSharpGraph gMethod = actionMethod.TryGetCSharpGraph<CSharpGraph>();
				CxList execute = methodsInAction.GetByAncs(typeName).FindByShortName(gMethod.Text.Trim('"'));
				executes.Add(execute);
			}
			catch (Exception ex)
			{
				// just in case
				cxLog.WriteDebugMessage(ex);
			}
		}
	}
}

result = executes;