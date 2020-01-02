/*This query fills the hapi dictionary to make it easy to find sanitized inputs in Hapi_Find_Sanitize query. */
CxList routeClassDecl = Hapi_Find_Routes();
CxList allClassDecls = Find_Classes();

Dictionary<int, int> HapiDictionary = new Dictionary<int, int>();
CxList validateKey = All.FindByShortName("validate");
CxList handlersKey = All.FindByShortName("handler");
foreach(CxList route in routeClassDecl)
{
	CxList handler = Hapi_Find_Handler_Method_Under_Route(route, handlersKey);
	
	//Remove the hash handlers. Keep only the method handlers.
	handler -= allClassDecls;
	CxList validate = Hapi_Find_Validate_Under_Config(route, validateKey);
	
	//Fill the dictionary
	try 
	{
		CSharpGraph csHandler = handler.GetFirstGraph();
		CSharpGraph csValidate = validate.GetFirstGraph();
		if(csHandler != null && csValidate != null)
		{
			HapiDictionary.Add(csHandler.NodeId, csValidate.NodeId);
		}
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}
querySharedData.AddSharedData("HapiDictionary", HapiDictionary, true);