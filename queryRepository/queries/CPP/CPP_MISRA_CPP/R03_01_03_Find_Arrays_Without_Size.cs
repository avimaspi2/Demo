/*
 MISRA CPP RULE 3-1-3
 ------------------------------
 This query searches for array creations without implicit or initialized sizes.
 
 The Example below shows code with vulnerability: 

	int array1[10];			//Compliant
	extern int array2[];	//Non-compliant
	int array3[] = {0,1,2}	//Compliant
	extern int array4[22];	//Compliant

*/

CxList arrays = All.FindByFathers(All.FindByType(typeof(ArrayCreateExpr))).GetFathers();
foreach(CxList array in arrays) {
	ArrayCreateExpr curr = array.data.GetByIndex(0) as ArrayCreateExpr;
	if(curr.Sizes.Count == 0 && curr.DimensionCount == 0) {
		result.Add(array);
	}
}
result = result.GetFathers();