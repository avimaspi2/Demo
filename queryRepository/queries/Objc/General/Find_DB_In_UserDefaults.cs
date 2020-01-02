CxList methods = Find_Methods();
CxList userDefaultsDB = methods.FindByMemberAccess("NSUserDefaults.standardUserDefaults").GetAssignee();
userDefaultsDB.Add(methods.FindByMemberAccess("UserDefaults.standard").GetAssignee());
CxList unknownReferences = Find_UnknownReference().FindAllReferences(userDefaultsDB);

List<string> membersNames = new List<string>() {
		"setBool:forKey:",
		"setDouble:forKey:",
		"setFloat:forKey:",
		"setInteger:forKey:",
		"setObject:forKey:",
		"setURL:forKey:",
		"setValue:forKey:",
		"setValue:forKeyPath:",
		"setValue:forUndefinedKey:",
		"setNilValueForKey:",
		//Swift
		"set:forKey:" // set(_:forKey:)		
		};
CxList userDefaultsMethod = (methods * unknownReferences.GetMembersOfTarget()).FindByShortNames(membersNames);
result = All.GetParameters(userDefaultsMethod, 0);