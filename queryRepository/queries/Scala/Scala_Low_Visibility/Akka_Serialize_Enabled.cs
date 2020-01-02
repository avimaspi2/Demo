CxList appConf = Find_Hocon_Application_Conf();

CxList serializeFlagValues = appConf.FindByShortNames(new List<string>{"serialize-messages","serialize-creators"}).GetAssigner().FindByShortName("on");

result = serializeFlagValues;