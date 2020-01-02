CxList webConfigs = All.FindByFileName("*web.config");

CxList otherConfigFilesName = webConfigs.FindByAssignmentSide(CxList.AssignmentSide.Left).FindByShortName("configSource", false).GetAssigner();
CxList otherConfigFiles = All.NewCxList();

foreach(CxList config in otherConfigFilesName.FindByShortName("*.config", false))
{
	otherConfigFiles.Add(All.FindByFileName("*" + config.GetName()));
}

result.Add(webConfigs);
result.Add(otherConfigFiles);