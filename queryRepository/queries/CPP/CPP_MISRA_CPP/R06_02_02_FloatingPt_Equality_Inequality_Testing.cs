/* MISRA CPP RULE 6-2-2
 ------------------------------
 This query finds all floating point expressions that are tested for equality or inequality

 The following example shows what main code should look like: 


     float x,y;
	 if(x==y)          //non-compliant
     if(x!=0.0f)       //non-compliant
     if(x<0)           //non-compliant
     if(0.01f==0.01f)  //non-compliant	
     
     x=0.08f;
	   float *z=&x;
     if(*z>=y)         //non-compliant
     
     float * foo()
     { 
         some code
     }

     float bar()
     {
         some code
     } 
     if(*foo()==bar())  //non-compliant    

*/


//get all float typedefs: typedef float flt
CxList tr = All.FindByType(typeof(TypeRef));
CxList unrf = All.FindByType(typeof(UnknownReference));
CxList methodInv = All.FindByType(typeof(MethodInvokeExpr));

CxList typedefs = All.FindByName("CX_TYPEDEF").FindByType(typeof(StringLiteral));
CxList floatTypeDef = tr.GetByAncs(typedefs.GetAncOfType(typeof(FieldDecl))).FindByShortName("float");
CxList newName = floatTypeDef.GetAncOfType(typeof(FieldDecl));
CxList typedefDefinitions = All.NewCxList();
foreach(CxList nn in newName)
{
	CSharpGraph name = nn.data.GetByIndex(0) as CSharpGraph;
	if(name != null)
	{
		typedefDefinitions.Add(tr.FindByShortName(name.ShortName));
	}
}

//now we want to filter out all pointers from all non pointers
CxList allFloats = tr.FindByShortName("float") + typedefDefinitions;
CxList floatPointers = allFloats.FindByRegex(@"[^(]\s*\*\s*\w");
CxList fathers = floatPointers.GetFathers().FindByType(typeof(FieldDecl)) +
	floatPointers.GetFathers().FindByType(typeof(VariableDeclStmt));
CxList toRemove = tr.GetByAncs(fathers);

CxList floatNonPtrs = allFloats - floatPointers - toRemove;

//let's find all references to those
CxList floatVarNP = floatNonPtrs.GetFathers();
floatVarNP = floatVarNP.FindByType(typeof(ParamDecl)) +
	floatVarNP.FindByType(typeof(MethodDecl));
CxList avoidAllKids = floatNonPtrs.GetFathers() - All.FindByType(typeof(MethodDecl));

CxList toFindNP = Find_All_Declarators().GetByAncs(avoidAllKids) + floatVarNP;
CxList potentialNP = (unrf + methodInv).FindAllReferences(toFindNP);

CxList floatVarP = floatPointers.GetFathers();
floatVarP = floatVarP.FindByType(typeof(ParamDecl)) +
	floatVarP.FindByType(typeof(MethodDecl));
avoidAllKids = floatPointers.GetFathers() - All.FindByType(typeof(MethodDecl));
CxList toFindP = Find_All_Declarators().GetByAncs(avoidAllKids) + floatVarP;
CxList potentialP = (unrf + methodInv).FindAllReferences(toFindP);

potentialP -= potentialP.GetByAncs(potentialP.GetAncOfType(typeof(Param)));
potentialNP -= potentialNP.GetByAncs(potentialNP.GetAncOfType(typeof(Param)));
//now potentialP and potentialNP contains all references for us to test

CxList expressions = potentialNP.GetFathers();
CxList unaryEx = expressions.FindByType(typeof(UnaryExpr));

unaryEx = unaryEx.FindByShortName("Increment") + unaryEx.FindByShortName("Decrement");
expressions.Add(unaryEx.GetFathers());
CxList expressionsNP = potentialP.GetFathers().FindByType(typeof(UnaryExpr));
expressions.Add(expressionsNP.GetFathers());

CxList realNum = All.FindByType(typeof(RealLiteral));
realNum = realNum.FindByRegex(@"\df");
expressions.Add(realNum.GetFathers());

result.Add(expressions.GetByBinaryOperator(BinaryOperator.GreaterThan) +
	expressions.GetByBinaryOperator(BinaryOperator.LessThan) +
	expressions.GetByBinaryOperator(BinaryOperator.IdentityEquality) +
	expressions.GetByBinaryOperator(BinaryOperator.IdentityInequality) +
	expressions.GetByBinaryOperator(BinaryOperator.LessThanOrEqual) +
	expressions.GetByBinaryOperator(BinaryOperator.GreaterThanOrEqual));