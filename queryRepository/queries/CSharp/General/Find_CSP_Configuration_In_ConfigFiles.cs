CxList customHeaders = Find_Web_Config().FindByName("CONFIGURATION.SYSTEM.WEBSERVER.HTTPPROTOCOL.CUSTOMHEADERS*");
CxList cspString = Find_Strings().FindByName("Content-Security-Policy", true);
result = cspString.FindByAssignmentSide(CxList.AssignmentSide.Right) * customHeaders.GetAssigner();