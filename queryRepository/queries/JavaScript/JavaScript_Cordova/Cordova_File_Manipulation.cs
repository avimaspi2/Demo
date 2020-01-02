/// <summary>
/// File Writes Influenced by non Sanitized Inputs
/// </summary>
CxList write = PhoneGap_File_Write();
CxList inputs = Find_Inputs();
CxList sanitize = basic_Sanitize();

result = write.InfluencedByAndNotSanitized(inputs, sanitize);