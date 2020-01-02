List<string> templateSanitizers = new List<string> {
	"HTMLEscape*", 		// HTMLEscape, HTMLEscaper or HTMLEscapeString
	"JSEscape*", 		// JSEscape, JSEscaper or JSEscapeString
	"URLQueryEscaper"};

// Find_HtmlTemplate_Sanitizers
// https://golang.org/pkg/html/template/
result = All.FindByMemberAccess("\"html/template\".*").FindByShortNames(templateSanitizers);