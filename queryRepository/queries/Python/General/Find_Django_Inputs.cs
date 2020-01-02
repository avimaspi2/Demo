/*** This query finds inputs from Django

Documentation for Models: https://docs.djangoproject.com/en/dev/ref/models/fields/#field-types
Documentation for Forms: https://docs.djangoproject.com/en/dev/ref/forms/fields/#field-types
***/
if (Find_Django().Count != 0)
{
	CxList methods = Find_Methods();
	string[] fieldTypesModel = {
		"AutoField",
		"BigIntegerField",
		"BinaryField",
		"BooleanField",
		"CharField",
		"CommaSeparatedIntegerField",
		"DateField",
		"DateTimeField",
		"DecimalField",
		"EmailField",
		"FileField",
		"FilePathField",
		"FloatField",
		"ImageField",
		"IntegerField",
		"IPAddressField",
		"GenericIPAddressField",
		"NullBooleanField",
		"PositiveIntegerField",
		"PositiveSmallIntegerField",
		"SlugField",
		"SmallIntegerField",
		"TextField",
		"TimeField",
		"URLField"
		};

	string[] fieldTypesForm = {
		"BooleanField",
		"CharField",
		"ChoiceField",
		"ComboField",
		"DateField",
		"DateTimeField",
		"DecimalField",
		"EmailField",
		"FileField",
		"FilePathField",
		"FloatField",
		"ImageField",
		"IntegerField",
		"IPAddressField",
		"GenericIPAddressField",
		"MultipleChoiceField",
		"MultiValueField",
		"NullBooleanField",
		"RegexField",
		"SlugField",
		"SplitDateTimeField",	
		"TimeField",
		"URLField",
		"TypedChoiceField",
		"TypedMultipleChoiceField"
		};

	foreach(string field in fieldTypesModel)
	{
		result.Add(methods.FindByName("models." + field)); 
	}

	result.Add(Find_Methods_By_Import("*.models", fieldTypesModel));

	foreach(string field in fieldTypesForm)
	{
		result.Add(methods.FindByName("forms." + field)); 
	}

	result.Add(Find_Methods_By_Import("*.forms", fieldTypesModel));

	// add cookies
	CxList cookies = methods.FindByName("request.COOKIES");
	cookies.Add(Find_Members("request.GET"));
	cookies.Add(Find_Members("request.POST"));
	cookies.Add(Find_Members("request.FILES"));
	//Handle cases like request.GET['c'] 
	CxList indexCookies = cookies.GetMembersOfTarget();

	result.Add(cookies);
	result.Add(indexCookies - indexCookies.FindByType(typeof(MethodInvokeExpr)));
	// Handle view methods parameter as input
	CxList viewInputs = All.GetParameters(Find_MethodDecls().FindByFileName("*\\views*"), 0);
	viewInputs.FindByFileName("*.py");
	result.Add(viewInputs);
	result.Add(All.FindAllReferences(viewInputs).GetMembersOfTarget());

	CxList memberAccess = Find_MemberAccesses().FindByFileName("*.html");
	result.Add(memberAccess.FindByName("*form.*"));
	result.Add(memberAccess.FindByName("*RegisterForm.*"));
}
else
{
	result = All.NewCxList();
}