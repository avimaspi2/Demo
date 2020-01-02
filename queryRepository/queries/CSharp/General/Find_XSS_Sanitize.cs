CxList methods = Find_Methods();
CxList integers = Find_Integers();
CxList integersTypeRef = integers.FindByType(typeof(TypeRef));

CxList resourceMethod = All.FindByMemberAccess("*.GetGlobalResourceObject");
resourceMethod.Add(All.FindByMemberAccess("*.GetLocalResourceObject"));

CxList sanitizingMethods = All.FindByMemberAccess("*.GetType");

CxList guid = base.Find_ObjectCreations().FindByShortName("Guid", true);

CxList htmlSafer = methods.FindByShortName("GetSafeHtmlFragment", false).GetMembersWithTargets();

CxList escapingMethods = All.FindByMemberAccess("Uri.EscapeDataString");
escapingMethods.Add(All.FindByMemberAccess("Uri.EscapeUriString"));

// Get Left Parts (Scheme, Authority)
CxList leftParts = methods.FindByMemberAccess("Uri.GetLeftPart");
leftParts.Add(methods.FindByName("*Request.Url.GetLeftPart"));
leftParts -= All.GetParameters(leftParts)
	.FindByShortNames(new List<string>{"Path","Query"})
	.GetAncOfType(typeof(MethodInvokeExpr));
escapingMethods.Add(leftParts);

// ToString invocation of DateTime's
String[] dateTimeTypes = new String[] {"DateTime", "System.DateTime"};
CxList dateTime = base.Find_UnknownReference().FindByTypes(dateTimeTypes);
{	
	CxList dateTimeFathers = dateTime.GetFathers();
	String[] dateTimeStaticMembers = new string[]{
		"DateTime.Now",
		"DateTime.UtcNow",
		"DateTime.Today",
		"DateTime.SpecifyKind"};

	foreach (String m in dateTimeStaticMembers)
		dateTime.Add(dateTimeFathers.FindByMemberAccess(m));
	
	dateTime = dateTime.GetMembersOfTarget().FindByName("*.ToString");
}

CxList nameof = methods.FindByShortName("nameof");
result.Add(nameof);

//ToString and ToShortDateString invocation of Integers 
//Integers CastExpr
CxList integerCastExpr = integersTypeRef.GetFathers().FindByType(typeof(CastExpr));
CxList membersOfCastExpr = integerCastExpr.GetFathers().GetMembersOfTarget();
result.Add(membersOfCastExpr.FindByShortNames(new List<String> {"ToShortDateString", "ToString"}));

//razor helper object is sanitized 
CxList helperMethods = Find_ASP_MVC_HtmlHelper_Methods();
helperMethods -= helperMethods.FindByShortName("Raw");
result.Add(helperMethods.GetTargetOfMembers());

//Find XSS sanitizing Headers
//Content-Type as plain/text and Content-Disposition with as attachment are benign
//The second one only if attached with the option: nosniff
CxList httpResponses = Find_Response();
CxList httpHeaderChangeMethods = Find_Change_Response_Header();

CxList httpResponsesSanitized = httpResponses.GetMembersOfTarget()
	.FindByShortName("ContentType").GetAssigner().FindByShortName("text/plain");
CxList httpResponsesParams = httpResponses.FindByType(typeof(StringLiteral));
httpResponsesParams.Add(All.GetParameters(httpHeaderChangeMethods));
CxList contentType = httpResponsesParams.FindByShortName("Content-Type");
CxList textPlain = httpResponsesParams.FindByShortName("text/plain");
CxList ancesterMethod = 
	contentType.GetAncOfType(typeof(MethodInvokeExpr)) * textPlain.GetAncOfType(typeof(MethodInvokeExpr));
httpResponsesSanitized.Add(ancesterMethod);

//If ContentType different than text/plain than
//Only fully sanitized if X-Content-Type-Options: nosniff is set::
CxList contentDisposition = httpResponsesParams.FindByShortName("Content-Disposition");
CxList attachment = httpResponsesParams.FindByRegex("attachment; filename=");
CxList contentSafe = contentDisposition
	.GetAncOfType(typeof(MethodInvokeExpr)) * attachment.GetAncOfType(typeof(MethodInvokeExpr));

CxList xContentTypeOptions = httpResponsesParams.FindByShortName("X-Content-Type-Options");
CxList noSniff = httpResponsesParams.FindByShortName("nosniff");
CxList noSniffOption = xContentTypeOptions
	.GetAncOfType(typeof(MethodInvokeExpr)) * noSniff.GetAncOfType(typeof(MethodInvokeExpr));
//If the results are influenced by these headers they are considered sanitized
//result.Add(httpResponsesSanitized.GetTargetOfMembers().GetTargetOfMembers());

CxList httpResponsesSanitizedContent = All.NewCxList();
httpResponsesSanitizedContent.Add(httpResponsesSanitized);
httpResponsesSanitizedContent.Add(All.FindAllReferences(contentSafe) * All.FindAllReferences(noSniffOption));
CxList xssOutputs = All.FindByMemberAccess("Response.*");
CxList httpResponsesFullySanitized = xssOutputs.InfluencedBy(httpResponsesSanitizedContent);

CxList contentMethods = methods.FindByShortName("Content");
CxList applicationJsonStrVal = All.FindByAbstractValue(
	absVal => absVal is StringAbstractValue strAbsVal 
	&& string.Equals(strAbsVal.Content, "application/json", StringComparison.CurrentCultureIgnoreCase));
CxList contentSanitizers = applicationJsonStrVal.GetParameters(contentMethods, 1);


result.Add(methods.FindByMemberAccess("SyntaxFactory.ParseSyntaxTree"));
result.Add(methods.FindByMemberAccess("Server.MapPath"));
result.Add(dateTime);
result.Add(Find_XSS_Replace());
result.Add(Find_Encode());
result.Add(integers);
result.Add(resourceMethod);
result.Add(sanitizingMethods);
result.Add(guid);
result.Add(htmlSafer);
result.Add(escapingMethods);
result.Add(httpResponsesFullySanitized);
result.Add(contentMethods.FindByParameters(contentSanitizers));