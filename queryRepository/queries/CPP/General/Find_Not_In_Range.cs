/*
	Find_Not_In_Range 
	package 		- The name of the package (Optional) (dont use this arg if you want to use built in methods)
	methodName 		- The name of the methods with the arguments we are looking for
	argPosition 	- The argument position
	min 			- minimum value accepted ( 'null' for infinity ) 
	max 			- maximum value accepted ( 'null' for infinity ) 
*/ 
if (param.Length >= 4 && param.Length <= 5)
{	
	int i = 0;
	string package = null;	
		
	if ( param.Length == 5 ) {
		package = param[i++] as string;			
	}
	
	var methodName = param[i++];
		
	int argPosition = (int) param[i++];
	int? min = (int?) param[i++];
	int? max = (int?) param[i++];

	CxList absIntegers = All.NewCxList();
	CxList methods = All.NewCxList();
	CxList parameters = All.NewCxList();
	CxList range = All.NewCxList();

	absIntegers.Add(All.FindByAbstractValue(abstractValue => abstractValue is IntegerIntervalAbstractValue));

	IAbstractValue absMinMaxRange = new IntegerIntervalAbstractValue(min, max);
	range.Add(absIntegers.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(absMinMaxRange)));
	
	if ( param.Length == 5 ){
		methods = Find_Members_By_Include(package, new string[] {methodName as string});
	}else{
		methods = (methodName is string) ? Find_Methods().FindByShortName(methodName as string) : methodName as CxList;
	}
		
	parameters = All.GetParameters(methods, argPosition);
		
	// remove 'Param' Types
	CxList typeParam = parameters.FindByType(typeof(Param));
	parameters = parameters - typeParam;

	CxList goodOnes = parameters.FindByAbstractValues(range);
	CxList badOnes = parameters - goodOnes;

	result.Add(badOnes);
}else
{
	cxLog.WriteDebugMessage("Number of parameters should be 4 or 5");			
}