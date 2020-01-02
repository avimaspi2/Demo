//	Buffer_Overflow_sizeof
//  //////////////////////
// To find the size of an array of type t the following line should be used: sizeof(arr) / sizeof(t).
// In this query we'll search for sizeof's in array-indexers that aren't of the array's types devided by something.
///////////////////////////////////////////////////////////////////////

// Find all arrays
CxList index = Find_IndexerRefs();

// Find all sizeof methods in arrays
var sizeofList = Find_Methods().GetByAncs(index).FindByName("sizeof");

// Ignore the "sizeof(something)" in cases like: arr[sizeof(arr) / sizeof(something)].
CxList divides = All.FindByType(typeof(BinaryExpr)).GetByBinaryOperator(BinaryOperator.Divide);
CxList sizeofRightSideOfDividedBy = All.NewCxList();
CxList sizeofLeftSideOfDividedBy = All.NewCxList();

foreach (CxList divide in divides){
	try
	{
		var g = divide.data.GetByIndex(0) as BinaryExpr;
		CxList devideRightSide = All.FindById(g.Right.DomId);
		sizeofRightSideOfDividedBy.Add(devideRightSide);
		if ((devideRightSide * sizeofList).Count > 0)
		{
			var devideLeftSide = All.FindById(g.Left.DomId);
			sizeofLeftSideOfDividedBy.Add(devideLeftSide);
		}
	}
	catch(Exception e)
	{
		cxLog.WriteDebugMessage(e.Message);
	}
}
sizeofList -= sizeofRightSideOfDividedBy;

// Prepare a list to be used in the loop
CxList param1 = All.NewCxList();

// Helpers to improve performance
CxList parameters = All.GetParameters(sizeofList);
CxList paramsDef1 = All.FindDefinition(parameters);
CxList paramsDef2 = All.FindByFathers(paramsDef1);
CxList indexDef1 = All.FindDefinition(sizeofList.GetAncOfType(typeof(IndexerRef)));
CxList indexDef2 = All.FindByFathers(indexDef1);

// Check each sizeof element
foreach(CxList sizeofElement in sizeofList)
{
	// Find the array itself (indexer ref)
	index = sizeofElement.GetAncOfType(typeof(IndexerRef));

	// Get the parameters of the element
	param1 = parameters.GetParameters(sizeofElement);
	
	// Get the relevant graphs, so we can check the name and type
	string paramName = param1.GetName();
	string indexName = index.GetName();

	var onLeftSideOfDevide = (sizeofElement * sizeofLeftSideOfDividedBy).Count > 0;
	// If the sizeof isn't devided by anything - it's defaintlly a TP result
	if (!onLeftSideOfDevide)
		result.Add(sizeofElement.DataInfluencedBy(param1));
	
	// See if the array type differs from the parameter
	var indexAndParamSameName = paramName == indexName; 
	var indexAndParamSameType = (index.FindByType(paramName)).Count > 0;
	if (!indexAndParamSameName && !indexAndParamSameType)
	{
		// Ignore cases of different variables of the same type
		CxList paramType = paramsDef2.FindByFathers(paramsDef1.FindDefinition(param1));
		if (paramType.Count > 0)
		{
			CxList indexType = indexDef2.FindByFathers(indexDef1.FindDefinition(index));
			if (indexType.FindByShortName(paramType.GetName()).Count > 0)
			{
				continue;
			}
		}
		// Show the complete dependency between the parameter and the sizeof.
		result.Add(sizeofElement.DataInfluencedBy(param1));
	}
}