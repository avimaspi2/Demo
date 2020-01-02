/*
MISRA C RULE 13-4
------------------------------
This query searches for floating point objects in for loops control statements

	The Example below shows code with vulnerability: 

float32_t f;
for ( i = 0; i < f; i++ ){
...
}

*/

// first we build a list of all floating point typedefs
CxList typedefDecls = All.FindByType(typeof(StringLiteral)).FindByName("CX_TYPEDEF").GetFathers();
CxList floatTypedefDecls = typedefDecls.FindByType("float") + typedefDecls.FindByType("double");
ArrayList floatTypes = new ArrayList();
floatTypes.Add("float");
floatTypes.Add("double");
foreach(CxList cur in floatTypedefDecls){
	string typeName = ((Declarator) cur.data.GetByIndex(0)).Name;
	if(typeName != null){
		if (!floatTypes.Contains(typeName))
		{
			floatTypes.Add(typeName);
			floatTypes.Add("*." + typeName);
		}
	}
}

// now build a list of all floating point declarators
CxList floatDecls = Find_All_Declarators().FindByTypes((string[]) floatTypes.ToArray(typeof(string)));
floatDecls -= typedefDecls;

// uses are decl instances
CxList fpoints = All.FindAllReferences(floatDecls).FindByType(typeof(UnknownReference)) +
	All.FindByType(typeof(RealLiteral));
// remove all casted
CxList castIntoTypes = All.FindByFathers(All.FindByType(typeof(CastExpr))).FindByType(typeof(TypeRef));
fpoints -= fpoints.GetByAncs(castIntoTypes);
foreach(CxList cur in castIntoTypes){
	TypeRef g = (TypeRef) cur.data.GetByIndex(0);
	if (g == null || g.FullName == null) {
		continue;
	}
	string curName = g.FullName;
	// if irrelevant type, remove it from casted list
	if(curName != null)
	{
		if (!floatTypes.Contains(curName))
		{
			castIntoTypes -= cur;
		}
	}
}
// add back relevant casted
fpoints.Add(All.GetByAncs(castIntoTypes.GetFathers()).FindByType(typeof(UnknownReference)) +
	All.GetByAncs(castIntoTypes.GetFathers()).FindByType(typeof(RealLiteral)));


// find all for loop control expressions
CxList forControls = All.NewCxList();
CxList forIters = All.FindByType(typeof(IterationStmt));
CxList forIterSons = All.FindByFathers(forIters);

foreach (CxList cur in forIters){
	IterationStmt curIter = (IterationStmt) cur.data.GetByIndex(0);
	if(curIter != null && curIter.IterationType != null && curIter.Test != null)
	{
		if (curIter.IterationType == IterationType.For){
			//forControls.Add(curIter.Test.NodeId, curIter.Test);
			Expression t = curIter.Test;
			if(t != null)
			{
				forControls.Add(All.FindById(t.NodeId));
			}
				
		}
	}
}


result = fpoints.GetByAncs(forControls);