CxList ESAPI = All.NewCxList();
CxList getESAPI = Get_ESAPI();

// Encrypt
ESAPI.Add(getESAPI.FindByShortName("encrypt"));

// Random numbers/names
ESAPI.Add(getESAPI.FindByShortName("getRandom*"));

//// Validation rules:
ESAPI.Add(getESAPI.FindByShortNames(new List<string> {"getSafe", "sanitize", "whitelist"}));
/*
ESAPI.Add(Get_ESAPI().FindByShortName("getSafe"));
ESAPI.Add(Get_ESAPI().FindByShortName("sanitize"));
ESAPI.Add(Get_ESAPI().FindByShortName("whitelist"));
*/

//// Validator:
// getValid*
ESAPI.Add(getESAPI.FindByShortName("getValid*"));
// isValid*
CxList isValid = getESAPI.FindByShortName("isValid*");

// Find all "isValid* methods in conditions
CxList conditions = Find_Conditions();

CxList isValidInCondition = isValid.GetByAncs(conditions) + conditions.DataInfluencedBy(isValid);
isValidInCondition.Add(All.GetParameters(getESAPI.FindByShortName("assert*"), 1));

CxList unknown = All.FindByType(typeof(UnknownReference));

// All unknown references that influence the isValid* inside the condition, and all their references
ESAPI.Add(All.FindAllReferences(unknown.DataInfluencingOn(isValidInCondition)));

result = ESAPI;