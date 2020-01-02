/* MISRA CPP RULE 6-6-3
 ------------------------------
 This query finds out if the continue statement is used not in a well formed loop - a well formed loop is defined
 by rules 6-5-1 to 6-5-6
 The following example shows what main code should look like: 
     

  	for (s=9; s<100; s=s+1)
    {
     continue;   //non-compliant - rule 6-5-4  
    }
	while(condition)
    {
        continue; //non compliant- not inside for 
    }

*/

//finds all continues that are outside of Fors.
CxList allContinues = All.FindByType(typeof(ContinueStmt));
CxList allContinueLoops = allContinues.GetAncOfType(typeof(IterationStmt));
CxList helper = allContinueLoops;
foreach(CxList allf in allContinueLoops)
{
	IterationStmt i = allf.data.GetByIndex(0) as IterationStmt;
	if(i != null)
	{
		IterationType it = i.IterationType;
		if(!it.ToString().Equals("For"))
		{
			helper -= allf;
		}
	}
}
allContinueLoops = helper;
CxList contInFor = allContinues.GetByAncs(allContinueLoops);
result.Add(allContinues - contInFor);

//now check if the for is illegal.

CxList illegalFors = (R06_05_02_Loop_Counter_Modify() +
	R06_05_03_Change_Lc_In_St_And_Cond() + R06_05_05_Lcv_Change_In_For_Stmt()).GetAncOfType(typeof(IterationStmt))
	+ R06_05_04_Incremental_Modified() 
	+ R06_05_06_Bool_Lcv_Change()
	+ R06_05_01_Single_Non_Float_LC();


CxList temp = All.NewCxList();


foreach(CxList cur in allContinueLoops)
{
	CSharpGraph cl = cur.data.GetByIndex(0) as CSharpGraph;
	if(cl != null)
	{
		temp.Add(illegalFors.FindById(cl.NodeId));
	}
}
result.Add(contInFor.GetByAncs(temp));