CxList mav_addobject = All.FindByMemberAccess("ModelAndView.addObject");
CxList mav_setview = All.FindByMemberAccess("ModelAndView.setViewName");
CxList allParamNames = All.GetParameters(mav_addobject, 0);
CxList allParamVals = All.GetParameters(mav_addobject, 1);
CxList unknownRef = Find_UnknownReference();

// For each view parameter
foreach(CxList addObject in mav_addobject){
	CxList paramName = allParamNames.GetParameters(addObject, 0);
	CxList paramVal = allParamVals.GetParameters(addObject, 1);
	
	try{
		CSharpGraph paramName_graph = paramName.TryGetCSharpGraph<CSharpGraph>();
		if(paramName_graph == null){
			continue;
		}
		String param_name = paramName_graph.ShortName;
		param_name = param_name.TrimStart(new char[]{'"'}).TrimEnd(new char[]{'"'});
		int file_id = paramName_graph.LinePragma.GetFileId();
		
		// Find to which view does it go
		CxList setViews = mav_setview.FindByFileId(file_id).DataInfluencedBy(paramVal);
		foreach(CxList setView in setViews){
			CxList viewName = All.GetParameters(setView, 0);
			
			try{
				CSharpGraph viewName_graph = viewName.TryGetCSharpGraph<CSharpGraph>();
				if(viewName_graph == null){
					continue;
				}
				String view_name = viewName_graph.ShortName;
				view_name = view_name.Substring(2, view_name.Length - 4);
				String view_path = "*" + view_name + ".jsp";
				
				// Search for a corresponding variable in the corresponding view file
				CxList paramInView = unknownRef.FindByShortName(param_name)
					.FindByFileName(view_path);
				
				CustomFlows.AddFlow(paramVal, paramInView);
			}catch(Exception ex){
				cxLog.WriteDebugMessage(ex);
			}
		}
	}catch(Exception ex){
		cxLog.WriteDebugMessage(ex);
	}
}