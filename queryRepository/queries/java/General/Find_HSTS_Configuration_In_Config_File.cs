/* targetStrings variable contains keywords that will let
  identify if there is any HSTS configuration made with XML */
CxList targetStrings = All.FindByShortNames(
	new List<string>(){"*httpHeaderSecurity*",
		"*HstsFilter*"}, false);
result = targetStrings.FindByFileName("*.xml");