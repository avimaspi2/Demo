CxList methods = Find_Methods();
result = methods.FindByMemberAccess("pagereference.getheaders") + 
	methods.FindByMemberAccess("pagereference.getcontenttype") + 
	methods.FindByMemberAccess("currentpage.getheaders") + 
	methods.FindByMemberAccess("currentpage.getcontenttype") + 
	methods.FindByMemberAccess("httprequest.getheader");