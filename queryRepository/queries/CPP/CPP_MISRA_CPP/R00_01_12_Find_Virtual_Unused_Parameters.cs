/*
 MISRA CPP RULE 0-1-12
 ------------------------------
 This query searches for unused parameters in virtual functions and 
 all of their overrides. 
 
 The Example below shows code with vulnerability: 

      class A
		{
		public:
			virtual void unused (int * para1,
								int unusedpara) = 0; //Non-compliant
			virtual void used (int * para1,
								int & para2) = 0;
		};
		class B1: public A
		{
	    public:
          virtual void unused (int * para1,
					            int unusedpara) //Non-compliant
          {
	          *para1 = 1U;
          }
          virtual void used (int * para1,
					            int & para2) 
          {
            *para1 = 1U;
          }
		};
		class B2: public A
		{
	    public:
          virtual void unused (int * para1,
					            int unusedpara) //Non-compliant
          {
	          *para1 = 1U;
          }
          virtual void used (int * para1,
					            int & para2)
          {
            para2 = 0;
          }
		};

*/

CxList virtMethods = All.FindByType(typeof(MethodDecl)).FindByFieldAttributes(Modifiers.Virtual);
CxList unCheckedMethods = virtMethods;

CxList classes = All.FindByType(typeof(ClassDecl));
classes = virtMethods.GetClass(classes);
CxList allParams = All.FindByType(typeof(ParamDecl));
allParams = allParams.GetParameters(virtMethods);
CxList typerefs = All.FindByType(typeof(TypeRef));
typerefs = typerefs.GetByAncs(allParams);

foreach(CxList curr in virtMethods) {
	if ((virtMethods - unCheckedMethods).FindByShortName(curr).Count > 0) {
		continue;
	}
	unCheckedMethods -= curr;
	CxList overriden = curr;
	CxList sons = classes.InheritsFrom(classes.GetClass(curr));
	CxList others = unCheckedMethods.FindByShortName(curr);
		
	CxList currParams = allParams.GetParameters(curr);
	CxList currTyperefs = typerefs.GetByAncs(currParams);
	bool isOverride = true;
	foreach(CxList other in others) {
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
			unCheckedMethods -= other;
			overriden.Add(other);
		}
	}//end inner foreach
	CxList otherParams = allParams.GetParameters(overriden);
	others.Add(curr);
	//Start checking for curr's unused parameters 
	for (int i = 0; i < currParams.Count; i++) 
	{
		CxList currParam = otherParams.GetParameters(others, i);
		if( All.FindAllReferences(currParam).Count == currParam.Count ) {
			result.Add(currParam);
		}
	}
}//end outer foreach