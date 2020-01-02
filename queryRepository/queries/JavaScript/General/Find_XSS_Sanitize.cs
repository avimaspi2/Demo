CxList methods = Find_Methods();
result.Add(methods.FindByShortName("CxDefaultSanitizer"));

CxList allSetterSanitizers = Find_Safe_Setters();
CxList eventSetters = Find_Vulnerable_SetAttribute();
CxList tempRes = (allSetterSanitizers - allSetterSanitizers.DataInfluencingOn(eventSetters));
CxList sa = tempRes.FindByShortName("setAttribute");
tempRes -= sa;
tempRes.Add((All - Find_Param()).GetParameters(sa, 1));
result.Add(tempRes);

result.Add(Find_Handlebars_Sanitize());

CxList toRemove = Find_Inputs();
toRemove.Add(Find_Outputs_XSS());//to remove setAttributes such as innerHTML document.getElementById('a').innerHTML = z;

result.Add(Find_SAPUI_XSS_Sanitize());
result -= toRemove;