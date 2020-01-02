CxList methods = Find_Methods();

//Black list of Content-Types that are known to be XSS enablers
List < string > content_type_black_list = new List<string>() {
		"*text/html*",
		"*application/xhtml+xml*",
		"*application/xml*",
		"*text/xml*",
		"*image/svg+xml*"
		};


//Get all header methods
CxList header_methods = methods.FindByShortName("header");

//Get only the first param and take only the strings (discard Param objects)
CxList fst_param_of_header_methods = All.GetParameters(header_methods, 0) * Find_Strings();
//Get the string params that contain Content-Type:... in the name
CxList content_types = fst_param_of_header_methods.FindByShortName("Content-Type*",false);
//Get the content types from the black list
CxList bad_content_types = content_types.FindByShortNames(content_type_black_list, false);
//Discard the bad content-types and take the header methods that define good content-types
CxList all_content_type_headers = header_methods.FindByParameters(content_types);
CxList good_content_header_methods = all_content_type_headers - header_methods.FindByParameters(bad_content_types);

//Get all the elements under the fathers of the good headers (to reduce search space)
CxList possible_sanitizers = All.GetByAncs(good_content_header_methods.GetFathers().GetFathers());
//reduce the sanitizers to only methods (the kind of outputs to sanitize)
possible_sanitizers *= methods;

CxList sanitizers = All.NewCxList();
//Get all the elements that appear only after the position (line) of the header
foreach(CxList header in good_content_header_methods){
	CSharpGraph header_obj = header.TryGetCSharpGraph<CSharpGraph>();
	CxList methods_after_header = possible_sanitizers.GetByAncs(header.GetFathers().GetFathers());
	foreach(CxList method_after_header in methods_after_header) {
		CSharpGraph method_after_header_obj = method_after_header.TryGetCSharpGraph<CSharpGraph>();
		if(method_after_header_obj.LinePragma.Line >= header_obj.LinePragma.Line) {
			sanitizers.Add(method_after_header);
		}
	}
}

//Also, all the headers with content-type are sanitizers
sanitizers.Add(all_content_type_headers);
result = sanitizers;