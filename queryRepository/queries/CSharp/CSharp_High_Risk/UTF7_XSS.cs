if(All.isWebApplication)
{
	CxList UTF7 = Find_Strings().FindByName("UTF-7");
	CxList response = All.FindByName("*Response.Charset");

	UTF7 = response.DataInfluencedBy(UTF7);

	// get last node of the path (this node is part of response CxList
	CxList temp = UTF7.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

	// take from response only nodes that influenced by UTF7
	response = temp;
	
	if(UTF7.Count > 0)
	{
		CxList outputs = Find_XSS_Outputs();
		CxList inputs = Find_Interactive_Inputs();
		CxList sanitize = Find_UTF7_XSS_Sanitize();
		CxList tempInputs = All.NewCxList();
		
		//limit to inputs in the same class as "UTF-7"
		foreach (CxList r in response)
		{
			CxList responseClass = r.GetAncOfType(typeof(ClassDecl));

			foreach (CxList i in inputs)
			{
				CxList inputsClass = i.GetAncOfType(typeof(ClassDecl));
				CxList sameClass = responseClass * inputsClass;

				if (sameClass.Count > 0)
				{ 
					tempInputs.Add(i);
				}
			}
		}
		
		inputs = tempInputs;	
		
		result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);
	}
}