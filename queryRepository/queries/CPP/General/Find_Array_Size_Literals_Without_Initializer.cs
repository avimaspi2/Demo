CxList arrays = Find_ArrayCreateExpr();
CxList num = Find_Integer_Literals();
CxList init = All.FindByType(typeof(ArrayInitializer));
CxList filter = init.FindByFathers(arrays);
CxList exprs = Find_Expressions();

arrays -= (exprs.GetByAncs(filter)-filter).GetFathers();

arrays -= arrays.GetByAncs(Find_ObjectCreations());

foreach(CxList array in arrays){
	try{
		foreach(Expression expr in array.TryGetCSharpGraph<ArrayCreateExpr>().Sizes){
			result.Add(num.FindById(expr.NodeId));
		}
	}catch(Exception){};
}