CxList mapConnect = All.GetByAncs(All.FindByMemberAccess("map.connect"));
CxList mapConnectParams1 = All.GetParameters(mapConnect, 1);
mapConnect -= mapConnect.FindByParameters(mapConnectParams1);
CxList mapConnectParams0 = All.GetParameters(mapConnect, 0);

result = mapConnectParams0.FindByShortName(@":controller/:action/:id");