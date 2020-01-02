//CxList encode = Find_URL_Encode() + Find_HTML_Encode();

CxList methods = Find_Methods();
CxList viewMethods = Find_View_Code() * methods;

CxList encode = methods.FindByShortName("*encode*", false) - 
	methods.FindByShortName("*unencode*", false);

encode.Add(methods.FindByShortName("*htmlentities*", false));
encode.Add(methods.FindByShortName("*htmlspecialchars*", false));
encode.Add(methods.FindByShortName("*filter_var*", false));
encode.Add(methods.FindByShortName("*strip_tags*", false));
encode.Add(methods.FindByShortName("fix_quotes"));

encode.Add(methods.FindByShortName("esc*") +
	methods.FindByShortName("*esc") + 
	methods.FindByShortName("*escape*") +
	methods.FindByShortName("sanitize*") +
	All.GetByAncs(viewMethods.FindByShortName("h")));

result = encode;