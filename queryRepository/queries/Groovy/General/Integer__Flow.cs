// Find integer overflow and underflow, based on an input parameter

if (param.Length == 1)
{
	CxList inputs = param[0] as CxList;

	// All integers
	CxList integers = All.FindByType("int");
	// Only integers in the left side of an assignment
	integers = integers.FindByAssignmentSide(CxList.AssignmentSide.Left);

	// All binary expressions
	CxList bin = All.FindByType(typeof(BinaryExpr));
	// ... +,-,*,/ do not have short names, but ==, &&, != etc. do have, which makes life a little simpler
	bin = bin.FindByShortName("");
	// If statements containint the potential vulnerable integer
	CxList ifWithInt = integers.GetAncOfType(typeof(IfStmt));

	// All condition expressions' contents of the relevant "if" statements. Will be used to check (even if heuristically)
	// if the right thing is checked
	CxList conditions = All.GetByAncs(Find_Conditions().GetByAncs(ifWithInt));

	// The assign expression of the integer (could be a declarator)
	CxList intAssign = integers.GetFathers() + integers.GetAncOfType(typeof(Declarator));

	// (relevant) Binary expression under the assign-int statement
	CxList binInAssign = bin.GetByAncs(intAssign);

	// Unknown references in the binary expression assign statement
	CxList unknownUnderBinary = All.FindByType(typeof(UnknownReference));
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
			relevantAssign.Add(b.GetAncOfType(typeof(AssignExpr)) + b.GetAncOfType(typeof(Declarator)));
		}
	}

	// Sanitize by basic keywords
	CxList methods = Find_Methods();
	CxList sanitizer = 
		methods.FindByShortName("indexOf") +
		methods.FindByShortName("Index") +
		methods.FindByShortName("length") +
		methods.FindByShortName("size") +
		methods.FindByMemberAccess("Integer.parseInt") +
		Find_Dead_Code_Contents();

	// The result is every input that influences the relevant integers. There is no need to check influencs of
	// the binary expression as well, since it is defined to be in the right-hand side of the assignment.
	result = relevantAssign.InfluencedByAndNotSanitized(inputs, sanitizer);
}