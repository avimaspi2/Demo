// Finds the Encodes which are razor auto-encodes
CxList htmlEncodes = Find_Encode();

CxList razorEncodings = htmlEncodes.FindByFileName("*.cshtml"); 
CxList doubleEncodes = razorEncodings.GetFathers().GetByAncs(razorEncodings);
doubleEncodes = doubleEncodes.GetAncOfType(typeof(MethodInvokeExpr));
result = razorEncodings - doubleEncodes;