CxList inputs = Find_Inputs();
CxList evaluateXPATH = Find_Members("document.evaluate");
CxList sanitizer = Sanitize();
result = evaluateXPATH.InfluencedByAndNotSanitized(inputs, sanitizer);