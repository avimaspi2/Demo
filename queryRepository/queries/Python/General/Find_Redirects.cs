CxList methods = Find_Methods();

// Django
CxList django_patterns = All.FindByName("redirect_to").GetAncOfType(typeof(MethodInvokeExpr));
CxList django_redirects = Find_Methods_By_Import("django.http", new string[]{"HttpResponseRedirect", "HttpResponsePermanentRedirect"});
django_redirects.Add(Find_Methods_By_Import("django.shortcuts", new string[]{"redirect"}));
django_redirects.Add(methods.FindByName("RedirectView.as_view"));
django_redirects.Add(django_patterns);
// When the redirect is created with HttpResponse["Location"]
CxList resp = Find_Methods_By_Import("django.http", new string[]{"HttpResponse"});
CxList assigned = All.FindByFathers(resp.GetFathers()).FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList assigned_accesses = All.FindAllReferences(assigned).GetMembersOfTarget();
CxList response_refs = assigned_accesses.FindByMemberAccess("*.\"Location\"");
response_refs.Add(assigned_accesses.FindByMemberAccess("*.\'Location\'"));
response_refs = response_refs.FindByAssignmentSide(CxList.AssignmentSide.Left);
django_redirects.Add(response_refs.GetTargetOfMembers());

// Flask
CxList flask_redirect = methods.FindByName("flask.redirect");

// CGI redirects
CxList cgi_redirects = methods.FindByName("print");
CxList stringLiterals = Find_Strings();
CxList redirectLocationStrings = All.NewCxList();
//We consider a potential redirect all the string literals that contain the word "location":
//- At the beginning of the string or preceded by a new line character
//- On the end of the string or followed by a colon
//Some examples:
// "location"
// "Location: response.redirect"
// "HTTP/1.1 302\nLocation: redirect.php"
//  '''HTTP/1.1 302
//Location: redirect.php'''
String pattern = @"(^|\n|\\n)[lL]ocation($|:)";	
	
foreach(CxList literal in stringLiterals){
	try 
	{
		StringLiteral text = literal.TryGetCSharpGraph<StringLiteral>();
		String fullName = text.FullName.Trim(new char[]{'"', '\''});
		if(Regex.IsMatch(fullName, pattern)){
			redirectLocationStrings.Add(literal);
		}
	} 
	catch (Exception e)
	{
		cxLog.WriteDebugMessage(e.Message);
	} 
}	
cgi_redirects = cgi_redirects.DataInfluencedBy(redirectLocationStrings).GetAncOfType(typeof(MethodInvokeExpr));

CxList redirects = All.NewCxList();
redirects.Add(cgi_redirects);
redirects.Add(django_redirects);
redirects.Add(flask_redirect);

result = redirects;