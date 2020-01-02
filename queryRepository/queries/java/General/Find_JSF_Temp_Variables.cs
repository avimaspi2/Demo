if(cxScan.IsFrameworkActive("JSF"))
{
	CxList JsfWrapper = All.FindByShortName("Checkmarx_Class_Init_JSF").FindByType(typeof(MethodDecl));
	CxList JsfWrapperParams = All.GetParameters(JsfWrapper);
	
	result = JsfWrapperParams;
}