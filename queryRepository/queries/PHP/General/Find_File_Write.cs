CxList methods = Find_Methods();

List < string > paramsListStrings = new List<string> {
	"chmod", 
	"chgrp", 
	"chown", 
	"copy", 
	"disk_free_space", 
	"disk_total_space", 
	"eio_chmod", 
	"eio_chown", 
	"eio_mkdir", 
	"eio_mknod", 
	"eio_rmdir", 
	"eio_unlink", 
	"fclose", 
	"feof", 
	"fflush", 
	"flock", 
	"ftell", 
	"ftruncate", 
	"loadXML", 
	"mkdir", 
	"pclose", 
	"popen", 
	"posix_mknod", 
	"rmdir", 
	"umask", 
	"touch", 
	"unlink", 
	"stream_set_write_buffer", 
	"set_file_buffer", 
	"symlink", 
	"tempman", 
	"rename", 
	"move_uploaded_file", 
	"fwrite", 
	"gzwrite", 
	"gzputs", 
	"lchgrp", 
	"lchown", 
	"link", 
	"fseek", 
	"fputcsv", 
	"fputs", 
	"file_put_contents", 
	"dio_write", 
	"eio_write", 
	"yaml_emit_file"};

CxList firstParamMethods = methods.FindByShortNames(paramsListStrings);

List < string > secondParamsListStrings = new List<string> {
"stream_set_write_buffer", 
"set_file_buffer", 
"symlink", 
"tempman", 
"rename", 
"move_uploaded_file", 
"fwrite", 
"gzwrite", 
"gzputs", 
"lchgrp", 
"lchown", 
"link", 
"fseek", 
"fputcsv", 
"fputs", 
"file_put_contents", 
"dio_write", 
"eio_write", 
"yaml_emit_file", 
"bzwrite", 
"clearstatcache", 
"event_buffer_write", 
"shmop_write", 
"recode_file"};

CxList secondParamMethods = methods.FindByShortNames(secondParamsListStrings);

List < string > thirdParamsListStrings = new List<string>(new string[] {
	"recode_file", 
	"error_log", 
	"xdiff_file_bdiff", 
	"xdiff_file_bpatch", 
	"xdiff_file_diff_binary", 
	"xdiff_file_diff", 
	"xdiff_file_patch_binary", 
	"xdiff_file_patch", 
	"xdiff_file_rabdiff"});

CxList thirdParamMethods =
	methods.FindByShortNames(thirdParamsListStrings);

CxList fourthParamMethods = methods.FindByShortName("xdiff_file_merge");

CxList allParamMethods =
	methods.FindByShortName("fprintf") +
	methods.FindByShortName("vfprintf");

CxList taintedParams = 
	All.GetParameters(firstParamMethods, 0) +
	All.GetParameters(secondParamMethods, 1) +
	All.GetParameters(thirdParamMethods, 2) +
	All.GetParameters(fourthParamMethods, 3) +
	All.GetParameters(allParamMethods);
		
result = taintedParams;