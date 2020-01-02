/**
	This query finds all arrays which have a predictable size that is between the the specified 
		interval defined in the arguments of this query. If some argument isn't passed or
		is null, the algorithm will consider as a minimum or maximum limit (long).

	The arguments are the following:
		Minimum Value of the interval | type=long | Default=long.MinValue 
		Maximum Value of the interval | type=long | Default=long.MaxValue

	Examples:
		Find_Arrays_By_Size():
			Will find all arrays with a predicatable size.
		Find_Arrays_By_Size(1L) or Find_Arrays_By_Size(1L,null):
			Will find all arrays with a size bigger then 1. 
		Find_Arrays_By_Size(1L,32L):
			Will find all arrays with a size bigger then 1 and smaller than 32.
		Find_Arrays_By_Size(null,64L):
			Will find all arrays with a size smaller than 64.
*/

/*** Defaults ***/
long minValue = long.MinValue;
long maxValue = long.MaxValue;

/*** Parameters definition ***/
if(param.Length >= 2 && param[1]!=null){
	maxValue = (long) param[1];
}
if(param.Length >= 1 && param[0]!=null){
	minValue = (long) param[0];
}
	
/*** Algorithm to gather arrays with the specified size ***/
IAbstractValue absInterval = new IntegerIntervalAbstractValue(minValue, maxValue);

CxList absObjects = All.FindByAbstractValue(abstractValue => {
	if (abstractValue is ObjectAbstractValue){
		var objAbsValue = abstractValue as ObjectAbstractValue;
		if(objAbsValue.AllocatedSize != null){
			return objAbsValue.AllocatedSize.IncludedIn(absInterval);
		}
	}
	return false;
	});

result = absObjects;