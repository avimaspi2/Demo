CxList firstParam = 
	All.FindByMemberAccess("HTMLDB_ITEM.TEXT_FROM_LOV", false) + 
	All.FindByMemberAccess("HTMLDB_ITEM.TEXT_FROM_LOV_QUERY", false);

CxList secondParam = 
	All.FindByMemberAccess("HTMLDB_ITEM.CHECKBOX", false) + 
	All.FindByMemberAccess("HTMLDB_ITEM.HIDDEN", false) + 
	All.FindByMemberAccess("HTMLDB_ITEM.MD5_HIDDEN", false);

CxList thirdParam = 
	All.FindByMemberAccess("HTMLDB_ITEM.DATE_POPUP", false) + 
	All.FindByMemberAccess("HTMLDB_ITEM.TEXTAREA", false) + 
	All.FindByMemberAccess("HTMLDB_ITEM.TEXT", false);

CxList popMethods = 	
	All.FindByMemberAccess("HTMLDB_ITEM.POPUP_FROM_LOV", false) + 
	All.FindByMemberAccess("HTMLDB_ITEM.POPUPKEY_FROM_LOV", false);

//removing methods with escape parameter 
CxList escape_pop = All.GetParameters(popMethods, 6).FindByShortName("*YES*", false).GetAncOfType(typeof(MethodInvokeExpr));
popMethods -= escape_pop;

CxList radioGroup = All.FindByMemberAccess("HTMLDB_ITEM.RADIOGROUP", false);
radioGroup = All.GetParameters(radioGroup) - All.GetParameters(radioGroup,0);

CxList selectMethods =
	All.FindByMemberAccess("HTMLDB_ITEM.SELECT_LIST", false) + 
	All.FindByMemberAccess("HTMLDB_ITEM.SELECT_LIST_FROM_LOV", false) + 
	All.FindByMemberAccess("HTMLDB_ITEM.SELECT_LIST_FROM_LOV_X", false);
selectMethods = All.GetParameters(selectMethods) - All.GetParameters(selectMethods,0);
	
	
result = 
	All.GetParameters(firstParam, 0) +
	All.GetParameters(secondParam, 1) + 
	All.GetParameters(thirdParam, 2) + 
	All.GetParameters(popMethods, 2) + 
	radioGroup + 
	selectMethods;