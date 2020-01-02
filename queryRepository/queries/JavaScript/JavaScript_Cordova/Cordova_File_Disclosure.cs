/// <summary>
/// File Reads Influenced by non Sanitized Inputs
/// </summary>
CxList reads = PhoneGap_File_Read();
CxList inputs = Find_Inputs();
CxList sanitize = basic_Sanitize();

result = reads.InfluencedByAndNotSanitized(inputs, sanitize);