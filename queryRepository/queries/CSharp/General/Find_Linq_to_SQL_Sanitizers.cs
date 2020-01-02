//Find All DataContext.ExecuteQuery
CxList type = All.FindByMemberAccess("DataContext.ExecuteQuery");
//Get its parameters
CxList allParameters = All.GetParameters(type);
//The First parameter of DataContext.ExecuteQuery it's not sanititzed by parameterization
CxList notSanitizedParameters = allParameters.GetParameters(type, 0);
//Keep the ones that are considered to be sanitized by parameterization
CxList parameterized = allParameters - notSanitizedParameters;
//Add Unknown references
parameterized.Add(All.FindByFathers(parameterized).FindByType(typeof(UnknownReference)));
//Remove type of Param's from the final result
parameterized -= parameterized.FindByType(typeof(Param));

result.Add(parameterized);