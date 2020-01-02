if (param.Length == 1)
{
	string versionName = param[0] as string;
	if(!string.IsNullOrEmpty(versionName))
	{
		//CVE-2013-4152
		//Spring OXM wrapper in Spring Framework before 3.2.4 and 4.0.0.M1 doesn't disable External Entity Resolution
		//when using JAXB unmarshaller with StreamSource or SAXSource
		if(versionName.CompareTo("3.2.4") < 0 || ( versionName.CompareTo("4") > 0 && versionName.CompareTo("4.0.0.M1") < 0)){
			string[] jaxbTypes = new string[]{"Jaxb1Marshaller","Jaxb2Marshaller"};
			CxList jaxbUnmarshaller = All.FindByTypes(jaxbTypes);
			CxList jaxbMethods = jaxbUnmarshaller.GetMembersOfTarget().FindByShortName("unmarshal");

			string[] vulnerableTypes = new string[]{"SAXSource","StreamSource"};
			CxList vulnerableData = All.FindByTypes(vulnerableTypes);

			result = jaxbMethods.FindByParameters(vulnerableData);
		}
	}
}