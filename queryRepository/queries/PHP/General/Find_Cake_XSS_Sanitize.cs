if(Find_Ctp_Files().Count > 0)
{
	CxList controller = All.FindByType(typeof(ClassDecl)).FindByShortName("*sController");
	//$value=Sanitize::clean($input)
	CxList sanitizer = (All.FindByShortName("Sanitize").GetByAncs(controller)).GetMembersOfTarget();
	
	System.Collections.Generic.List < string > sanitizerStrings = 
		new System.Collections.Generic.List<string>(new string[] {"clean", "paranoid", "escape", "html"});

	sanitizer = sanitizer.FindByShortNames(sanitizerStrings);

	result = sanitizer;

	CxList methods = Find_Methods();
	result.Add(methods.FindByShortName("h") +
		methods.FindByShortName("clean").DataInfluencedBy(All.FindByShortName("CleanerHelper")));

	//form clean

	CxList form = All.FindByType(typeof(MemberAccess)).FindByShortName("Form");
	CxList formHelperMethodCall = form.GetTargetOfMembers().FindByType(typeof(ThisRef)).GetMembersOfTarget().GetMembersOfTarget();
	result.Add(formHelperMethodCall.FindByType(typeof(MethodInvokeExpr)).FindByShortName("clean"));
	//all DB-out methods are sanitizers except query.
	result.Add(Find_Cake_DB_Out() - All.FindByShortName("query"));
	//all DB-In methods are sanitizers except updateAll 
	CxList saveExpr = Find_Cake_DB_In() - All.FindByShortName("updateAll");
	result.Add(saveExpr);
	
	//Validator class
	CxList inputs = Find_Interactive_Inputs();
	//get add methods from validator
	CxList adds = methods.FindByShortName("add", false);
	//list of rules that can be use as sanitizer
	List <string> validation_names = new List<string>() {"cc", "numeric", "alphanumeric", "email"};
	CxList validations = All.FindByShortNames(validation_names);

	CxList relevant_adds = All.NewCxList();
	foreach(CxList a in adds){
		//get add that have rules that can be used as sanitizer
		CxList ancs_of_add = validations.GetByAncs(a);
		if(ancs_of_add.Count > 0){
			relevant_adds.Add(a);
		}
	}
	CxList validator_objects = All.FindAllReferences(relevant_adds.GetTargetOfMembers());
	CxList errors = methods.FindByShortName("errors") * validator_objects.GetMembersOfTarget();
	
	//Potential sanitized inputs
	CxList inputReferences = All.FindAllReferences(All.GetParameters(errors));
	result.Add(inputReferences);
	
	//Validation class
	//methods that checks if the value is a certain type
	CxList validationMethodsCheck = methods.FindByMemberAccess("Validation.alphaNumeric");
	validationMethodsCheck.Add(methods.FindByMemberAccess("Validation.boolean"));
	validationMethodsCheck.Add(methods.FindByMemberAccess("Validation.cc"));
	validationMethodsCheck.Add(methods.FindByMemberAccess("Validation.data"));
	validationMethodsCheck.Add(methods.FindByMemberAccess("Validation.datetime"));
	validationMethodsCheck.Add(methods.FindByMemberAccess("Validation.decimal"));
	validationMethodsCheck.Add(methods.FindByMemberAccess("Validation.email"));
	validationMethodsCheck.Add(methods.FindByMemberAccess("Validation.ip"));
	validationMethodsCheck.Add(methods.FindByMemberAccess("Validation.money"));
	validationMethodsCheck.Add(methods.FindByMemberAccess("Validation.naturalNumber"));
	validationMethodsCheck.Add(methods.FindByMemberAccess("Validation.numeric"));
	validationMethodsCheck.Add(methods.FindByMemberAccess("Validation.time"));
	validationMethodsCheck.Add(methods.FindByMemberAccess("Validation.url"));
	validationMethodsCheck.Add(methods.FindByMemberAccess("Validation.uuid"));
	result.Add(validationMethodsCheck);
	
	//inputs that are inside of Validation class methods
	CxList inputsInValidationMethods = All.GetParameters(validationMethodsCheck,0) * inputs;
	result.Add(All.FindAllReferences(inputsInValidationMethods));	
}