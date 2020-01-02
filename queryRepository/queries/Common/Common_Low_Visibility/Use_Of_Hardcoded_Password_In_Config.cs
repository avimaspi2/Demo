CxList allPasswords = Find_All_Passwords();

CxList propertiesFile = All.FindByFileName("*.properties");
	
CxList propertiesElements = propertiesFile.FindByType(typeof(UnknownReference));
propertiesElements.Add(propertiesFile.FindByType(typeof(MemberAccess)));

CxList passwordInPropertiesFiles = propertiesElements.FindByShortName(allPasswords, true);	

result.Add(passwordInPropertiesFiles);