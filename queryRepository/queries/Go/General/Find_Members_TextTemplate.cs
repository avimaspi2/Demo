List<string> templateSanitizers = new List<string> {
	"HTMLEscape*", 		// HTMLEscape, HTMLEscaper or HTMLEscapeString
	"JSEscape*", 		// JSEscape, JSEscaper or JSEscapeString
	"URLQueryEscaper"};

// https://golang.org/pkg/text/template/
result = All.FindByMemberAccess("\"text/template\".*").FindByShortNames(templateSanitizers);