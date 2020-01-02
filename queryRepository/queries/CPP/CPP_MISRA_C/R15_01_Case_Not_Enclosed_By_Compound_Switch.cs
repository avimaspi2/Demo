/*
MISRA C RULE 15-1
------------------------------
This query searches for instances of case not directly wrapped by a compound switch

	The Example below shows code with vulnerability: 

switch (i)
   {
      case 1:
      {
         case 2:                              
         {
            break;
         }
         break;        
      }
   }

*/ 
	
CxList switches = All.FindByType(typeof(SwitchStmt));

// switches with compound statements
CxList properSwitches = switches.FindByRegex(@"\Wswitch\s*\([^\)]*?\)[^;\{]*?\{", false, false, false);

// return cases which belong to a non compound switch
CxList cases = All.FindByType(typeof(Case));

result = All.FindByRegex(@"\Wcase\W", false, false, false, All.NewCxList()) - cases.FindByFathers(properSwitches);