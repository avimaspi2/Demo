//DateTime startTime = DateTime.Now;

CxList jspCode = Find_Jsp_Code();

CxList dspPartial = jspCode.FindByShortName("*__DSP__");
CxList dspDroplet = All.NewCxList();
CxList dspInvoke = All.NewCxList();


dspDroplet = dspPartial.FindByShortName("___retVal___*").FindByType(typeof(Declarator));

dspDroplet = jspCode.GetByAncs(dspDroplet);

dspInvoke = jspCode.FindByMemberAccess(".__DSP__");

dspInvoke.Add(dspInvoke.GetFathers());

dspInvoke.Add(jspCode.GetByAncs(dspInvoke));

//dspInvoke.Add(All.GetAncOfType(typeof(ExprStmt)));
dspInvoke.Add(dspInvoke.GetFathers());

CxList dspAll = All.NewCxList();
dspAll.Add(dspPartial);
dspAll.Add(dspDroplet);
dspAll.Add(dspInvoke);

Dictionary<long,int> existLine = new Dictionary<long,int>();

// create dictionary of all lines of requested nodes
foreach (CxList oneDspNode in dspAll) 
{
	try
	{	
		//Get the relevant activity name
		CSharpGraph g = oneDspNode.TryGetCSharpGraph<CSharpGraph>();
		if (g != null && g.LinePragma != null)  
		{ 
			int fileId = g.LinePragma.GetFileId();
			int lineNo = g.LinePragma.Line;
			long key = (long) fileId * Int32.MaxValue + lineNo;
			if (!existLine.ContainsKey(key))
				existLine[key] = 1;
		}
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}
//result = All.GetAncOfType(typeof(ExprStmt));
CxList tempResult = All.NewCxList();

// add all nodes if there line id and file id present in dspAll CxList
foreach (CxList oneNode in jspCode)
{
	try
	{
		//Get the relevant activity name
		CSharpGraph g = oneNode.TryGetCSharpGraph<CSharpGraph>();
		if (g != null && g.LinePragma != null) 
		{ 
			int fileId = g.LinePragma.GetFileId();
			int lineNo = g.LinePragma.Line;
			long key = (long) fileId * Int32.MaxValue + lineNo;
			if (existLine.ContainsKey(key))
				tempResult.Add(oneNode);
		}
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

result = tempResult;

//DateTime endTime = DateTime.Now;