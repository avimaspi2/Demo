CxList xmlQueryCandidates = Find_Strings().FindByShortNames(new List<string>{
		"*xmltable(*", "*xmltable (*", 
		"*select xmlquery(*", "*select xmlquery (*",
		"select xpath_exists(", "select xpath(", 
		"*.query(*", "*.query (*"
		}, false);


xmlQueryCandidates.Add(Find_Object_Create().FindByShortNames(new List<string>{
		"OXQDataSource", "OXQDDataSource", "DDXQDataSource" }));

result = xmlQueryCandidates;