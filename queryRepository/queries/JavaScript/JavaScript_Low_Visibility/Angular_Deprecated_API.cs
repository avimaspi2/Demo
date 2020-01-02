CxList strings = Find_String_Literal();

Func<string, string, CxList> findDeprecatedImport = (deprecatedSymbol, deprecatedPackage) =>
{
	CxList foundDeprecatedSymbol = All.FindByShortName(deprecatedSymbol);
	CxList foundDeprecatedPacakge = strings.FindByShortName(deprecatedPackage);
	CxList foundDeprecatedImport = foundDeprecatedSymbol.FindByPositions(foundDeprecatedPacakge, CxList.CxPositionProximity.FindInLine, false);
	return foundDeprecatedImport;
};

Func <string, string, CxList> findUsageOfDeprecatedApis = (deprecatedSymbol, deprecatedPackage) =>
{
	CxList foundDeprecatedImport = findDeprecatedImport(deprecatedSymbol, deprecatedPackage);
	CxList foundDeprecatedSymbol = (All - foundDeprecatedImport).FindByShortName(deprecatedSymbol).FindByFiles(foundDeprecatedImport);
	CxList foundDeprecatedSymbolOnePerLine = foundDeprecatedSymbol.FindByPositions(foundDeprecatedSymbol, CxList.CxPositionProximity.FindInLine, true);
	return foundDeprecatedSymbolOnePerLine;
};


result.Add(findUsageOfDeprecatedApis("DeprecatedI18NPipesModule", "@angular/common"));
result.Add(findUsageOfDeprecatedApis("state", "@angular/core"));
result.Add(findUsageOfDeprecatedApis("style", "@angular/core"));
result.Add(findUsageOfDeprecatedApis("animate", "@angular/core"));
result.Add(findUsageOfDeprecatedApis("AnimationEntryMetadata", "@angular/core"));
result.Add(findUsageOfDeprecatedApis("AnimationPlayer", "@angular/core"));
result.Add(findUsageOfDeprecatedApis("AnimationTransitionEvent", "@angular/core"));
result.Add(findUsageOfDeprecatedApis("AUTO_STYLE", "@angular/core"));
result.Add(findUsageOfDeprecatedApis("Request", "@angular/http"));
result.Add(findUsageOfDeprecatedApis("BaseRequestOptions", "@angular/http"));
result.Add(findUsageOfDeprecatedApis("RequestMethod", "@angular/http"));
result.Add(findUsageOfDeprecatedApis("BaseResponseOptions", "@angular/http"));
result.Add(findUsageOfDeprecatedApis("Response", "@angular/http"));
result.Add(findUsageOfDeprecatedApis("BrowserXhr", "@angular/http"));
result.Add(findUsageOfDeprecatedApis("CollectionChangeRecord", "@angular/core"));
result.Add(findUsageOfDeprecatedApis("Connection", "@angular/http"));
result.Add(findUsageOfDeprecatedApis("ConnectionBackend", "@angular/http"));
result.Add(findUsageOfDeprecatedApis("CookieXSRFStrategy", "@angular/http"));
result.Add(findUsageOfDeprecatedApis("DefaultIterableDiffer", "@angular/core"));
result.Add(findUsageOfDeprecatedApis("DeprecatedCurrencyPipe", "@angular/common"));
result.Add(findUsageOfDeprecatedApis("DeprecatedDatePipe", "@angular/common"));
result.Add(findUsageOfDeprecatedApis("DeprecatedDecimalPipe", "@angular/common"));