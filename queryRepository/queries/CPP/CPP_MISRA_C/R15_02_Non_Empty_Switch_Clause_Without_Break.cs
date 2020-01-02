/*
MISRA C RULE 15-2
------------------------------
This query searches for non empty switch clauses (cases) that don't contain a break statement;

	The Example below shows code with vulnerability: 

switch (i)
{
      case 1:
      {
         i++;
	  }                     
	  default:
      {
		 i=3;
		 break;
      }
}

*/

// find cases with non case statements inside them
CxList cases = All.FindByType(typeof(Case));
CxList nonCases = All - cases;
CxList nonEmptyCases = nonCases.GetAncOfType(typeof(Case));

// remove those with a break statement directly inside them
CxList casesWithBreak = cases * All.FindByType(typeof(BreakStmt)).GetFathers().GetFathers();

result = nonEmptyCases - casesWithBreak;