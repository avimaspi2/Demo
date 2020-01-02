/// <summary>
/// This query checks if untrusted mode is active.
/// Untrusted mode usually allows to use PoisonPill's on Akka Actors 
/// which may allow to disable the whole system when unproperly used.
/// This also allows remote connections, which can be dangerous.
/// </summary>
CxList config = Find_Hocon_Application_Conf();
CxList untrusted = config.FindByName("akka.remote.untrusted-mode");
CxList Ons = config.FindByShortName("on", false);

result = untrusted.FindByFathers(Ons.GetFathers().FindByType(typeof(AssignExpr)));