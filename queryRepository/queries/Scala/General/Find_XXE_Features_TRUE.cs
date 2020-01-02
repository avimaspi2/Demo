CxList features_true = 
	All.FindByName("*http://apache.org/xml/features/disallow-doctype-decl*") +
	All.FindByName("*http://xerces.apache.org/xerces2-j/features.html#disallow-doctype-decl*") +
	All.FindByMemberAccess("XMLConstants.FEATURE_SECURE_PROCESS");

result = features_true;