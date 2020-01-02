// Query Stored_File_Manipulation
// ==============================
// In deference from File manipulation, in stored file manipulation the input from the file (Find_Readapplied)
CxList inputs =  Find_Read();
inputs.Add(Find_DB_Out());

result = Find_Relative_Path_Traversal(inputs);