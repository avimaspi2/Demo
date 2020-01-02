/* MISRA CPP RULE 15-3-3
 ------------------------------
 This query finds all statememnts of accessing a non static member of class in the "catch" part of the 
 Constructor and Destructor.
 

 The Example below shows code with vulnerability:  
 
       class C
       { 
           public:
              int x;
              static int y;
           C()
           {    
                try{ 
                     //action that may raise an exception
                }
                catch(...)
                {
                      if(x==0)   //non-compliant-x may not exist  at this point
                      {  
                         //action dependent on value of x
                      }
                }  
           }
           ~C()
           {
                try 
                {
						 //action that may raise an exception
                }
                catch(...)
                {
                   if(foo())  //non-compliant- foo access to members that do not exist at this point
                   {
                        //action dependent on value of x
                   }                     
                }
            }
            bool foo()
            {
                if(x==9)
                {
                    return true;
                }
                return false; 
            }
        };
*/


//finds all references to non static fields inside a constructor's or destructor's catch statement:
if(param.Length == 0){
	CxList classDec=All.FindByType(typeof(ClassDecl));
	CxList methDecl = All.FindByType(typeof(MethodDecl));
	CxList unrf = All.FindByType(typeof(UnknownReference));
	CxList macc = All.FindByType(typeof(MemberAccess));
	CxList methInvExp = All.FindByType(typeof(MethodInvokeExpr));
	CxList classParams = All.FindByType(typeof(FieldDecl));
	Modifiers mod = new Modifiers();
	mod = Dom.Modifiers.Static;
	CxList stMod = All.FindByFieldAttributes(mod);
	classParams -= stMod;
	foreach (CxList cur in classParams)
	{
		CxList myClass = classDec.GetClass(cur);
		CSharpGraph d = myClass.data.GetByIndex(0) as CSharpGraph;
		if (d!=null && d.ShortName!=null && d.ShortName.StartsWith("checkmarx") )
		{
			classParams -= cur;
		}
	}
	CxList ctrDtr = All.FindByType(typeof(ConstructorDecl)) + All.FindByType(typeof(DestructorDecl));
	CxList catchExpr = All.FindByName("catch").GetByAncs(ctrDtr);
	CxList catchKids = unrf.GetByAncs(catchExpr);
	CxList refInCatch = catchKids.FindAllReferences(classParams);	
	CxList findRef = methInvExp.GetByAncs(catchExpr).FindAllReferences(methDecl) - methDecl;
	
	if(findRef.Count > 0)
	{
		//creating a visited list
		CxList marked = All.NewCxList();
		//recurstion:
		result = R15_03_03_Accessing_Non_Static_Mem_In_Ctr_Dtr(classParams, findRef, marked, result, methDecl, unrf, macc, methInvExp) + refInCatch;
	}
}
//enter the recursion
if(param.Length > 3){
	CxList inv = (CxList) param[1];
	CxList nonStatField = (CxList) param[0];
	CxList marked = (CxList) param[2];
	CxList res = (CxList) param[3];
	CxList methDecl = (CxList) param[4];
	CxList unrf = (CxList) param[5];
	CxList macc = (CxList) param[6];
	CxList methInvExp = (CxList) param[7];
	CxList newInvList = All.NewCxList();
	//avoid circling around
	foreach (CxList item in inv)
	{
		bool found = false;
		foreach(CxList done in marked)
		{
			if(item == done){
				found = true;
				break;
			}
		}
		if(!found)
		{
			newInvList.Add(item);
		}
	}
	if(newInvList.Count > 0)
	{	

		foreach (CxList item in newInvList)
		{
			marked.Add(item);
		}
		//adding non static references that are accessed in methods from the constructor or destructor

		CxList MethodDec = methDecl.FindAllReferences(newInvList);
	
		CxList nonStat = unrf.GetByAncs(MethodDec).FindAllReferences(nonStatField) +
			macc.GetByAncs(MethodDec).FindAllReferences(nonStatField);
		res.Add(nonStat);
		CxList findRef = methInvExp.GetByAncs(MethodDec).FindAllReferences(methDecl) - methDecl - macc;

		result = R15_03_03_Accessing_Non_Static_Mem_In_Ctr_Dtr(nonStatField, findRef, marked, res, methDecl, unrf, macc, methInvExp) + res;
	}
}