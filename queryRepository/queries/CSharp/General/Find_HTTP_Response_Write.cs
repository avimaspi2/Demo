/* This query finds methods that write a response to a HTTP request */
CxList resps = Find_Response();
result = resps.GetRightmostMember().FindByShortNames(new List<string>(){"Write*","End","TransmitFile","BinaryWrite"});