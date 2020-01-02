// Find Content-Security-Policy meta tag
string regex = @"<meta\s+(http-equiv=""Content-Security-Policy"")[^>]*>";
result = All.FindByRegexExt(regex, new List<string>{"*.html", "*.jsp", "*.cshtml"});