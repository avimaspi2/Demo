//	Process Control
//  ---------------
//  Find all LoadLibrary and LoadModule affected from user input and stored data
//  Find parameters of given methods that are not defined
///////////////////////////////////////////////////////////////////////

// Finds relevant Methods that are influenced by inputs
CxList relevantMethods = Find_LoadLibrary();

CxList inputs = Find_Interactive_Inputs();

CxList methodsInfluenced = relevantMethods.DataInfluencedBy(inputs);

// For the remaining Methods find those that don't have the path fully specified
CxList relevantMethodsNotFound = relevantMethods - methodsInfluenced;
CxList relevantMethodsParams = All.GetParameters(relevantMethodsNotFound); 

CxList strings = Find_Strings();

//Checks for absolute path, if C:[\/] isn't included at the begining we can assume that
//it's not an absolute path
CxList fullPathStrings = strings.FindByShortNames(new List<string>{"C:/*","C:\\*","/*"},false);
CxList vulnStrings = strings - fullPathStrings;

CxList notFullPathVulnMethods = relevantMethodsNotFound.InfluencedBy(vulnStrings);

methodsInfluenced.Add(notFullPathVulnMethods);
result = methodsInfluenced;