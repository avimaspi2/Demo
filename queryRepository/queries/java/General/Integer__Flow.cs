// Find integer overflow and underflow, based on an input parameter

if (param.Length == 1)
{
	CxList inputs = param[0] as CxList;

	// All integers
	CxList integers = All.FindByType("int");
	// Only integers in the left side of an assignment
	integers = integers.FindByAssignmentSide(CxList.AssignmentSide.Left);

	// All binary expressions
	// ... +,-,*,/ do not have short names, but ==, &&, != etc. do have, which makes life a little simpler	
		
	CxList bin = All.NewCxList();
	CxList allBin = base.Find_BinaryExpr();
	CxList lhs = All.FindByAssignmentSide(CxList.AssignmentSide.Left);
	lhs = allBin.GetByAncs(lhs.GetAncOfType(typeof(IndexerRef)));
	//remove binary expressions that are inside of an array in left side
	allBin -= lhs;
		
	foreach (CxList exp in allBin)
	{
		BinaryExpr binExp = exp.TryGetCSharpGraph<BinaryExpr>();
		if (( binExp.Operator == BinaryOperator.Subtract )
			|| ( binExp.Operator == BinaryOperator.Divide )
			|| ( binExp.Operator == BinaryOperator.Multiply )
			|| ( binExp.Operator == BinaryOperator.ShiftLeft )
			|| ( binExp.Operator == BinaryOperator.Add )
			|| ( binExp.Operator == BinaryOperator.PowerOf ))
		{
			bin.Add(exp);
		}
	}
	
	// If statements containint the potential vulnerable integer
	CxList ifWithInt = integers.GetAncOfType(typeof(IfStmt));

	// All condition expressions' contents of the relevant "if" statements. Will be used to check (even if heuristically)
	// if the right thing is checked
	CxList conditions = All.GetByAncs(Find_Conditions().GetByAncs(ifWithInt));

	// The assign expression of the integer (could be a declarator)
	CxList intAssign = integers.GetAncOfType(typeof(AssignExpr));
	intAssign.Add(integers.GetAncOfType(typeof(Declarator)));

	// (relevant) Binary expression under the assign-int statement
	CxList binInAssign = bin.GetByAncs(intAssign);

	// Unknown references in the binary expression assign statement
	CxList unknownUnderBinary = Find_UnknownReference();
	unknownUnderBinary = unknownUnderBinary.GetByAncs(binInAssign);

	// Now pass on al the relevant binary expressions (in the assign statements) and see if it is well-checked
	// for an overflow
	CxList relevantAssign = All.NewCxList();
	foreach (CxList b in binInAssign)
	{
		// Find the unknown references under a binary expression; this is needed to see if there are any
		// references of them in the relevant if statement condition
		CxList binaryContent = unknownUnderBinary.GetByAncs(b);
	
		// Get the if statement around the current binary expression
		CxList relevantIf = b.GetAncOfType(typeof(IfStmt));
	
		// Find the references of the binary contents in the relevant condition statement
		CxList cond = conditions.GetByAncs(relevantIf);
		cond = cond.FindAllReferences(binaryContent);
	
		// If the number of references is at least as the number of the unknown references under the binary
		// expression, then most likely all the unknown references are covered in the condition. It's only a
		// heuristic check, in order not to need another foreach loop, and it looks strong enough for this
		// type of query, which is relatively not very tight
		if (cond.Count < binaryContent.Count)
		{
			// Add therelevant assign expression (or declarator)
			relevantAssign.Add(b.GetAncOfType(typeof(AssignExpr)));
			relevantAssign.Add(b.GetAncOfType(typeof(Declarator)));
		}
	}

	// Sanitize by basic keywords
	CxList methods = Find_Methods();
	CxList sanitizer = All.NewCxList();
	sanitizer.Add(methods.FindByShortNames(new List<string> {
			"indexOf",
			"Index",
			"length",
			"size"}));
		sanitizer.Add(methods.FindByMemberAccess("Integer.parseInt"));
		sanitizer.Add(methods.FindByMemberAccess("Integer.getInteger"));
		sanitizer.Add(methods.FindByMemberAccess("Integer.hashCode"));
		sanitizer.Add(methods.FindByMemberAccess("Integer.intValue"));
		sanitizer.Add(methods.FindByMemberAccess("Integer.shortValue"));
		sanitizer.Add(methods.FindByMemberAccess("Integer.valueOf"));
				
		sanitizer.Add(methods.FindByMemberAccess("DataInputStream.readInt"));
		sanitizer.Add(methods.FindByMemberAccess("DataInputStream.readBoolean"));
		sanitizer.Add(methods.FindByMemberAccess("DataInputStream.readByte"));
		sanitizer.Add(methods.FindByMemberAccess("DataInputStream.readUnsignedByte"));
		sanitizer.Add(methods.FindByMemberAccess("DataInputStream.readShort"));
		sanitizer.Add(methods.FindByMemberAccess("DataInputStream.readUnsignedShort"));
		sanitizer.Add(methods.FindByMemberAccess("DataInputStream.readChar"));
				
		sanitizer.Add(methods.FindByMemberAccess("Scanner.nextInt"));
		sanitizer.Add(methods.FindByMemberAccess("Scanner.nextShort"));
		sanitizer.Add(Find_Dead_Code_Contents());
		
		sanitizer.Add(All.FindByTypes(new string [] {"bool","boolean", "byte", "short", "char"}));

	// The result is every input that influences the relevant integers. There is no need to check influencs of
	// the binary expression as well, since it is defined to be in the right-hand side of the assignment.
	result = relevantAssign.InfluencedByAndNotSanitized(inputs, sanitizer);
}