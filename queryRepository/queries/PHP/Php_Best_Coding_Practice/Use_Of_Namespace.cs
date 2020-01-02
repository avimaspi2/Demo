CxList namespaces = All.FindByType(typeof(NamespaceDecl));
CxList globalNamespaces = namespaces.FindByShortName("$NS_*");
result = namespaces - globalNamespaces;