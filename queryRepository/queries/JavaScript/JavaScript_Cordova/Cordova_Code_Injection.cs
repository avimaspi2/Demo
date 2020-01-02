/// <summary>
/// Finds redirects on WebViews on Cordova Applications
/// </summary>
CxList inputs = Find_Inputs();
CxList code = PhoneGap_Find_Code_Injection();
CxList sanitize = basic_Sanitize();

result = code.InfluencedByAndNotSanitized(inputs, sanitize);