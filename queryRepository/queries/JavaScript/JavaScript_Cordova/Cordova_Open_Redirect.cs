/// <summary>
/// Finds Redirects on WebViews on Phonegap Applications
/// </summary>
CxList inputs = Find_Inputs();
CxList redir = PhoneGap_Find_Open_Redirect();
CxList sanitize = basic_Sanitize();

result = redir.InfluencedByAndNotSanitized(inputs, sanitize);