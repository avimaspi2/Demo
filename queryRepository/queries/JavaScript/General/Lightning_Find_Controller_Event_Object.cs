CxList allInController = Lightning_Find_All_In_Controller(); 
CxList globalNS = All.FindByShortName("CxJSNS*");
	
CxList heuristicActions = Find_MethodDecls().GetByAncs(globalNS);
CxList componentRef = allInController.GetParameters(heuristicActions, 1);

result = allInController.FindByType(typeof(UnknownReference))
	.FindByShortName(componentRef);