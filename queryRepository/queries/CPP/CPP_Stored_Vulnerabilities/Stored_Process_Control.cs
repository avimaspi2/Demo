//	Stored Process Control
//  ---------------
//  Find all LoadLibrary and LoadModule affected from stored data
//  
//	///////////////////////////////////////////////////////////////////////

// Finds relevant Methods that are influenced by stored data
CxList relevantMethods = Find_LoadLibrary();

CxList inputs = Find_Read();
inputs.Add(Find_DB());

result = relevantMethods.DataInfluencedBy(inputs);