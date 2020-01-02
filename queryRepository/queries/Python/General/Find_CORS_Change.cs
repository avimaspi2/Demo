CxList flasks = All.FindAllReferences(All.FindByTypes(new string[]
	{"Flask","Blueprint"}).GetAssignee());
CxList methods = Find_Methods();
CxList corsMethods = methods.FindByShortName("CORS").FindByParameters(flasks);

CxList strings = Find_Strings();
CxList origins = strings.FindByShortName("origins");
CxList wildcard = strings.FindByShortName(@"""*""");

CxList allWildCardOrigins = All.NewCxList();
allWildCardOrigins.Add(wildcard);
allWildCardOrigins.Add(origins);

CxList possiblesanitizer = All.NewCxList();
possiblesanitizer.Add(strings);
possiblesanitizer -= allWildCardOrigins;

CxList originsFather = origins.GetAncOfType(typeof(ArrayInitializer));
CxList possiblesanitizerFather = possiblesanitizer.GetAncOfType(typeof(ArrayInitializer));

CxList sanitizedOrigin = originsFather * possiblesanitizerFather;
CxList sanitizedCorsMethod = corsMethods.DataInfluencedBy(origins.FindByFathers(sanitizedOrigin));

CxList corsVuln = corsMethods - sanitizedCorsMethod;

CxList decoratorCrossOrigin = methods.FindByShortName("cross_origin");
CxList originChangeMethods = decoratorCrossOrigin.FindByParameterName("origin");

CxList sanitizedCrossOrigin = originChangeMethods.FindByParameters(possiblesanitizer);

CxList crossOriginVuln = decoratorCrossOrigin - sanitizedCrossOrigin;

result = corsVuln;
result.Add(crossOriginVuln);