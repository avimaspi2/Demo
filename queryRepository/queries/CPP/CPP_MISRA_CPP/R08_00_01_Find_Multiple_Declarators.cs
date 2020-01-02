/*
 MISRA CPP RULE 8-0-1
 ------------------------------
 This query searches for multiple declarations in the same row.

 The Example below shows code with vulnerability: 

      int main() {
		    int i,j; 	//Non-compliant
		}

*/

CxList decs = Find_All_Declarators();
SortedList lps = new SortedList(new Checkmarx.DataCollections.PragmaComparer());
foreach(CxList dec in decs) {
	LinePragma lp = (LinePragma) ((Declarator) dec.data.GetByIndex(0)).LinePragma;
	if (!lps.Contains(lp)) {
		lps.Add(lp, null);
	}
}

decs = All.FindByPositions(lps, 0, true);
decs = All.FindByRegex(@",\s*[_A-Za-z]", false, false,false) * decs;
decs -= decs.FindByType(typeof(CharLiteral)) + decs.FindByType(typeof(IntegerLiteral)) +
	decs.FindByType(typeof(StringLiteral)) + decs.FindByType(typeof(Param))
	+ decs.FindByType(typeof(ArrayInitializer));
result = decs;