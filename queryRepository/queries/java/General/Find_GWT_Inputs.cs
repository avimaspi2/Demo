CxList methods = Find_Methods();

CxList inputs = All.NewCxList();
	// static functions
inputs.Add(methods.FindByName("GWT.getHostPageBaseURL"));
inputs.Add(methods.FindByName("GWT.getModuleBaseURL"));
inputs.Add(methods.FindByName("GWT.getModuleBaseForStaticFiles"));
inputs.Add(methods.FindByName("Window.Location.getQueryString"));
inputs.Add(methods.FindByName("Window.Location.getProtocol"));
inputs.Add(methods.FindByName("Window.Location.getHost"));
inputs.Add(methods.FindByName("Window.Location.getHostName"));
inputs.Add(methods.FindByName("Window.Location.getParameter"));
inputs.Add(methods.FindByName("Window.Location.getParameterMap"));
inputs.Add(methods.FindByName("Window.Location.getPath"));
	// not static functions
/*	
	methods.FindByMemberAccess("MediaElement.getCurrentSrc") +
	methods.FindByMemberAccess("MediaBase.getCurrentSrc") +
	methods.FindByMemberAccess("DefaultRequestTransport.getRequestUrl") +
	methods.FindByMemberAccess("DataResource.getSafeUri") +
	methods.FindByMemberAccess("ImageResource.getSafeUri") +
	methods.FindByMemberAccess("ServiceDefTarget.getServiceEntryPoint") +
	methods.FindByMemberAccess("SymbolData.getSourceUri") +
	methods.FindByMemberAccess("MediaElement.getSrc") +
	methods.FindByMemberAccess("IFrameElement.getSrc") +
	methods.FindByMemberAccess("FrameElement.getSrc") +
	methods.FindByMemberAccess("ImageElement.getSrc") +
	methods.FindByMemberAccess("ScriptElement.getSrc") +
	methods.FindByMemberAccess("SourceElement.getSrc") +
	methods.FindByMemberAccess("MediaBase.getSrc") +
	methods.FindByMemberAccess("ObjectElement.getData") +
	methods.FindByMemberAccess("ServiceDefTarget.getServiceEntryPoint") +
*/	
inputs.Add(methods.FindByMemberAccess("HttpServletRequest.getPathInfo"));
inputs.Add(methods.FindByMemberAccess("HttpServletRequest.getServletPath"));
inputs.Add(methods.FindByMemberAccess("HttpServletRequest.getRequestURI"));
inputs.Add(methods.FindByMemberAccess("HttpServletRequest.getQueryString"));
inputs.Add(methods.FindByShortName("getHref"));
inputs.Add(methods.FindByShortName("getUrl"));

result = inputs;