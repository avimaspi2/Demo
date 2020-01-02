// database
CxList db = Find_DB_In();
// strings
CxList strings = Find_Strings();
// strings that end with "id" (there might be also "pid" and others, but then I'm starting with many
// potential false positives
CxList id = strings.FindByShortName("*id\"");
// Interative inputs, that are influenced by this id (usually getParameter or alike)
CxList input = Find_Interactive_Inputs();
input = input.DataInfluencedBy(id);


/// DB influenced by potentially problematic input
result = db.DataInfluencedBy(input).DataInfluencedBy(id);