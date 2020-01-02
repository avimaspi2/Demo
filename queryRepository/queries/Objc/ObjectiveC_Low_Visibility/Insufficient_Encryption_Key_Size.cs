CxList encryptParams = All.GetParameters(Find_Encryption_Methods(), 4);
CxList integers = Find_IntegerLiterals();
CxList bin = Find_BinaryExpr();
bin = bin.FindByShortName("");

foreach(CxList prm in encryptParams)
{
	IntegerLiteral p = prm.TryGetCSharpGraph<IntegerLiteral>();
	if (p != null)
	{
		try 
		{
			// Size is in bytes, so if we need 256 bit we write 32
			if (p.Value < 32)
			{	
				result.Add(prm);
			}
		}
		catch (Exception exc)
		{
			cxLog.WriteDebugMessage(exc);
		}
	}
	else if (prm.FindByShortName("kCCKeySize*").Count > 0)
	{
		CSharpGraph g = prm.TryGetCSharpGraph<CSharpGraph>();
		if (g != null)
		{
			try
			{
				string pName = g.ShortName;
				if (
					pName.Equals("kCCKeySizeAES128") ||
					pName.Equals("kCCKeySizeAES192") ||
					pName.Equals("kCCKeySizeDES") ||
					pName.Equals("kCCKeySize3DES") ||
					pName.Equals("kCCKeySizeMinCAST") ||
					pName.Equals("kCCKeySizeMinRC4") ||
					pName.Equals("kCCKeySizeMinRC2") ||
					pName.Equals("kCCKeySizeMinBlowfish")
					)
				{
					result.Add(prm);
				}
			}
			catch (Exception exc)
			{
				cxLog.WriteDebugMessage(exc);
			}
		}
		else
		{
			CxList relevantIntegers = integers.InfluencingOnAndNotSanitized(prm, bin);
			foreach (CxList relevantIntegerPath in relevantIntegers.GetCxListByPath())
			{
				CxList relevantInteger = relevantIntegerPath.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
				IntegerLiteral i = relevantInteger.TryGetCSharpGraph<IntegerLiteral>();
				if (i != null)
				{
					try
					{
						// Size is in bytes, so if we need 256 bit we write 32
						if (i.Value < 32)
						{	
							result.Add(relevantIntegerPath);
						}
					}
					catch (Exception exc)
					{
						cxLog.WriteDebugMessage(exc);
					}
				}
			}
		}
	}
}