/*checks if verify_peer of ssl is set to false*/
CxList falseIndicator = All.FindByShortName("verify_peer").FindByType(typeof(StringLiteral));
//CxList methods = Find_Methods();
CxList arrayMethod = All.FindByParameters(falseIndicator).FindByShortName("array");
CxList boolean = All.FindByShortName("false").FindByType(typeof(BooleanLiteral));
CxList unknownRef = All.FindByType(typeof(UnknownReference));

CxList potentialBool = All.NewCxList();
CxList indicatorCounter = All.NewCxList();
CxList unkAndBool = unknownRef + boolean;

foreach(CxList array in arrayMethod)
{
	int i;
	for(i = 0; i < 30; i++)
	{
		indicatorCounter = falseIndicator.GetParameters(array, i);		
		if(indicatorCounter.Count > 0)
		{
			break;
		}
	}

	potentialBool.Add(unkAndBool.GetParameters(array, i + 1));	
}
result.Add(potentialBool * boolean);
result.Add(potentialBool.DataInfluencedBy(boolean));