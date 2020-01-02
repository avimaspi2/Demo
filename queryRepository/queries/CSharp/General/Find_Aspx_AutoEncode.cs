// Finds the Encodes which are Aspx auto-encodes
CxList htmlEncodes = Find_Encode();

CxList aspxEncodings = htmlEncodes.FindByFileName("*.aspx"); 
CxList doubleEncodes = aspxEncodings.GetFathers().GetByAncs(aspxEncodings);
doubleEncodes = doubleEncodes.GetAncOfType(typeof(MethodInvokeExpr));
result = aspxEncodings - doubleEncodes;