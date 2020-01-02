CxList controllers = Find_Controllers();

CxList requestMappings = All.GetByAncs(controllers).FindByCustomAttribute("RequestMapping").GetAncOfType(typeof(MethodDecl));

CxList allRequestMappingParams = All.GetParameters(requestMappings);

CxList requestMappingParams = All.NewCxList();
requestMappingParams.Add(allRequestMappingParams);
requestMappingParams -= requestMappingParams.FindByType("BindingResult");
requestMappingParams -= requestMappingParams.FindByType("Errors");
requestMappingParams -= requestMappingParams.FindByType("Model*");
requestMappingParams -= requestMappingParams.FindByType("*session*");
requestMappingParams -= requestMappingParams.FindByType("*response*");

result = requestMappingParams;

result.Add(All.FindByCustomAttribute("ModelAttribute").GetFathers().FindByType(typeof(ParamDecl)));