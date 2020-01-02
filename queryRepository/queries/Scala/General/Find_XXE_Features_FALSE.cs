CxList features_false = 
	All.FindByName("*http://xml.org/sax/features/external-parameter-entities*") +
	All.FindByName("*http://xerces.apache.org/xerces-j/features.html#external-parameter-entities*") +
	All.FindByName("*http://xerces.apache.org/xerces2-j/features.html#external-parameter-entities*") +
	All.FindByName("*http://xml.org/sax/features/external-general-entities*") +
	All.FindByName("*http://xerces.apache.org/xerces-j/features.html#external-general-entities*") +
	All.FindByName("*http://xerces.apache.org/xerces2-j/features.html#external-general-entities*") +
	All.FindByMemberAccess("XMLConstants.ACCESS_EXTERNAL_DTD") +
	//These are for StAX...
	All.FindByName("*javax.xml.stream.isSupportingExternalEntities*") +
	All.FindByName("*javax.xml.stream.supportDTD*") +
	All.FindByMemberAccess("XMLInputFactory.IS_SUPPORTING_EXTERNAL_ENTITIES") +
	All.FindByMemberAccess("XMLInputFactory.SUPPORT_DTD");

result = features_false;