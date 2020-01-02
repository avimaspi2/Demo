CxList featuresTrue = All.FindByName("*http://apache.org/xml/features/disallow-doctype-decl*");
featuresTrue.Add(All.FindByName("*http://xerces.apache.org/xerces2-j/features.html#disallow-doctype-decl*"));
featuresTrue.Add(All.FindByMemberAccess("XMLConstants.FEATURE_SECURE_PROCESS"));



result = featuresTrue;