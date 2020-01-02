/* MISRA CPP RULE 15-0-2
 ------------------------------
 This query finds all throw statements that throw pointers (excluding function pointers)
 
 The Example below shows code with vulnerability:     
		
	 class B{};
     class C
     {
		 B* b;
		 C()
		 {
		    throw &b;                //non-compliant
		 }
		 C(int a)
		 {
		    throw(new B());          //non-compliant  
		    throw b;                 //non-compliant        
		 }		
	 };
		
	 void bar(char * a, int ** c)
     {
		 throw a;                     //non-compliant
		 throw c;                     //non-compliant
	 }
	 int * foo(int a)
     {
		 return &a;                 
	 }
		
	 int main()
     {
		 throw (foo('s'));            //non-compliant
	 }
		
*/

//handle explicit pointers
//finds all pointer declarations
CxList ptrDecl = Find_All_Declarators();
ptrDecl = ptrDecl.FindByRegex(@"[^(]\s*(\*\s*)+\w");
CxList decl = All.FindByType(typeof(ParamDecl));

//finds all method params that are pointers
//for example void foo(int* a){}
CxList tr = All.FindByFathers(decl).FindByType(typeof(TypeRef));
CxList ptrParam = tr.FindByRegex(@"([(]\s*\w+\s*(\*\s*)+\s*\w+)|(,\s*\w+\s*(\*\s*)+\s*\w+)");
CxList ptrAsMethodParam=ptrParam.GetAncOfType(typeof(ParamDecl))+ptrDecl;

//finds all throw associated to the pointers
CxList refToPtr = All.FindAllReferences(ptrAsMethodParam) - ptrAsMethodParam;
CxList throwWithExplicit= refToPtr.GetAncOfType(typeof(ThrowStmt));
CxList throwParams= All.FindByFathers(throwWithExplicit);
CxList ptrAccess = throwParams - throwParams.FindByType(typeof(UnaryExpr));
result=ptrAccess + throwParams.FindByType(typeof(UnaryExpr)).FindByName("Address");

//throw new object
CxList throwObj = All.FindByType(typeof(ThrowStmt));
throwObj=All.FindByFathers(throwObj).FindByType(typeof(ObjectCreateExpr));
result.Add(throwObj);

//handle byAddress pointers
CxList declarators = Find_All_Declarators() - ptrDecl;
CxList paramNotPtr = decl - ptrAsMethodParam + declarators;
CxList refToParam = All.FindAllReferences(paramNotPtr) - paramNotPtr;
CxList throwWithParams = refToParam.GetAncOfType(typeof(ThrowStmt));
CxList throwParamsImplicit1 = All.FindByFathers(throwWithParams);
result.Add(throwParamsImplicit1.FindByType(typeof(UnaryExpr)).FindByName("Address"));

//throw a returned pointer
CxList throwStmt = All.FindByType(typeof(ThrowStmt));
foreach(CxList cur in throwStmt){
	CxList throwCallsMeth = All.FindByFathers(cur).FindByType(typeof(MethodInvokeExpr));
	CxList method = All.FindDefinition(throwCallsMeth);
	CxList retType = All.FindByFathers(method).FindByType(typeof(TypeRef));
	CxList temp = retType.FindByRegex(@"[^(]\s*\*\s*\w");
	if(temp.Count > 0)
	{
		result.Add(cur);
	}
		
}