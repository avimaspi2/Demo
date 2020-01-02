// Insecure Data Storage
// -----------------------
// This risk addresses the obvious concern of sensitive data being stored on mobile devices
// The purpose of the query is to detect any attempt to write sensitive information to the device
// when it is not encrypted.

result = Find_Insecure_Data_Storage(Find_Personal_Info());
result.Add(Find_Insecure_Data_Storage(Find_Cryptographic_Keys()).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow));