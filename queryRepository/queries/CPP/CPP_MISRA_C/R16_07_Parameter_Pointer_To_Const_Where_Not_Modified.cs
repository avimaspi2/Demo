/*
MISRA C RULE 16-7
------------------------------
This query searches for aramaters in function definitions where param is pointer and not const pointer,
 but the pointer target is not modified within the function body

	The Example below shows code with vulnerability: 

bool_t foo(int *i) 
{
	*i = *i * 2;
	return 0;
}

*/

// ignore function declarations and main
CxList functionDefinitions = All.FindByType(typeof(StatementCollection)).GetFathers().FindByType(typeof(MethodDecl));
functionDefinitions -= functionDefinitions.FindByShortName("main");
// start with all pointer paramaters
CxList definitionParams = All.FindByType(typeof(ParamDecl)).FindByFathers(All.FindByFathers(functionDefinitions));
CxList pointerParams = definitionParams.FindByRegex(@"\w\s*?\*",false,false,false);

// remove pointers to const
pointerParams -= pointerParams.FindByRegex(@"(?:const(\s*\w+)?\s*\*)\w", false, false, false);

// now only keep those whose data is changed
CxList changedPointerDataRef = All.NewCxList();
CxList pointerParamDataRef = All.FindAllReferences(pointerParams).GetFathers().FindByType(typeof(UnaryExpr)).FindByName("Pointer");

// data ref direct assigments
changedPointerDataRef.Add(pointerParamDataRef.FindByAssignmentSide(CxList.AssignmentSide.Left));

// data ref postfix/prefix increment/decrement
CxList preIncDec = All.NewCxList();
foreach (CxList cur in All.FindByType(typeof(UnaryExpr))){
	UnaryOperator curOp = ((UnaryExpr) cur.data.GetByIndex(0)).Operator;
	if ((curOp == UnaryOperator.Increment) || (curOp == UnaryOperator.Decrement)){
		preIncDec.Add(All.FindById(((UnaryExpr) cur.data.GetByIndex(0)).Right.NodeId));
	}
}
CxList postIncDec = All.NewCxList();
foreach (CxList cur in All.FindByType(typeof(PostfixExpr))){
	PostfixOperator curOp = ((PostfixExpr) cur.data.GetByIndex(0)).Operator;
	if ((curOp == PostfixOperator.Increment) || (curOp == PostfixOperator.Decrement)){
		if (((PostfixExpr) cur.data.GetByIndex(0)).Left != null){
			postIncDec.Add(All.FindById(((PostfixExpr) cur.data.GetByIndex(0)).Left.NodeId));
		}
	}
}

// note cahnged paramaters
changedPointerDataRef.Add(pointerParamDataRef * (preIncDec + postIncDec));
changedPointerDataRef.Add(pointerParamDataRef * (preIncDec + postIncDec).GetFathers());

// data ref passed by ref to function
CxList adressObjs = All.FindByType(typeof(UnaryExpr)).FindByShortName("Address");
changedPointerDataRef.Add(pointerParamDataRef.FindByFathers(adressObjs.FindByFathers(All.FindByType(typeof(Param)))));

// remove all param decls who have an instance changed
pointerParams -= pointerParams.FindDefinition(All.FindByFathers(changedPointerDataRef));

// param itself passed by value to function
pointerParams -= pointerParams.FindDefinition(All.FindAllReferences(pointerParams).FindByFathers(All.FindByType(typeof(Param))));

// remove changes through array access
CxList arrays = All.FindByType(typeof(IndexerRef)).FindAllReferences(pointerParams);
arrays = arrays.FindByAssignmentSide(CxList.AssignmentSide.Left);
arrays.Add(All.FindByName(arrays).FindByFathers(arrays));
pointerParams -= pointerParams.FindDefinition(arrays);

result = pointerParams;