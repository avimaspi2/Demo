/* MISRA CPP RULE 15-0-3
 ------------------------------
 This query checks if there is a goto label inside a try catch statement

 The Example below shows code with vulnerability: 

      
       void f()
       {   
             goto Label1;
             try
             {
                  Label1:       //non-compliant
                  some code
             }
             catch(...)
             {
               some code 				
             }
       }
       
*/


//finds all declarations
CxList decl= Find_All_Declarators();
//finds goto
CxList  allGoTos=All.FindByFathers(decl).FindByName("goto");
CxList labels=allGoTos.GetFathers();
CxList labelCalls = All.FindByType(typeof(LabeledStmt));
CxList goToLabel = All.NewCxList();
foreach (CxList cur in labels)
{	
	foreach(CxList lab in labelCalls)
	{
		
		LabeledStmt ls = lab.data.GetByIndex(0) as LabeledStmt;
		Declarator d = cur.data.GetByIndex(0) as Declarator;
		if(ls != null && d != null)
		{
			if((ls.Label) == (d.Name))
			{
				goToLabel.Add(All.FindById(ls.NodeId));
			}
		}
	}	
}
CxList tryStmt = goToLabel.GetAncOfType(typeof(TryCatchFinallyStmt));
CxList inTryCatch = All.GetByAncs(tryStmt).FindByType(typeof(LabeledStmt));
result.Add(inTryCatch);