CxList methods = Find_Methods();

List < string > paramsListStrings = new List<string> {
	"bzread",  
	"dio_read",  
	"disk_total_space", 
	"eio_readdir",  
	"file",  
	"file_get_contents",  
	"fgetc",  
	"fgetcsv",  
	"fgets",  
	"fgetss", 
	"fread",  
	"fscanf", 
	"get_meta_tags",  
	"gzfile",  
	"gzgetc",  
	"gzgets",  
	"gzgetss",  
	"gzread",  
	"gzpassthru", 
	"highlight_file", 
	"parse_ini_file", 
	"php_strip_whitespace", 
	"readfile",
	"scandir", 
	"show_source",  
	"simplexml_load_file",  
	"stream_get_contents",  
	"stream_get_line",  
	"yaml_parse_file",  
	"fpassthru"};			
CxList firstParamMethods = methods.FindByShortNames(paramsListStrings);
CxList taintedParams = 
	All.GetParameters(firstParamMethods, 0);
		
result = taintedParams;