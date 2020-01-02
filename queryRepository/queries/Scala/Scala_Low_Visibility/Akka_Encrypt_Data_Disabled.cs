/// <summary>
/// This query checks if the flag to encrypt data
/// in session isn't active
/// </summary>

CxList appConf = Find_Hocon_Application_Conf();
result = appConf.FindByName("*session.encrypt-data").GetAssigner().FindByShortName("false");