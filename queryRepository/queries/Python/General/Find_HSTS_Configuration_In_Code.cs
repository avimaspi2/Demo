/* find Flask sanitization with flask_sslify module */
CxList flaskApp = Find_UnknownReference()
	.FindAllReferences(All.FindByMemberAccess("flask.Flask").GetAssignee());
CxList sslify = All.FindByMemberAccess("flask_sslify.SSLify");

result = flaskApp.GetParameters(sslify);

if(result.Count == 0){
	/* find Django sanitization with middlewares */
	CxList settings = Find_Django_Config();
	CxList middlewareSettings = settings.FindByShortName("MIDDLEWARE");
	middlewareSettings = settings.FindByType(typeof(StringLiteral)).InfluencingOn(middlewareSettings);
	List < string > midwares = new List<string>(){};
	
	foreach(CxList middleware in middlewareSettings){
		string [] splited = middleware.GetName()
							.Trim("'\"".ToCharArray())
							.Split(".".ToCharArray());
		
		midwares.Add(splited[splited.Length-1]);
	}
	
	if(middlewareSettings.Count > 0){
		CxList callMethods = Find_MethodDecls()
							.FindByShortNames(new List<string>(){"__call__","process_response"});
		
		CxList possiblyMiddlewares = callMethods.GetAncOfType(typeof(ClassDecl));
		CxList middlewares = possiblyMiddlewares.FindByShortNames(midwares);		

		foreach(CxList midware in middlewares){
			if(All.GetByAncs(midware)
				.FindByShortName("Strict-Transport-Security", false)
			.Count > 0){
				result.Add(midware);
			}
		}
	}
}