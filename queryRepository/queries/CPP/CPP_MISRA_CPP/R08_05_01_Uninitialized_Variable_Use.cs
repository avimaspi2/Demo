/* MISRA CPP RULE 8-5-1
 ------------------------------
 This query finds all variables that are used before they are assigned a value

void bar(int p)
{
     int  a;  //non-compliant
     cout<<a;
     int j;   //non-compliant
	 foo(j);       
}

public:
       int z;
       int x;
    
       A()    //non-compliant neither z or x are initialized 
       {  
           z+=10;   
       }
       A(int p):z(p) //non-compliant - x is not initialized 
       {   
             
       }      
};

*/
result = Find_Use_Of_Uninitialized_Var();