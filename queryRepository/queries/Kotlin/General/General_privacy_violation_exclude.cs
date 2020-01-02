CxList personalList = General_privacy_violation_list();

CxList excludePersonalList = personalList.FindByShortNames(new List<string>{
		"*className*", "*author*" }, false);
//excludePersonalList.Add(Find_Dead_Code_Contents());

result = excludePersonalList;