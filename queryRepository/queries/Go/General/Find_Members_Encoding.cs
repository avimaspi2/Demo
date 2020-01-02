// https://golang.org/pkg/encoding/json/
List<string> encodingJsonSanitizers = new List<string> { "HTMLEscape", "NewEncoder" };
result = All.FindByMemberAccess("\"encoding/json\".*").FindByShortNames(encodingJsonSanitizers);