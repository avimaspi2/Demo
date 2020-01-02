/*
 MISRA CPP RULE 0-1-11
 ------------------------------
 This query searches for unused parameters in non-virtual functions. Where unnamed parameters in callback
 functions are tolerated.
 
 The Example below shows code with vulnerability: 

      typedef int ( * CallbackFn) (int a, int b);
		int Callback_1(int a, int b) { //Compliant
			return a + b;
		}
		int Callback_2(int a, int b) { //Non-compliant
			return a;
		}
		int Callback_3(int, int b) {   //Compliant by exception
			return b;
		}
		void Dispatch (int n,
	    			   int a,
					   int b,
					   int c, //Non-compliant
					   int) { //Non-compliant if Dispatch is not a callback.
			CallbackFn pFn;
			switch (n) {
				case 0: pFn = &Callback_1; break;
				case 1: pFn = &Callback_2; break;
				default: pFn = &Callback_3; break;                        
			}
			( *pFn)(a, b);
		}

*/

CxList methods = All.FindByType(typeof(MethodDecl));
methods -= methods.FindByFieldAttributes(Modifiers.Virtual);
//Remove prototype methods
methods = All.FindByFathers(methods).FindByType(typeof(StatementCollection)).GetFathers();

CxList methParams = All.GetParameters(methods);
//Remove used parameters
methParams -= All.FindDefinition(All.FindAllReferences(methParams) - methParams);
methParams -= methParams.FindByShortName(""); //remove (void) type params

//Find callback methods
methods = methParams.GetAncOfType(typeof(MethodDecl));
CxList allRefs = All.FindAllReferences(methods) - methods;
CxList addressedMethodRefs = All.FindAllReferences(methods).GetFathers().FindByType(typeof(UnaryExpr));
addressedMethodRefs = All.FindByFathers(addressedMethodRefs.FindByName("Address"));

foreach (CxList curr in methods) {
	CxList refs = allRefs.FindAllReferences(curr);
	CxList cut = refs * addressedMethodRefs;
	if (cut.Count > 0 ) {//curr is a callback method, find unnamed params
		CxList currParams = methParams.GetParameters(curr);
		methParams -= currParams.FindByShortName("");
	}
}
result = methParams;