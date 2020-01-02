if (param.Length == 1)
{
	//CVE-2013-7315 
	//Spring MVC in Spring Framework before 3.2.4 and 4.0.0.M1 doesn't disable External Entity Resolution
	//also when using JAXB in combination with a StAX XMLInputFactory

	//CVE-2013-6429
	//Spring MVC in Spring Framework before 3.2.5 and 4.0.0.M1 through 4.0.0.RC1 does not disable
	//external entity resolution when using SourceHttpMessageConverter
	string versionName = param[0] as string;

	string[] classesTypes = new string[]{"RestTemplate","RequestMappingHandlerAdapter"};
	CxList types = All.FindByTypes(classesTypes);
	types.Add(Find_Collections_Of_Type("HttpMessageConverter"));
	CxList getMessageConverters = types.GetMembersOfTarget();

	CxList convertersObjects = getMessageConverters.GetMembersOfTarget().FindByShortName("getMessageConverters");
	convertersObjects.Add(All.FindAllReferences(getMessageConverters.GetFathers()).GetMembersOfTarget());
	convertersObjects.Add(getMessageConverters);
	CxList messageConverters = convertersObjects.FindByShortName("add");
		
	if(versionName.CompareTo("3.2.5") < 0 || ( versionName.CompareTo("4.0.0.M1") >= 0 && versionName.CompareTo("4.0.0.RC1") <= 0)){
		CxList sourceHttpMessageConverter = All.FindByType("SourceHttpMessageConverter");
		result.Add(messageConverters.FindByParameters(sourceHttpMessageConverter));
	}
	
	//CVE-2014-0054
	//Spring MVC in Spring Framework before 3.2.8 and 4.0.0 before 4.0.2 does not disable external entity resolution
	//when using Jaxb2RootElementHttpMessageConverter
	if(versionName.CompareTo("3.2.8") < 0 || ( versionName.CompareTo("4") > 0 && versionName.CompareTo("4.0.2.RELEASE") < 0)){
		CxList jaxbHttpMessageConverter = All.FindByType("Jaxb2RootElementHttpMessageConverter");
		result.Add(messageConverters.FindByParameters(jaxbHttpMessageConverter));
	}
}