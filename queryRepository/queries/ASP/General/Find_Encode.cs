CxList encode = Find_URL_Encode();
encode.Add(Find_HTML_Encode());
encode.Add(Find_Methods().FindByShortName("*encode*", false));
CxList encRemove = encode.FindByShortName("*Decode*", false);
encRemove.Add(encode.FindByShortName("*UnEncode*", false));
encode -= encRemove;

result = encode;