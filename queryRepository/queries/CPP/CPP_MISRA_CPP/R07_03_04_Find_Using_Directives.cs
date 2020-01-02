/*
 MISRA CPP RULE 7-3-4
 ------------------------------
 This query searches for using-directives usage.

 The Example below shows code with vulnerability: 

        namespace NS1{
          int i1;
		}
		namespace NS2{
	        int i2;
		}
		namespace NS4{
          int i2;
          namespace NS5 {
                    int i3;
          };
		}
		using namespace NS1; 		//Non-compliant
		using NS2::i2; 				//Compliant
		using namespace NS4::NS5; 	//Non-compliant

*/

CxList imports = All.FindByType(typeof(Import));
result = imports.FindByRegex(@"using namespace\s", false, false,false);