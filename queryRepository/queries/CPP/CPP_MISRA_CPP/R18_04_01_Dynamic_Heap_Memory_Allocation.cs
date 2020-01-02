/*
 MISRA CPP RULE 18-4-1
 ------------------
  Dynamic heap memory allocation shall not be used.

  The Example below shows code with vulnerability: 

  			void foo(){          
 	 				int * i = new int; 	//Non-compliant
 	 				delete i; 			//Non-compliant  
 				}

*/

result = Find_Memory_Allocation() +
	All.FindByType(typeof(ObjectCreateExpr)).FindByRegex("new");