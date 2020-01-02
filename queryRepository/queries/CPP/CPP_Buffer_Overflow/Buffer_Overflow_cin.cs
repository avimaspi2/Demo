//            Buffer Overflow - cin
//  ////////////////////-
//  Find all cin inputs to arrays that have no cin.width sanitation.
///////////////////////////////////////////////////////////////////////

// Find assignments from cin (inputs)
CxList assignExpr = Find_AssignExpr();
List<string> cin_names = new List<string>() {"cin", "wcin"}; 
CxList cinsNames = All.FindByShortNames(cin_names);
CxList cins = cinsNames.GetByAncs(assignExpr);

// Find all array creations
CxList arrays = Find_ArrayCreateExpr(); // array creation
CxList arraysFathers = arrays.GetFathers();
arrays = All.FindByFathers(arraysFathers.FindByType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left);
arrays.Add(arraysFathers.FindByType(typeof(Declarator)));
arrays = All.FindAllReferences(arrays); // all the arrays' references

CxList cinsGetsArray = arrays.GetFathers() * cins.GetFathers(); // find only cin inputs
CxList cinsArrays = cins.GetByAncs(cinsGetsArray); // get the arrays themselves

// Result should contain all inputs to arrays
result.Add(cinsArrays * cins);

// Find sanitizer
CxList cinsWidth = All.FindByName("*cin.width");
cinsWidth.Add(All.FindByName("*wcin.width"));
cinsWidth = cinsWidth.GetTargetOfMembers().FindByShortNames(cin_names);

// Remove sanitizer
result -= cins.DataInfluencedBy(cinsWidth);

CxList unknownRefs = Find_Unknown_References();

List<string> cinReads = new List<string>() {"getline", "get","read"}; 
CxList cinMembers = cinsNames.GetMembersOfTarget().FindByShortNames(cinReads) ;

foreach(CxList getline in cinMembers){
	CxList param1 = unknownRefs.GetByAncs(All.GetParameters(getline, 0));
	if (param1 != null && param1.Count > 0)
	{
		CxList arraySize = Find_Array_Size(param1);
		int maxSize = 0;
		if(arraySize.Count > 0 && int.TryParse(arraySize.GetName(), out maxSize))
		{
			CxList param2 = All.GetParameters(getline, 1);
			if (param2.Count > 0)
			{
				param2 = param2.FindByAbstractValue(abstractValue => abstractValue is IntegerIntervalAbstractValue);
				IAbstractValue validInterval = new IntegerIntervalAbstractValue(0, maxSize - 1);
				CxList invalidIntervals = param2.FindByAbstractValue(abstractValue => !abstractValue.IncludedIn(validInterval));
		
				if(invalidIntervals.Count > 0)
					result.Add(getline);
			}
		}   
	}
}