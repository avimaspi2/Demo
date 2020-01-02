// https://golang.org/pkg/strconv/
List<string> strconvPkgMethods = new List<string> {
	"Atoi", 
	"Is*",		// IsGraphic or IsPrint
	"Parse*"	// ParseBool, ParseFloat, ParseInt or ParseUint
	};
result = All.FindByMemberAccess("\"strconv\".*").FindByShortNames(strconvPkgMethods);