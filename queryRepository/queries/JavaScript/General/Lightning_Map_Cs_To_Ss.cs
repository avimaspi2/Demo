Lightning_Map_Component_To_Controller();
CxList apex = Get_Apex();
CxList apexControllers = apex.FindByFileName("*Controller.cls");

CxList ssControllerMapping = cxXPath.FindXmlNodesByQualifiedName("*.cmp", 8, "aura", "component", true);

Dictionary<string,CxList> clientSideToServerSideMapping = new Dictionary<string,CxList>();


foreach(CxList controllerMapping in ssControllerMapping)
{
	CxXmlNode controllerMappingAsNode = controllerMapping.GetFirstGraph() as CxXmlNode;
	
	if(controllerMappingAsNode != null &&controllerMappingAsNode.LinePragma.FileName!=null)
	{
		string viewFileName = controllerMappingAsNode.LinePragma.FileName;
		
		string controllerName = controllerMappingAsNode.GetAttributeValueByName("controller");
		if(controllerName != string.Empty)
		{
			if(!clientSideToServerSideMapping.ContainsKey(viewFileName))
			{
				string fileName = "*" + controllerName + ".cls";
				
				CxList relevant = apexControllers.FindByFileName(fileName);
				
				Dictionary<string,string> sharedData = querySharedData.GetSharedData("Lightning_Cmp_To_Cont") as Dictionary<string,string>;
				
				viewFileName = viewFileName.ToLower();
				
				if(sharedData.ContainsKey(viewFileName))
				{
					string clientCName = sharedData[viewFileName];
					
					clientCName = clientCName.ToLower();
					
					if(!clientSideToServerSideMapping.ContainsKey(clientCName))
					{
						clientSideToServerSideMapping.Add(clientCName, relevant);
						
					}
				}				
			}
		}
	}	
}
querySharedData.AddSharedData("Lightning_CS_To_SS", clientSideToServerSideMapping, true);