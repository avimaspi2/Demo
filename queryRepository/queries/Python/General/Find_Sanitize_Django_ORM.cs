CxList methods = Find_DB_In_Django();
methods.Add(Find_DB_Out_Django());

CxList allParams = Find_Param();

CxList relevantMethods = methods.FindByName("*.execute");
relevantMethods.Add(methods.FindByName("*.raw"));

result.Add(All.GetByAncs(allParams.GetParameters(relevantMethods, 1)));