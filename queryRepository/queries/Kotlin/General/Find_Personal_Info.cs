// Personal List
result.Add(Find_personal_information(
	General_privacy_violation_list(),
	General_privacy_violation_exclude()
	));

// Credit List
result.Add(Find_personal_information(
	Credit_privacy_violation_list(), 
	Credit_privacy_violation_exclude()
));

// Password List
CxList passwordPrivacyViolationList = Password_privacy_violation_list();
CxList findPersonalInfo = All.NewCxList();
findPersonalInfo.Add(passwordPrivacyViolationList);

result.Add(Find_personal_information(
	findPersonalInfo,
	Password_privacy_violation_exclude()
	));

// Android Personal Info
result.Add(Find_Android_Personal_Info());