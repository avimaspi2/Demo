// This query finds JSON strings returned to the server output
// where there's no prefix or mitigation to avoid JS interpretation
CxList methods = Find_Methods();
CxList outputs = NodeJS_Find_Interactive_Outputs();
CxList objs = Find_ObjectCreations();

// Arrays are not safe
CxList unsafeObjs = objs.FindByShortName("Array");
objs -= unsafeObjs;

// to Json converters
CxList stringify = methods.FindByName("*JSON.stringify");
stringify.Add(methods.FindByShortName("getJSON"));
stringify -= stringify.InfluencedByAndNotSanitized(objs,unsafeObjs);
stringify.Add(stringify.GetFathers().FindByType(typeof(Param)));

// consider prefix/suffixed JSON as sanitized
CxList sanitize = Find_Binarys().FindByShortName("+");

result = stringify.InfluencingOn(outputs);
result.Add((stringify * outputs) - sanitize);