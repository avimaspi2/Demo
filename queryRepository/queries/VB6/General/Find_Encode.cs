//CxList encode = Find_URL_Encode() + Find_HTML_Encode();

CxList encode = Find_Methods().FindByShortName("*encode*", false) - 
	Find_Methods().FindByShortName("*unencode*", false);
result = encode;