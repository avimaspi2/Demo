/*
MISRA C RULE 15-3
------------------------------
This query searches switches where the final case is not "Default"

	The Example below shows code with vulnerability: 

switch ( defaults )
   {
      case 1U:
      {
         use_uint16 ( defaults );
         break;
      }
      default:                       
      {
		 use_uint16(defaults);
         break;
      }
      case 2U:
      {
         use_uint16 ( defaults );
         break;
      }
   }


*/

CxList switches = All.FindByType(typeof(SwitchStmt));
CxList cases = All.FindByType(typeof(Case));

// go over switches, add switches with no default, or default is a middle case
foreach (CxList sw in switches){
	CxList sonCases = cases.FindByFathers(sw);
	bool foundDefault = false;
	
	// search cases until default is found
	foreach (CxList curCase in sonCases){
		Case myCase = curCase.data.GetByIndex(0) as Case;
		if (foundDefault){
			
			// A case exists after the default, we want to add this switch
			foundDefault = false;
			break;
		}
		
		if (myCase.IsDefault){
			foundDefault = true;
		}
	}
	
	if (!foundDefault)
		result.Add(sw);
}