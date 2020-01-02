// https://golang.org/pkg/math/rand/
List<string> mathRandPkgMethods = new List<string> {
	"ExpFloat64", "Float*", "Int*", 
	"NormFloat64", "Perm", "Read", "Uint*"
	};
result =  All.FindByMemberAccess("\"math/rand\".*").FindByShortNames(mathRandPkgMethods);