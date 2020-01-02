/*
 MISRA CPP RULE 10-3-3
 ------------------------------
 This query searches for functions that were introduced as pure, then defined, then re-declared
 as pure.
 
 The Example below shows code with vulnerability: 

      class Foo
		{
		public:
			virtual void a() = 0;
		};

		class Boo : public Foo
		{
		public:
		    virtual void a() { }
		};

		class Bar : public Boo
		{
		public:
		    virtual void a() = 0; //Non-compliant
		};

*/

//Find abstract functions.
CxList absFuncs = All.FindByType(typeof(MethodDecl));
CxList unChecked = absFuncs;
absFuncs = absFuncs.FindByFieldAttributes(Modifiers.Abstract);

CxList classes = All.FindByType(typeof(ClassDecl));
classes = classes.GetClass(unChecked);
CxList allParams = All.FindByType(typeof(ParamDecl));
allParams = allParams.GetParameters(unChecked);
CxList typerefs = All.FindByType(typeof(TypeRef));
typerefs = typerefs.GetByAncs(allParams);

foreach (CxList curr in absFuncs) {
	
	if ( (unChecked * curr).Count == 0 ){
		continue;	
	}
	unChecked -= curr;
	CxList sons = classes.InheritsFrom(classes.GetClass(curr));
	//Get other functions with same name - divide into abstract and non-abstract.
	CxList others = unChecked.FindByShortName(curr);

	CxList currParams = allParams.GetParameters(curr);
	CxList currTyperefs = typerefs.GetByAncs(currParams);
	CxList overriden = All.NewCxList();
	CxList overridenPure = All.NewCxList();
	bool isOverride = true;
	foreach(CxList other in others) 
	{
		CxList otherTypeRefs = allParams.GetParameters(other);
		otherTypeRefs = typerefs.GetByAncs(otherTypeRefs);
		//Check if otherMethod overrides currMethod.
		if(sons.FindByShortName(classes.GetClass(other)).Count == 1 &&
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
			if(other.FindByFieldAttributes(Modifiers.Abstract).Count == 1) {
				overridenPure.Add(other);
				unChecked -= other;
			}
			else {
				overriden.Add(other);
			}
		}
	}//end inner foreach
	
	CxList grandsons = classes.InheritsFrom(classes.GetClass(overriden));
	foreach (CxList absOther in overridenPure) {
		if (grandsons.FindByName(classes.GetClass(absOther)).Count == 1) {
			result.Add(absOther);
		}
	}
}