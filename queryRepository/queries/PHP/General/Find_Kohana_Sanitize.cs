//Kohana sanitizers:
CxList methods = Find_Methods();
CxList memberAccess = All.FindByType(typeof(MemberAccess));

CxList checkMethods = methods.FindByShortName("check");

//Validation::Factory is considered a sanitizer only if one of the rules has a sanitizer ex: ->rule('email', 'email') and
//if after the application of the rules, they are checked ex: 	if ($validation->check())
//if one of the rules sanitizes one field, but another field is not sanitized, then we consider that the whole object
//is not sanitized. Ex: email is sanitized, password is not sanitized, then object validation is not sanitized. 
//$validation = Validation::factory($this->request->post())
//	->rule('email', 'email') //sanitized, but all object is not sanitized
//	 
//	->rule('password', 'not_empty')
CxList allValidation = 
	All.FindByMemberAccess("Validation.factory") + 
	All.FindByMemberAccess("Validate.factory");



CxList createExp = All.FindByType(typeof(ObjectCreateExpr));
allValidation.Add(createExp.FindByShortName("Validate") + createExp.FindByShortName("Validation"));//for Kohana 2.x versions

CxList instORM = All.FindByMemberAccess("ORM.factory");
 
CxList target = checkMethods.GetTargetOfMembers();
CxList methodsInfluences = target.DataInfluencedBy(instORM);
//Rules of Valid class that are sanitizers
List<String> rulesSanitize = new List<String> {"email", "url", "ip", "phone", "credit_card", "date", "alpha", 
		"alpha_dash", "alpha_numeric", "digit", "decimal", "numeric", "range", "color"};

//Dcitionary to save every item inside a rule, if a rule has several rules for the same parameter and one of them 
//is not sanitized the object is considered not sanitized
Dictionary <string,bool> sanitizedObjects = new Dictionary<string,bool>();

//Final list of objects that are sanitized
CxList ValidationSanitizedObjects = All.NewCxList();
//flag to see if an object is considered sanitized
bool isSanitized = true;
bool flag = true;
//Iterate over all validation objects
foreach(CxList validator in allValidation){
	//List of rules inside each method
	CxList rules = validator.GetMembersOfTarget();
	//Iterate over all rules inside a validation object
	while(rules.Count > 0){
		//look only at rules, there can also be filters
		if(rules.FindByShortName("rule").Count > 0){
			//Get first parameter of the rule to add to the dictionary
			CxList firstParameter = All.GetParameters(rules, 0).FindByType(typeof(StringLiteral));
			//Get first parameter to add to the dictionary
			StringLiteral firstParameterName = firstParameter.TryGetCSharpGraph<StringLiteral>();
			//Get first parameter name of the rule and add it to the dictionary and remove "" from the key
			if(!sanitizedObjects.ContainsKey(firstParameterName.ShortName.Replace("'", ""))){
				sanitizedObjects.Add(firstParameterName.ShortName.Replace("'", ""), false);
			}
			//Get second parameter to see if it is a sanitizer
			CxList secondParameter = All.GetParameters(rules, 1).FindByType(typeof(StringLiteral));
			//List to verify if the second parameter is a sanitizer
			CxList secondParameterSanitizer = secondParameter.FindByShortNames(rulesSanitize);
			//if it is, put its flag on the dictionary as true
			if (secondParameterSanitizer.Count > 0){
				sanitizedObjects[firstParameterName.ShortName] = true;
			}
			//Get next rule
			rules = rules.GetMembersOfTarget();
		}
		else{
			//Get next rule
			rules = rules.GetMembersOfTarget();
		}
	}
	
	//After checking all rules verify that the object is sanitized, it is only sanitized if all keys are true
	foreach(String key in sanitizedObjects.Keys){
		if(sanitizedObjects[key] == false)
			isSanitized = false;
	}
	//if validation object is sanitized add it to the final list
	if(isSanitized == true){
		ValidationSanitizedObjects.Add(validator);
	}
	
	//Reset values needed for next iteration
	sanitizedObjects.Clear();
	flag = true;
	isSanitized = true;
}

CxList getInstInfluences = target.DataInfluencedBy(ValidationSanitizedObjects);
CxList isValidMethods = (ValidationSanitizedObjects + getInstInfluences).GetMembersOfTarget() * checkMethods;
CxList ORMcheckMethod = (instORM + methodsInfluences) * checkMethods.GetTargetOfMembers();
result.Add(ORMcheckMethod);

//handling if-statement with only validation/validate check() method

CxList ifStmt = (All.FindByType(typeof(IfStmt)));
CxList exp = All.FindByType(typeof(Expression));
CxList isValidMethodAsCond = isValidMethods.GetByAncs(exp.FindByFathers(ifStmt));
CxList relevantIfStmt = isValidMethodAsCond.GetAncOfType(typeof(IfStmt));


CxList isNOTValidMethodAsCond = isValidMethodAsCond.GetByAncs(All.FindByShortName("Not").FindByType(typeof(UnaryExpr)));

CxList sanitizeIfStatement = All.GetByAncs(relevantIfStmt - isNOTValidMethodAsCond.GetAncOfType(typeof(IfStmt)));

result.Add(sanitizeIfStatement);

	
//Kohana_Database
CxList suspectedDbMethods = methods.FindByShortNames(new List<string>
	{"escape", "quote", "quote_column","quote_identifier","quote_table"});

CxList dbInstance = 
	All.FindByMemberAccess("Database.instance") + 
	All.FindByMemberAccess("Database_MySQL.instance") + 
	All.FindByMemberAccess("Database_PDO.instance");


CxList directDbMethods = All.NewCxList();
CxList dbInstanceInfluences = suspectedDbMethods.GetTargetOfMembers().DataInfluencedBy(dbInstance);	
directDbMethods.Add((dbInstance + dbInstanceInfluences).GetMembersOfTarget() * suspectedDbMethods);
directDbMethods.Add(All.FindByMemberAccess("Database.escape"));//For kohana 2.x versions

//handling bind() method executed by database related objects
CxList bindFuncObjCreate = 
	All.FindByMemberAccess("DB.delete") +
	All.FindByMemberAccess("DB.expr") +
	All.FindByMemberAccess("DB.insert") +
	All.FindByMemberAccess("DB.query") + 
	All.FindByMemberAccess("DB.select") +
	All.FindByMemberAccess("DB.select_array") +
	All.FindByMemberAccess("DB.update");
	
bindFuncObjCreate.Add(createExp.FindByShortNames(new List<string> 
	{"Database_Query", "Database_Query_Builder", "Database_Query_Builder_Delete","Database_Query_Builder_Insert",
		"Database_Query_Builder_Join","Database_Query_Builder_Select","Database_Query_Builder_Update","Database_Query_Builder_Where"}));

//find only relevant bind or param methods
CxList bindFunc = 
	(methods + memberAccess).FindByShortName("bind") + 
	(methods + memberAccess).FindByShortName("parameters") + 
	(methods + memberAccess).FindByShortName("param");


CxList bindFuncObjCreateInfluences = bindFunc.GetTargetOfMembers().DataInfluencedBy(bindFuncObjCreate);				
directDbMethods.Add((bindFuncObjCreateInfluences + bindFuncObjCreate).GetMembersOfTarget() * bindFunc);

result.Add(directDbMethods);