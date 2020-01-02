if(param.Length == 1)
{
	string className = param[0] as string;
	
	CxList methods = Find_Methods();
	//Look for sap.ui.define
	CxList defineList = methods.FindByName("sap.ui.define");
	CxList arrayCreateExpr = Find_ArrayCreateExpr();
	CxList initializers = Find_ArrayInitializer();
	//Go over all sap.ui.define and collect all loaded UI Components
	CxList loadedUIComponents = All.NewCxList();
	foreach(CxList define in defineList)
	{
		CxList defineParam = arrayCreateExpr.GetParameters(define, 0);
		CxList arrayInitializer = initializers.FindByFathers(defineParam);
		ArrayInitializer arrayInit = arrayInitializer.TryGetCSharpGraph<ArrayInitializer>();
		if(arrayInit != null)
		{

			ExpressionCollection exprCollection = arrayInit.InitialValues;	
			if(exprCollection != null)
			{			

				//Look for the position of the given className
				int position = -1;
				for(int i = 0; i < exprCollection.Count; i++)
				{	
					Expression expr = exprCollection[i];
					if(expr == null) continue;
					CxList currentIndex = All.FindById(expr.NodeId);
					if(className == currentIndex.GetName())
					{	
						position = i;
					}
				}
				if(position > -1)
				{
					//Look for the component declaration
					CxList functionParam = All.GetParameters(define, 1);
					loadedUIComponents.Add(All.GetParameters(functionParam, position));
				}
			}
		}
	}
	//Extending class in sap.ui.define
	CxList loadedUIComponentsRefs = All.FindAllReferences(loadedUIComponents);
	result.Add(loadedUIComponentsRefs);
	
	//Direct usage of sap.ui.core.*.extend
	string objectName = className.Replace('/', '.');
	CxList objectReferences = methods.FindByName(objectName);
	result.Add(objectReferences);
}