CxList methods = Find_Methods();

result = methods.FindByMemberAccess("SmsMessage.get*");
result.Add(methods.FindByShortName("getIntent"));