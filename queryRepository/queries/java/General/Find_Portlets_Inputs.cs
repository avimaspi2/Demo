CxList methods = Find_Methods();
CxList PortletRequest = methods.FindByMemberAccess("PortletRequest.*");

result.Add(PortletRequest.FindByShortName("getParameter*"));
result.Add(PortletRequest.FindByShortName("getProperty*"));
result.Add(PortletRequest.FindByShortName("getPrivateParameterMap"));
result.Add(PortletRequest.FindByShortName("getPublicParameterMap"));
result.Add(PortletRequest.FindByShortName("getPortlet*"));
result.Add(PortletRequest.FindByShortName("getCookies"));

result.Add(methods.FindByMemberAccess("PortletConfig.getInitParameter*"));
result.Add(methods.FindByMemberAccess("PortletConfigState.getParameter"));