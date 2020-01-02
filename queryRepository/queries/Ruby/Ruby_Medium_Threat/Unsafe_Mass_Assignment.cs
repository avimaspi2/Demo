// In case the attr_accessible was defined absolutely:
CxList send = All.FindByMemberAccess("ActiveRecord.Base").GetMembersOfTarget().FindByShortName("send");
send = All.GetParameters(send, 0).FindByShortName("attr_accessible");
/* In case it didn't, we have the following two cases:
	case 1:
		User.new(param[:user])
		// and user model has no attr_accessible defined

	case 2:
		we have flow from inputs to specific database calls 
		and the call is not sanitized by attr_accessible on model context
*/

if (send.Count == 0)
{
	CxList allModels = Find_Models();
	CxList modelUnderModels = allModels;	
	CxList unknownRef = All.FindByType(typeof(UnknownReference));
	CxList invokeExpr = All.FindByType(typeof(MethodInvokeExpr));
	CxList inputs = (Find_Interactive_Inputs() + invokeExpr).FindByShortName("params");
	CxList newStatament = invokeExpr.FindByShortName("new") +
		All.FindByType(typeof(ObjectCreateExpr));	
	CxList paramOfObjectCreate = unknownRef.GetParameters(newStatament).DataInfluencedBy(inputs);	
	CxList methods = Find_Methods();
	CxList sanitize = Find_Strong_Parameters();
	
	sanitize += invokeExpr.FindAllReferences(
		sanitize.GetFathers().FindByType(typeof(ReturnStmt))
		.GetAncOfType(typeof(MethodDecl))
		).GetFathers().GetAncOfType(typeof(MethodInvokeExpr));
	
	CxList ofNew = (inputs + paramOfObjectCreate).GetAncOfType(typeof(ObjectCreateExpr));
	
	ofNew.Add((inputs + paramOfObjectCreate).GetAncOfType(typeof(MethodInvokeExpr)).FindByShortName("new"));	
	paramOfObjectCreate.Add((inputs + paramOfObjectCreate).GetByAncs(ofNew));	

	CxList attributes = All.FindByShortName("*attributes");
	
	CxList methodInvoke = methods.FindByShortName("update_attributes");
	methodInvoke.Add(methods.FindByShortName("create"));
	methodInvoke.Add(methods.FindByShortName("build"));
	methodInvoke.Add(methods.FindByShortName("first_or_create"));
	methodInvoke.Add(methods.FindByShortName("first_or_initialize"));
	methodInvoke.Add(methods.FindByShortName("assign_attributes"));
	methodInvoke.Add(methods.FindByShortName("update"));
	methodInvoke.Add(methods.FindByShortName("update_attributes!"));
	methodInvoke.Add(methods.FindByShortName("create!"));
	methodInvoke.Add(methods.FindByShortName("first_or_create!"));
	methodInvoke.Add(methods.FindByShortName("first_or_initialize!"));

	CxList potentialMassAssign = (attributes + methodInvoke).GetTargetOfMembers();
		
	CxList attrAccess = All.FindByShortName("attr_accessible").GetByAncs(modelUnderModels);	
	
	ofNew = ofNew.GetTargetOfMembers();	
	CxList newAndPotential = potentialMassAssign + ofNew;
	CxList tempResult = All.NewCxList();
	foreach(CxList potential in newAndPotential)
	{		
		CSharpGraph model = potential.data.GetByIndex(0) as CSharpGraph;
		string name = model.ShortName;		
		CxList mod = modelUnderModels.FindByShortName(name, false) +
			modelUnderModels.FindByShortName(name.TrimEnd(new char []{'s'}), false);
		mod -= mod.GetClass(attrAccess.GetByClass(mod));
		
		if(mod.Count > 0)
		{
			tempResult.Add(potential);
							
		}
		
	}
	result.Add((tempResult.GetMembersOfTarget()).InfluencedByAndNotSanitized(inputs, sanitize)); 
	
	// only required when Strong Parameters is not in use
	if(sanitize.Count == 0){
		//this part handles a more difficult flow from inputs to potential vulnerability
		tempResult = All.NewCxList();
		CxList target = methodInvoke.GetTargetOfMembers().DataInfluencedBy(inputs);
		CxList left = All.FindByAssignmentSide(CxList.AssignmentSide.Left).FindByShortName(target).FindAllReferences(target);
		CxList classType = Find_DB_Out().GetByAncs(left.GetFathers()).GetTargetOfMembers();
		foreach(CxList potential in classType)
		{		
			CSharpGraph model = potential.data.GetByIndex(0) as CSharpGraph;
			string name = model.ShortName;		
			CxList mod = modelUnderModels.FindByShortName(name, false) +
				modelUnderModels.FindByShortName(name.TrimEnd(new char []{'s'}), false);
			mod -= mod.GetClass(attrAccess.GetByClass(mod));
			if(mod.Count > 0)
			{
				CxList ResultToConcat =	target.FindAllReferences(left.GetByAncs(potential.GetFathers()));//concatenate
					
				result.Add(potential.ConcatenateAllSources(ResultToConcat));						
			}	
		}
	}
	
	//the case that the assignment is done in the model class:
	CxList vulnerable = (methodInvoke + attributes).GetByAncs(modelUnderModels.GetClass(methodInvoke + attributes));
	CxList nonSanitizedVul = modelUnderModels.GetClass(vulnerable) - modelUnderModels.GetClass(attrAccess);
	vulnerable = vulnerable.GetByAncs(nonSanitizedVul);

	result.Add(vulnerable.InfluencedByAndNotSanitized(inputs, sanitize));	
}