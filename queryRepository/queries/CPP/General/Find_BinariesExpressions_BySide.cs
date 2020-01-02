// This auxiliary function returns the left or right items in binary expressions presents in CxLists
// param[0] -> The CxList to search the items
// param[1] -> Boolean indicating if we shoud return the left side (true) or right side (false) of the binary expression.
if (param.Length == 2) {
	CxList list = param[0] as CxList;
	bool isLeftSide = (bool) param[1];
	
	foreach(CxList val in list) {
		BinaryExpr item = val.TryGetCSharpGraph<BinaryExpr>();
		if (item != null) {
			var expr = isLeftSide ? item.Left : item.Right;	
			if (expr != null) {
				result.Add(All.FindById(expr.NodeId));
			}
		}
	}
}