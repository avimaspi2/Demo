if(param.Length == 1 || param.Length == 2)
{
	string className = param[0] as string;
	bool getExtend = param.Length == 2 ? (bool)param[1] : false;
	
	//Extending class in sap.ui.define
	CxList loadedUIComponentsRefs = Find_SAPUI_Objects_By_Name(className);
	CxList uiExtend = loadedUIComponentsRefs.GetMembersOfTarget().FindByShortName("extend");
	
	result = getExtend ? uiExtend : All.GetParameters(uiExtend, 1) - Find_Param();
}