// Find all strings that contain "password"
CxList passwordString = Find_Password_Strings();
// Find all outputs to file
CxList outputs = Find_Write();

// Choose only relevant password strings - that affect output
CxList passwordStringToOutput = passwordString.DataInfluencingOn(outputs);

// Get the strings that contain "type=" and also contain ""text", 
// so that there's a good chance that the type is text
CxList typeText = 
	passwordStringToOutput.FindByShortName(@"*type=*")
	+ passwordStringToOutput.FindByShortName(@"*type =*");
typeText = typeText.FindByShortName(@"*""text*");

// Then remove "type=password", just to clean possible mistakes/FP.
result = typeText - typeText.FindByShortName(@"*type=\""password*");