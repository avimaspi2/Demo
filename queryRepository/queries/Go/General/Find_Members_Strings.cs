List<string> stringsPkgMethods = new List<string> {
	"Compare", "Contains*", "Count", "EqualFold", "Has*", "Index*", "LastIndex*"
	};
result = All.FindByMemberAccess("\"strings\".*").FindByShortNames(stringsPkgMethods);