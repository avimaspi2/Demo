//	Cleartext_Transmission_Of_Sensitive_Information
//  -----------------------
//  In this query we look for a transmission of sensitive information
//  that is not protected by SSL. 
//  People should use SSL framework like OpenSSL (uses SSL_write() method). 
///////////////////////////////////////////////////////////////////////

CxList personalInfo = Find_Personal_Info() + Find_All_Passwords();

CxList sendToSocket = Find_Unprotected_Socket_Usage();

result = personalInfo.DataInfluencingOn(sendToSocket).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);