//this query will create custom flows between server side controller action return value and the equivalent response in 
//the client side controller

Lightning_Map_Cs_To_Ss();

Dictionary<string,CxList> mappingDictionary = querySharedData.GetSharedData("Lightning_CS_To_SS") as Dictionary<string,CxList>;
CxList allInLightningController = Lightning_Find_All_In_Controller();
CxList accessToSSController = allInLightningController.FindByShortName("c.*");
CxList apex = Get_Apex();
CxList nullRef = apex.FindByShortName("null");
CxList returnStmt = apex.FindByType(typeof(ReturnStmt));
CxList directChildOfReturn = (apex - nullRef).FindByFathers(returnStmt);
foreach(CxList ssElementAccess in accessToSSController)
{
	CxList sinkOfFlow = Lightning_Find_Response_Return_Value(ssElementAccess);			
	string currentFileName = ssElementAccess.GetFirstGraph().LinePragma.FileName;
	if(mappingDictionary.ContainsKey(currentFileName))
	{
		CxList relevant = (mappingDictionary[currentFileName]);
		string methodName = ssElementAccess.GetName().Remove(0, 2);		
		CxList relevantMethods = relevant.FindByShortName(methodName, false);
		CxList source = directChildOfReturn.GetByAncs(relevantMethods);
		foreach(CxList s in source)
		{								
			CustomFlows.AddFlow(s, sinkOfFlow);															
		}
	}		
	
}