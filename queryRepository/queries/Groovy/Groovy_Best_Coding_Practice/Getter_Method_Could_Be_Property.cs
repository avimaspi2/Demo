// If a class defines a public method that follows the Java getter notation and 
// that returns a constant, then it is cleaner to provide a Groovy property for 
// the value rather than a Groovy method.
CxList methods = All.FindByType(typeof(MethodDecl)).FindByShortName("get*");

CxList list = All.NewCxList();

foreach(CxList m in methods)
{
	CxList stmtCollect = All.FindByType(typeof(StatementCollection)).FindByFathers(m);
	
	if (m.GetAncOfType(typeof(ClassDecl)).Count > 0 // the father is a class
		&& stmtCollect.Count == 1) // single return
	{
		try
		{
			ClassDecl c = m.GetAncOfType(typeof(ClassDecl)).data.GetByIndex(0) as ClassDecl;
			StatementCollection stmtCol = stmtCollect.data.GetByIndex(0) as StatementCollection;
			ReturnStmt ret = stmtCol[0] as ReturnStmt;
			if (ret != null)
			{
				Expression e = ret.Expression;
				if (e.ResultType != null && 
					(e.ResultType.Name.Equals("string") ||
					e.ResultType.Name.Equals("int") ||
					e.ResultType.Name.Equals("float") ||
					e.ResultType.Name.Equals("double") ||
					e.ResultType.Name.Equals("bool"))) // literals
				{
					list.Add(m);
				}
				else if(e.GetType() == typeof(UnknownReference))
				{
					UnknownReference id = e as UnknownReference;
					CxList vars = All.FindByType(typeof(FieldDecl)).
						FindByFieldAttributes(Modifiers.Static).
						FindByName(c.Name + "." + id.VariableName);
					if (vars.Count == 1)
					{
						list.Add(m);
					}
				}
			}
		}
		catch(Exception ex)
		{
			cxLog.WriteDebugMessage(ex.Message);
		}
	}
}
result = list;