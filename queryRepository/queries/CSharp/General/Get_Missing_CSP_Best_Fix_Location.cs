result = base.Get_Missing_CSP_Best_Fix_Location();

CxList webConfig = Find_Web_Config().FindByFileName("*web.config");
CxList webConfigClass = webConfig.FindByName("CxXmlConfigClass*", true).FindByType(typeof(ClassDecl));

if (webConfigClass.Count != 0)
{
	CSharpGraph webClass = webConfigClass.GetFirstGraph();
	result = All.FindById(webClass.NodeId);
}