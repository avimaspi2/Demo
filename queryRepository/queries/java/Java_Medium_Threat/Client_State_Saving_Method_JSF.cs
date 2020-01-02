/****

The query looks for the following information:

<context-param>
    <param-name>javax.faces.STATE_SAVING_METHOD</param-name>
    <param-value>client</param-value>
</context-param>

***/
if(All.isWebApplication)
{
	CxList webConfig = Find_Web_Xml();
	CxList strings = webConfig.FindByType(typeof(StringLiteral));
	
	// Find all conditions in a config file
	CxList conditions = webConfig * Find_Conditions();
	
	// Find the "javax.faces.STATE_SAVING_METHOD" string
	CxList saving_method = strings.FindByName("javax.faces.STATE_SAVING_METHOD");
	
	// Find the "client" string
	CxList client_value = strings.FindByName("client");
	
	CxList if_saving_method = saving_method.GetAncOfType(typeof(IfStmt)).GetFathers().GetAncOfType(typeof(IfStmt));
	
	result = client_value.GetByAncs(if_saving_method);
	
}