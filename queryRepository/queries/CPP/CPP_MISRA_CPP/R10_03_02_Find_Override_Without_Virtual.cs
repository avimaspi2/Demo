/*
 MISRA CPP RULE 10-3-2
 ------------------------------
 This query searches for methods that override virtual methods, but are not declared 
 with the virtual keyword.

 The Example below shows code with vulnerability: 

      class Foo
			{
			public:
				virtual void DoSomething() { }
			};
		class Bar : public Foo
			{
			public:
			    void DoSomething(); //Non-compliant
			};

*/

//Find virtual methods.
CxList virtMethod = All.FindByType(typeof(MethodDecl));
virtMethod = virtMethod.FindByFieldAttributes(Modifiers.Virtual);

//Find methods that may be problematic.
CxList protoMethods = All.FindByType(typeof(MethodDecl));
protoMethods -= All.FindByFathers(protoMethods).FindByType(typeof(StatementCollection)).GetFathers();
protoMethods -= virtMethod;
protoMethods = protoMethods.FindByShortName(virtMethod);

CxList virtClasses = All.FindByType(typeof(ClassDecl)).GetClass(virtMethod);
CxList protoClasses = All.FindByType(typeof(ClassDecl)).InheritsFrom(virtMethod);
protoClasses = All.FindByType(typeof(ClassDecl));//protoClasses.GetClass(protoMethods);

CxList virtParams = All.FindByType(typeof(ParamDecl));
CxList protoParams = virtParams.GetParameters(protoMethods);
virtParams = virtParams.GetParameters(virtMethod);

CxList virtTyperefs = All.FindByType(typeof(TypeRef)).GetByAncs(virtParams);
CxList protoTyperefs = All.FindByType(typeof(TypeRef)).GetByAncs(protoParams);

foreach (CxList curr in virtMethod) {
	CxList sons = protoClasses.InheritsFrom(virtClasses.GetClass(curr));//Get classes that inherent from curr's class.
	//Get methods with same name that aren't virtual.
	CxList others = protoMethods.FindByShortName(curr);
	others = others.GetByClass(sons);
	CxList currParams = virtParams.GetParameters(curr);
	CxList currTyperefs = virtTyperefs.GetByAncs(currParams);
	bool isOverride = true;
	foreach(CxList other in others) 
	{
		CxList otherTypeRefs = protoParams.GetParameters(other);
		otherTypeRefs = protoTyperefs.GetByAncs(otherTypeRefs);
		//Check if otherMethod overrides currMethod.
		if(sons.FindByShortName(protoClasses.GetClass(other)).Count == 1 &&
			currTyperefs.Count == otherTypeRefs.Count) 
		{
			for(int i = 0; i < currTyperefs.Count; i++) 
			{
				string cName = ((TypeRef) currTyperefs.data.GetByIndex(i)).TypeName;
				string oName = ((TypeRef) otherTypeRefs.data.GetByIndex(i)).TypeName;
				if(!cName.Equals(oName)) 
				{
					isOverride = false;	
					break;
				}
			}//end for
		}//end if
		else 
		{
			isOverride = false;
		}
		if (isOverride) 
		{
			result.Add(other.Concatenate(curr));
			protoMethods -= other;
			protoTyperefs -= otherTypeRefs;
		}
	}
}