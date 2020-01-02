/* MISRA CPP RULE 6-2-1   
 ------------------------------
 This query finds assignment operators that are used in sub- expressions
 

 The Example below shows code with vulnerability:  
 
      x=y;             //compliant
      x=y=z;           //non-compliant
      if(x!=y)         //compliant 
      {
          some code 
      }
    
      bool b1=x!=y;    //compliant
      if(x=y)          //non-compliant 
      {
          some code 
      }

      if((x=y)!=0)     //non-compliant
      {
        some code 
      }

      if(int i=foo())  //compalint
      {
        some code 
      }
*/

//for multiple assignment x=y=z
CxList exprStsmt = All.FindByType(typeof(ExprStmt));
CxList unrf = All.FindByType(typeof(UnknownReference));
CxList asn = All.FindByType(typeof(AssignExpr));
result.Add(asn.FindByFathers(asn.FindByFathers(exprStsmt)));
//for if statements if(x=y)
CxList ifs = All.FindByType(typeof(IfStmt));
unrf = unrf.GetByAncs(ifs);
CxList exp = All.FindByType(typeof(Expression));
CxList ifsChildren= unrf.GetByAncs(exp.FindByFathers(ifs));
result.Add(ifsChildren.FindByAssignmentSide(CxList.AssignmentSide.Right) * unrf);