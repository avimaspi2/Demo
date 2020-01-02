CxList methods = Find_Methods();
CxList inputs = Find_Inputs();
CxList outputs = Find_OGNL_Outputs();

CxList sanitizers = Find_General_Sanitize();

//Base64 encoding
CxList encodeBase64 = methods.FindByMemberAccess("Base64.encode*", true);
encodeBase64.Add(methods.FindByName("Base64.Encoder.encode*", true));
encodeBase64.Add(methods.FindByName("Base64.getEncoder.encode*", true));
sanitizers.Add(encodeBase64);

//Hexadecimal encode
CxList encodeHex = All.FindByMemberAccess("Hex.encode*", true);
sanitizers.Add(encodeHex);

// Exclude getAtribute Parameter
CxList getAttribute = methods.FindByName("request.getAttribute");
getAttribute.Add(methods.FindByMemberAccess("HttpServletRequest.getAttribute"));
sanitizers -= All.GetParameters(getAttribute);

result.Add(inputs.InfluencingOnAndNotSanitized(outputs, sanitizers));