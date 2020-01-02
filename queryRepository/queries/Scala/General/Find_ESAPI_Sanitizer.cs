CxList ESAPI = All.NewCxList();

CxList getESAPI = Get_ESAPI();

// Encrypt
ESAPI.Add(getESAPI.FindByMemberAccess("Encryptor.encrypt"));

// Random numbers/names
ESAPI.Add(getESAPI.FindByMemberAccess("Randomizer.get*"));

// getValid*
ESAPI.Add(getESAPI.FindByMemberAccess("Validator.getValid*"));
ESAPI.Add(getESAPI.FindByMemberAccess("ValidationRule.getValid"));

//// Validation rules:
ESAPI.Add(getESAPI.FindByMemberAccess("ValidationRule.getSafe"));
ESAPI.Add(getESAPI.FindByMemberAccess("ValidationRule.sanitize"));
ESAPI.Add(getESAPI.FindByMemberAccess("ValidationRule.whitelist"));

// isValid*
CxList isValid = 
	getESAPI.FindByMemberAccess("Validator.isValid*") +
	getESAPI.FindByMemberAccess("ValidationRule.isValid*");

// Find all "isValid* methods in conditions, or conditions influenced by isValid*
CxList conditions = Find_Conditions();
CxList isValidInCondition = isValid.GetByAncs(conditions) + conditions.DataInfluencedBy(isValid);
isValidInCondition.Add(All.GetParameters(getESAPI.FindByShortName("assert*"), 1));

CxList unknown = All.FindByType(typeof(UnknownReference));

// All unknown references that influence the isValid* inside the condition, and all their references
ESAPI.Add(All.FindAllReferences(unknown.DataInfluencingOn(isValidInCondition)));

result = ESAPI + getESAPI.FindByMemberAccess("Encoder.encodeForBase64");