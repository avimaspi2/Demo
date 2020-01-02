CxList featuresFalse = All.FindByName("*http://xml.org/sax/features/external-parameter-entities*");
featuresFalse.Add(All.FindByName("*http://xerces.apache.org/xerces-j/features.html#external-parameter-entities*"));
featuresFalse.Add(All.FindByName("*http://xerces.apache.org/xerces2-j/features.html#external-parameter-entities*"));
featuresFalse.Add(All.FindByName("*http://xml.org/sax/features/external-general-entities*"));
featuresFalse.Add(All.FindByName("*http://xerces.apache.org/xerces-j/features.html#external-general-entities*"));
featuresFalse.Add(All.FindByName("*http://xerces.apache.org/xerces2-j/features.html#external-general-entities*"));
featuresFalse.Add(All.FindByMemberAccess("XMLConstants.ACCESS_EXTERNAL_DTD"));
	//These are for StAX...
featuresFalse.Add(All.FindByName("*javax.xml.stream.isSupportingExternalEntities*"));
featuresFalse.Add(All.FindByName("*javax.xml.stream.supportDTD*"));
featuresFalse.Add(All.FindByMemberAccess("XMLInputFactory.IS_SUPPORTING_EXTERNAL_ENTITIES"));
featuresFalse.Add(All.FindByMemberAccess("XMLInputFactory.SUPPORT_DTD"));


result = featuresFalse;