// Find NSPredicate which are impacted directly by input
CxList methods = Find_Methods();
CxList inputs = Find_Inputs();

CxList predicateWithFormat = methods.FindByMemberAccess("NSPredicate.predicateWithFormat:*");

string[] nspredicateargs = {"NSPredicate:argumentArray*", "NSPredicate:arguments*"};
CxList swift_nspredicate = methods.FindByShortNames(new List<string>(nspredicateargs));

// Swift
swift_nspredicate.Add(methods.FindByShortName("NSPredicate*").FindByParameterName("format", 0));
swift_nspredicate.Add(methods.FindByShortName("NSString:encoding:error:").FindByParameterName("contentsOfURL", 0));


CxList nSPredicate = methods.FindByMemberAccess("NSPredicate.*");

List<string> nSPredicateMethodsNames = new List<string> {"argumentArray:","arguments:"};

CxList nSPredicateMethods = nSPredicate.FindByShortNames(nSPredicateMethodsNames);
swift_nspredicate.Add(nSPredicateMethods.FindByParameterName("format", 0));


predicateWithFormat.Add(swift_nspredicate);
CxList potenialVulnerableParameters = All.GetParameters(predicateWithFormat, 0);
CxList vulnerableParameters = potenialVulnerableParameters.DataInfluencedBy(inputs);
result = vulnerableParameters;