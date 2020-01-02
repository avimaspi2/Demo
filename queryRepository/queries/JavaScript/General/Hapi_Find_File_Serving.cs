// This query searches for files or directories being served in Hapi routes 
CxList replyObjects = Hapi_Find_Reply_Interface();
CxList allRoutes = Hapi_Find_Routes();

CxList fieldDecls = Find_FieldDecls();
CxList associativeArrayExprs = Find_AssociativeArrayExpr();

List <string> staticFileServingList = new List<string> {"file", "directory"};
CxList path = All.FindByShortName("path");

CxList handlersUnderRoutes = Hapi_Find_Handler_Method_Under_Route(allRoutes);

CxList filesAndDirectories = fieldDecls.GetByAncs(associativeArrayExprs.FindByFathers(handlersUnderRoutes))
	.FindByShortNames(staticFileServingList);

CxList paths = path.GetByAncs(filesAndDirectories);
result.Add(paths);	

result.Add(filesAndDirectories.FindByShortName("file"));

result.Add(replyObjects.GetMembersOfTarget().FindByShortName("file"));