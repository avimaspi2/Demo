CxList unknownRefs = Find_UnknownReference();
CxList overrides = Find_Methods_By_Import("django.test.utils", new string[]{"override_settings"},null,unknownRefs);
CxList relevantOverrides = overrides.FindByParameterName("CORS_ORIGIN_ALLOW_ALL");

CxList parameters = All.GetParameters(relevantOverrides);
CxList trueAbsValueHeader = parameters.FindByAbstractValue(_ => _ is TrueAbstractValue);

CxList vulnOverrides = relevantOverrides.FindByParameters(trueAbsValueHeader);


CxList methods = Find_Methods().FindByShortName("getattr");

parameters = All.GetParameters(methods);

IAbstractValue absValue = new StringAbstractValue("'CORS_ORIGIN_ALLOW_ALL'"); 
CxList allowOriginHeader = parameters.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(absValue));

trueAbsValueHeader = parameters.FindByAbstractValue(_ => _ is TrueAbstractValue);

CxList settings = Find_Methods_By_Import("django.conf", new string[]{"settings"},null,unknownRefs);

CxList vulnMethods = methods.FindByParameters(trueAbsValueHeader).FindByParameters(allowOriginHeader).FindByParameters(settings);

result = vulnOverrides;
result.Add(vulnMethods);