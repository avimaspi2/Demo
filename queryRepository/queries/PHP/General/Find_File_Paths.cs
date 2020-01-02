CxList methods = Find_Methods();

// Functions whose 1st parameter is filename / path

List < string > paramsListStrings = new List<string> {"basename", "chgrp", "chmod", "chown","copy", "dirname", "disk_free_space", 
		"disk_total_space",
		"diskfreespace",
		"file_exists", 
		"file_get_contents", 
		"file_put_contents",  
		"file", 
		"fileatime", 
		"filectime", 
		"filegroup", 
		"fileinode", 
		"filemtime", 
		"fileowner", 
		"fileperms", 
		"filesize", 
		"filetype", 	
		"fopen", 
		"is_dir", 
		"is_executable", 
		"is_file", 
		"is_link", 
		"is_readable", 
		"is_uploaded_file", 
		"is_writable", 
		"is_writeable", 
		"lchgrp", 
		"lchown", 
		"link", 	
		"linkinfo", 
		"lstat", 
		"mkdir", 
		"move_uploaded_file", 
		"parse_ini_file", 	
		"pathinfo", 	
		"readfile", 
		"readlink", 	
		"realpath", 
		"rename", 	
		"rmdir", 
		"stat", 
		"symlink", 	
		"tempnam", 
		"touch", 
		"unlink", 
		"gzfile", 
		"gzopen", 
		"readgzfile",
		"shell_exec"};
	
CxList fileAccess0 = methods.FindByShortNames(paramsListStrings);

// Functions whose 2nd parameter is filename / path
List < string > fileAccessListStrings = new List<string> {"clearstatcache", "copy", "link", 
		"move_uploaded_file", "rename", "symlink"};

CxList fileAccess1 = methods.FindByShortNames(fileAccessListStrings);
	
result = All.GetParameters(fileAccess0, 0) + All.GetParameters(fileAccess1, 1);