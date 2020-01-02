CxList reactClasses = React_Find_ClassComponent();
CxList fields = Find_FieldDecls();
CxList arrays = base.Find_ArrayCreateExpr();

CxList mixins = fields.FindByShortName("mixins");
CxList mixinsReact = mixins.GetByAncs(reactClasses);
CxList mixinsInitializers = arrays.GetByAncs(mixinsReact);

foreach(CxList mixin in mixinsInitializers){
	ArrayCreateExpr arrayDef = mixin.TryGetCSharpGraph<ArrayCreateExpr>();
	if(arrayDef != null && arrayDef.Initializer != null && arrayDef.Initializer.InitialValues != null) {
		if(arrayDef.Initializer.InitialValues.Count > 1) {
			result.Add(mixin);
		}
	}
}