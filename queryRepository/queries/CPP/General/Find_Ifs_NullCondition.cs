CxList nullLiterals = All.FindByType(typeof(NullLiteral)) + Find_Integer_Literals().FindByShortName("0");
IAbstractValue zero = new IntegerIntervalAbstractValue(0);
IAbstractValue nullVal = new NullAbstractValue();
CxList nullPointers = Find_UnknownReference().FindByAbstractValue(abs => zero.IncludedIn(abs) || nullVal.IncludedIn(abs));

CxList notEqBinaries = Find_BinaryExpressions().GetByBinaryOperator(BinaryOperator.IdentityInequality);
CxList leftSides = Find_BinariesExpressions_BySide(notEqBinaries, true);
CxList rightSides = Find_BinariesExpressions_BySide(notEqBinaries, false);

CxList validBins = All.NewCxList();
foreach(CxList binExpr in notEqBinaries) {
	CxList left = leftSides * nullPointers;
	CxList right = rightSides * nullLiterals;
	if (left.Count > 0 && right.Count > 0) validBins.Add(binExpr);
}

result = Find_Ifs() * validBins.GetFathers();