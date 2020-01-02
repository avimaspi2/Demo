CxList allRoutes = Hapi_Find_Routes();
CxList trueLiterals = Find_BooleanLiteral().FindByShortName("true");
CxList fieldDecls = Find_FieldDecls();

CxList handlers = Hapi_Find_Handler_Method_Under_Route(allRoutes);
CxList directories = fieldDecls.FindByShortName("directory").GetByAncs(handlers);
CxList listings = fieldDecls.FindByShortName("listing").GetByAncs(directories);

result = trueLiterals.GetAssignee().GetAncOfType(typeof(FieldDecl)) * listings;