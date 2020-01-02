CxList methods = Find_Methods();

CxList html = methods.FindByShortName("*htmlentities*", false);
html.Add(methods.FindByShortName("*htmlspecialchars*", false));
result = html;