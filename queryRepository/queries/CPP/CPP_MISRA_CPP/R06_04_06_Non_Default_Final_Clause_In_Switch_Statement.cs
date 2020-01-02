/*
MISRA CPP RULE 6-4-6
--------------------------------
This query searches switches where the final case is not "default", or if all enum types were called as cases.


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

CxList enumMembers = All.FindByType(typeof(EnumMemberDecl));
CxList enumTypes = All.GetClass(enumMembers);
CxList enumTypeRefs = All.FindByType(typeof(TypeRef)).FindByName(enumTypes);
CxList enumInst = enumTypeRefs.GetAncOfType(typeof(Declarator));

CxList switches = All.FindByType(typeof(SwitchStmt));
CxList cases = All.FindByType(typeof(Case));
CxList conditions = All.FindByFathers(switches) - cases;
// Get only relevant conditions and their declarators
conditions = conditions.FindByName(enumInst);
enumInst = enumInst.FindByName(conditions);
enumTypeRefs = enumTypeRefs.GetByAncs(enumInst);
enumTypes = enumTypes.FindByName(enumTypeRefs);
enumMembers = enumMembers.GetByClass(enumTypes);

CxList enumSwitches = conditions.GetFathers();
switches -= enumSwitches;

foreach (CxList sw in enumSwitches) {
	CxList currEnum = enumTypeRefs.GetByAncs(enumInst.FindByName(conditions.FindByFathers(sw)));
	currEnum = enumMembers.GetByClass(enumTypes.FindByName(currEnum));
	CxList sonCases = cases.FindByFathers(sw);
	if (currEnum.Count > sonCases.Count) {
		switches.Add(sw);
	}
}

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