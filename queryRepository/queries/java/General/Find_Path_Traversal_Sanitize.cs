/*
	This query finds replacing mechanisms for "\" and "/" simultaneously.
	Ex:
		someString = someString.replace("/","").replace("\\","");
		someString = someString.replace(File.separator, "");
*/

CxList methods = Find_Methods();
CxList strings = Find_Strings();

/* 	
	Both File.separator and System.getProperty("file.separator") 
	return the adequate path separator for the supporting OS.
*/
CxList fileSeparators = All.FindByMemberAccess("File.separator");
CxList getPropertyMethods = methods.FindByMemberAccess("System.getProperty");
fileSeparators.Add(getPropertyMethods.FindByParameters(strings.FindByShortName("file.separator")));

/*	
	String.replace() and String.replaceAll  
*/
CxList replaceMethods = methods.FindByShortName("replace*", false);

/*  
	The first parameter indicates the old value of what should be replaced in the string  
*/
CxList replaceFirstParameter = All.GetParameters(replaceMethods, 0);
replaceFirstParameter -= replaceFirstParameter.FindByType(typeof(Param));

CxList pathTraversalSanitizers = All.NewCxList();
pathTraversalSanitizers.Add(replaceMethods.DataInfluencedBy(fileSeparators));

/*  
	Both OS should be considered for a full sanitization, so a distinction is in order  
*/
CxList slashReplacement = All.NewCxList();
CxList backSlashReplacement = All.NewCxList();
CxList regexOrSubstringsOfReplace = strings.DataInfluencingOn(replaceFirstParameter);
regexOrSubstringsOfReplace.Add(strings * replaceFirstParameter);
foreach(CxList regexOrSubstring in regexOrSubstringsOfReplace){
	if(regexOrSubstring.GetName().Contains("/"))slashReplacement.Add(regexOrSubstring);
	else if(regexOrSubstring.GetName().Contains("\\"))backSlashReplacement.Add(regexOrSubstring);
}
/*
	Ensure both slashes are removed from inputs.
*/
CxList slashReplacementMethods = replaceMethods.DataInfluencedBy(slashReplacement);
slashReplacementMethods.Add(replaceMethods * slashReplacement);
CxList backSlashReplacementMethods = replaceMethods.DataInfluencedBy(backSlashReplacement);
backSlashReplacementMethods.Add(replaceMethods * backSlashReplacement);
	
pathTraversalSanitizers.Add(slashReplacementMethods.DataInfluencedBy(backSlashReplacementMethods));
pathTraversalSanitizers.Add(backSlashReplacementMethods.DataInfluencedBy(slashReplacementMethods));

/*
	Methods from java.nio.file.Files with Path typed parameters handle files. This stub adds
	non Path parameters to the sanitizers.
*/
CxList filesMethods = All.NewCxList();
CxList filesOpen = Find_Files_Open();
filesMethods = Find_Java7_Files_Open();
filesMethods.Add(filesOpen.FindByMemberAccess("Files.*"));

CxList filesMethodsParams = All.GetParameters(filesMethods) - methods;
CxList filesMethodsPathParams = filesMethodsParams.FindByTypes(new String[]{
	"Path","file.Path","nio.file.Path","java.nio.file.Path"});
filesMethodsPathParams.Add(filesMethodsParams.FindByType(typeof(Param)));
pathTraversalSanitizers.Add(filesMethodsParams - filesMethodsPathParams);
pathTraversalSanitizers.Add(All.FindByFathers(pathTraversalSanitizers.FindByType(typeof(IndexerRef))));
pathTraversalSanitizers.Add(Find_General_Sanitize());
pathTraversalSanitizers.Add(All.FindByMemberAccess("Path.getFileName"));

/*
	Files.write second argument is the content to be written and not the path, hence it can be considered a sanitizer.
*/
CxList filesWriteParams = filesMethodsParams.GetParameters(filesMethods.FindByShortName("write"));
CxList filesWriteFirstParam = filesWriteParams.GetParameters(filesMethods.FindByShortName("write"), 0);
pathTraversalSanitizers.Add(filesWriteParams - filesWriteFirstParam);

result = pathTraversalSanitizers - filesOpen;