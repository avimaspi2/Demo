/* targetStrings variable contains keywords that will let
  identify if there is any HSTS configuration made with XML */
CxList targetStrings = All.FindByShortNames(
	new List<string>(){"*Strict-Transport-Security*","hsts"}, false);
result = targetStrings.FindByFileName("*.config");