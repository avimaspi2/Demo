CxList encode = Find_HTML_Encode() + Find_URL_Encode();

encode.Add(Find_Methods().FindByShortName("*encode*", false));
encode -= Find_Methods().FindByShortName("*unencode*", false);

result = encode;