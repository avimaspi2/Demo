//this query wil create custom flows between components attribute definition and a getter in the controller

CxList declarators = Lightning_Get_Attribute_Declarations();
Lightning_Map_Component_To_Controller();
CxList allCmpRefs = Lightning_Find_Controller_Component_Object();
CxList getters = allCmpRefs.GetMembersOfTarget().FindByShortName("get");
CxList allInController = Lightning_Find_All_In_Controller();
CxList parameter = allInController.FindByType(typeof(Param));
CxList gettersParams = (allInController - parameter).GetParameters(getters, 0);

Dictionary<string,string> cmpToControllerMapping = querySharedData.GetSharedData("Lightning_Cmp_To_Cont") as Dictionary<string,string>;

foreach(CxList input in declarators)
{
	string inputNodeFileName = input.GetFirstGraph().LinePragma.FileName;
	
	if(cmpToControllerMapping.ContainsKey(inputNodeFileName))
	{
		string controllerFileName = cmpToControllerMapping[inputNodeFileName];		
		CxList allGetterParamsInFile = gettersParams.FindByFileName(controllerFileName);
		
		foreach(CxList parameterOfGet in allGetterParamsInFile)
		{
			if(parameterOfGet.GetName().StartsWith("v."))
			{
				string withoutV = parameterOfGet.GetName().Remove(0, 2);
				if(input.GetName().Equals(withoutV))
				{
					CxList equivElement = cxXPath.GetElementOfCreatedDeclaration(input);
					CustomFlows.AddFlow(equivElement, parameterOfGet);						
					
				}
			}
		}			
	}	
	
}