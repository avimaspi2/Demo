CxList encode = Find_HTML_Encode() + Find_URL_Encode();

CxList methods = Find_Methods();

encode.Add(methods.FindByShortName("*encode*", false));
encode -= methods.FindByShortName("*unencode*", false);

result = encode;