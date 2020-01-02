// find all inputs
CxList inputs = Find_Inputs();

// Get all the relevant digest algorithms
CxList digestStrings = Find_Hash_Strings();  

// All the update commands
CxList update = Find_Update_Commands();

// The result - any update of the relevant algorithms, influenced by any input
result = update.DataInfluencedBy(digestStrings).DataInfluencedBy(inputs);