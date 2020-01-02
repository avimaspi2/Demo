// Default Android Builtin Activity classes
List < string > classes = new List<string>{"Activity","AppCompatActivity"};

// allow to add more classes Names using parameter
if(param.Length > 0 && param[0] is IEnumerable<string>){
	IEnumerable< string > externalClassesName = param[0] as IEnumerable< string >;
	classes.AddRange(externalClassesName);
}

CxList listClass = Find_ClassDecl();
CxList typeRefsCollection = Find_TypeRefCollection();
CxList typeRefs = Find_TypeRef();
CxList methods = Find_Methods();

CxList onCreateMethod = Find_MethodDecls().FindByShortName("onCreate");
CxList onCreateMethodInvokesOnBase = methods.FindByShortName("onCreate").GetTargetOfMembers().FindByType(typeof(BaseRef));

CxList setFlagsMethod = methods.FindByShortName("setFlags");
CxList flagSecure = Find_Param().FindByName("*.LayoutParams.FLAG_SECURE").GetByAncs(setFlagsMethod);

CxList activityCreate = All.NewCxList();
foreach (string className in classes)
{
	activityCreate.Add(listClass.InheritsFrom(className));
}

Func <CxList , CxList> checkClass = null;
	
checkClass = (parent) => {
	
	CxList res = All.NewCxList();
	CxList stmt = onCreateMethod.GetByAncs(parent);	
	CxList secureStmt = flagSecure.GetByAncs(stmt);		

	
	if (secureStmt.Count == 0)
	{	
		CxList superOnCreates = onCreateMethodInvokesOnBase.GetByAncs(parent);		
		if(superOnCreates.Count == 0)
		{
			res.Add(onCreateMethod.GetMethod(superOnCreates));
		}
		else
		{
			//need to check the super!
			CxList filteredListClass = listClass.FindByShortName(parent.GetName());
			CxList InheriList = typeRefsCollection.FindByFathers(filteredListClass);
			CxList inheritTypes = typeRefs.FindByFathers(InheriList);			
			CxList classdef = listClass.FindDefinition(inheritTypes);
			
			CxList tmpCxList = All.NewCxList();
			
			foreach (CxList def in classdef){
				tmpCxList.Add(checkClass(def));
			}
			
			if (tmpCxList.Count > 0 || classdef.Count != inheritTypes.Count){
				//If you can't find definitions of all superclasses assume the worst			
				res.Add(stmt);
			}		
		}
	}
	return res;
};

foreach (CxList activityClass in activityCreate){
	result.Add(checkClass(activityClass));
}