/// <summary>
/// Log level used by the configured loggers (see "loggers") as soon
/// as they have been started; before that, see "stdout-loglevel"
/// Options: OFF, ERROR, WARNING, INFO, DEBUG
/// </summary>

CxList conf = Find_Hocon_Application_Conf();
CxList debugLoglevelEnabled = conf.FindByShortName("loglevel").GetAssigner().FindByShortName("DEBUG");
result = debugLoglevelEnabled;