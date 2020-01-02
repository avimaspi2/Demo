/* This query finds PhoneGap-specific outputs. */

// Auxiliary information
CxList methods = Find_Methods();

// FileTransfer.upload
result.Add(PhoneGap_File_Upload());

// Popup alert Outputs
result.Add(methods.FindByShortName("alert"));