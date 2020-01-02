CxList Params = Find_Param();
CxList Ifs = Find_Ifs();
CxList relevantParams = Params.GetByAncs(Ifs);
result = relevantParams.FindByShortName("LAPolicyDeviceOwnerAuthentication");