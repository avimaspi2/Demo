CxList methods = Find_Methods();
CxList userDefaultsDB = methods.FindByMemberAccess("NSUserDefaults.standardUserDefaults").GetAssignee();
userDefaultsDB.Add(methods.FindByMemberAccess("UserDefaults.standard").GetAssignee());
CxList unknownReferences = Find_UnknownReference().FindAllReferences(userDefaultsDB);
List<string> membersNames = new List<string>() {
		"*ForKey:",	
		"valueForKeyPath:",
		"valueForUndefinedKey:",
		"*ValueForKeyPath:",
		// Swift
		"objectForKey",
		"url:forKey:",
		"array:forKey:",
		"dictionary:forKey:",
		"string:forKey:",
		"stringArray:forKey:",
		"data:forKey:",
		"bool:forKey:",
		"integer:forKey",
		"float:forKey:",
		"double:forKey:"
		};
CxList userDefaultsMethod = (methods * unknownReferences.GetMembersOfTarget()).FindByShortNames(membersNames);
result = userDefaultsMethod;
result -= userDefaultsMethod.FindByShortName("removeObjectForKey:");