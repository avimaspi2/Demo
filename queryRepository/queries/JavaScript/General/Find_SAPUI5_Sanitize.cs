//jQuery.sap.encodeCSS validates the given string for a CSS context
//jQuery.sap.encodeHTML validates the given string for HTML contexts
//jQuery.sap.encodeJS validates the given string for a Typescript contexts
//jQuery.sap.encodeURL validates the given string for a URL context
//jQuery.sap.encodeURLParameters Encode a map of parameters into a combined URL parameter string
//jQuery.sap.encodeXML validates the given string for XML contexts
CxList sapLibrary = Find_SAP_Library();
CxList sapMembers = sapLibrary.GetMembersOfTarget();
result = sapMembers.FindByShortNames(new List<string> {"escapeHTML", "encodeHTML", "encodeXML", "encodeCSS",
		"encodeJS", "encodeURL", "encodeURLParameters", });