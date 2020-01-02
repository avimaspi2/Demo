/*
In this query we look for commands that follow a redirect commands.
For every redirect command we will look at the containing block and make sure that the "redirect" is the last
command in the block, by comparing Id's.
There might be (very) extreme cases where we fail to find a result - for example when there is a redirect
at the end of both "if" blocks (if and else), but there are additional commands after the if statement. For
now we prefer this then a more complex algorithm and/or false positives.
*/

// Find all the relevant redirects
CxList sendRedirect = All.FindByMemberAccess("HttpServletResponse.sendRedirect");
sendRedirect.Add(All.FindByName("*response.sendRedirect"));  
sendRedirect.Add(All.FindByName("*Response.sendRedirect"));
sendRedirect.Add(All.FindByMemberAccess("HTTPUtilities.safeSendRedirect")); //ESAPI

// Ignore single line if statements
sendRedirect -= sendRedirect.FindByFathers(sendRedirect.GetFathers().FindByType(typeof(IfStmt)));
sendRedirect -= sendRedirect.FindByFathers(sendRedirect.GetFathers().FindByType(typeof(IterationStmt)));

// Find the blocks within which 
CxList statementsList = sendRedirect.GetAncOfType(typeof(StatementCollection));
// Get everything under the blocks, except for expression statements (that might have a higher Id,
// and break statements, that might still appear and not disturb us (in switch case, for example
CxList underStatementsList = All.GetByAncs(statementsList) - statementsList;
underStatementsList -= underStatementsList.FindByType(typeof(ExprStmt));
underStatementsList -= underStatementsList.FindByType(typeof(BreakStmt));
underStatementsList -= underStatementsList.GetByAncs(underStatementsList.FindByType(typeof(ReturnStmt)));

// Pass on all statements and for each statement, calculate the Ids of the redirect and 
foreach (CxList statements in statementsList)
{
	// Find the redirect under the statements (there might be more than one)
	CxList redirect0 = sendRedirect.GetByAncs(statements);
	CxList redirect = All.NewCxList();
	redirect.Add(redirect0);
	foreach (CxList r in redirect0)
	{
		if (r.GetAncOfType(typeof(StatementCollection)) != statements)
		{
			redirect -= r;
		}
	}

	// If there are various redirects in the block - just take the first one
	if (redirect.Count > 1)
	{
		int minId = int.MaxValue;
		foreach (CxList rt in redirect)
		{
			try
			{
				CSharpGraph g = rt.TryGetCSharpGraph<CSharpGraph>();
				int index = g.NodeId;
				if (index < minId)
				{
					minId = index;
				}
			}
			catch (Exception ex)
			{
				// in case of an exception in the CSharpGraph transformation
				cxLog.WriteDebugMessage(ex);
			}
		}
		redirect = All.FindById(minId);
	}
	// Find statements in the block
	CxList underStatements = underStatementsList.GetByAncs(statements);
	// Find only nodes relevant to the "redirect"
	CxList redirectThings = underStatements.GetByAncs(redirect);
	// Update the statements, not to include the "redirect" nodes
	underStatements -= redirectThings;
	
	// Find the maximum id under the redirect command, and keep the node of this Id. This is necessary
	// in case of more than one redirect in a block
	int maxRedirectId = -1;
	
	CxList relevantRedirect = All.NewCxList();
	relevantRedirect.Add(redirect);	
	foreach (CxList rt in redirectThings)
	{
		try
		{
			CSharpGraph g = rt.TryGetCSharpGraph<CSharpGraph>();
			int index = g.NodeId;
			if (index > maxRedirectId)
			{
				maxRedirectId = index;
				relevantRedirect = rt;
			}
		}
		catch (Exception ex)
		{
			// in case of an exception in the CSharpGraph transformation
			cxLog.WriteDebugMessage(ex);
		}
	}
	// Find the redirect that applies to the node we've found (biggest node Id)
	relevantRedirect = relevantRedirect.GetAncOfType(typeof(MethodInvokeExpr));

	// Look for a bigger node Id than the redirect, in the block statements out of the redirect
	foreach (CxList us in underStatements)
	{
		try
		{
			CSharpGraph g = us.TryGetCSharpGraph<CSharpGraph>();
			int index = g.NodeId;
			if (index > maxRedirectId)
			{
				result.Add(relevantRedirect.Concatenate(us));
				break;
			}
		}
		catch (Exception ex)
		{
			// in case of an exception in the CSharpGraph transformation
			cxLog.WriteDebugMessage(ex);
		}
	}
}