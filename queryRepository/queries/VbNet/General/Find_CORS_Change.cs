CxList strings = All.FindByType(typeof(StringLiteral)); 
IAbstractValue absValue1 = new StringAbstractValue("access-control-allow-origin:");

CxList allowOrigins = strings.FindByAbstractValue(abstractValue => absValue1.IncludedIn(abstractValue));
CxList anyAbsValue = allowOrigins.FindByAbstractValue(_ => _ is AnyAbstractValue);

CxList allowOriginsFiltered = allowOrigins - anyAbsValue;
allowOriginsFiltered.Add(strings.FindByShortName("access-control-allow-origin", false));
CxList allowOriginWildcard = All.NewCxList();

if (allowOriginsFiltered.Count > 0){
	IAbstractValue absValue2 = new StringAbstractValue("*");
	CxList wildcard = strings.FindByAbstractValue(abstractValue => absValue2.IncludedIn(abstractValue));
	wildcard.Add(strings.FindByRegex("\"\\*\""));

	CxList allowOriginsFather = allowOriginsFiltered.GetAncOfType(typeof(BinaryExpr));
	allowOriginWildcard = wildcard.GetByAncs(allowOriginsFather);	

}
result = allowOriginWildcard;