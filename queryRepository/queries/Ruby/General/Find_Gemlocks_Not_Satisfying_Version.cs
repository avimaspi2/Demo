// Given a binaryexp node and a version number (minimum), return all the nodes NOT satisfying the 
//  version, based on the minimum requirement version found in the node
//  example syntax in preprocessed file:
//    $GemLCK["activesupport"] = ["3.1.0"]+["="]

if (param.Length > 1)
{
	CxList binaryexps = param[0] as CxList;
	string current = param[1] as string;


	CxList res = All.NewCxList();
	
	foreach (CxList binaryexp in binaryexps) {
	
		CxList literals = All.GetByAncs(binaryexp).FindByType(typeof(StringLiteral));
		
		CSharpGraph gr = literals.data.GetByIndex(0) as CSharpGraph;
		if (gr != null) {
			string strPackageName = gr.ShortName;
			if (literals.Count > 1 || param.Length >2) {
			
				CSharpGraph gr1 = literals.data.GetByIndex(0) as CSharpGraph;
				
				CSharpGraph gr2;
				
				if (param.Length > 2) { // special case for only one item
					 gr2 = gr1;
				} else {	
					 gr2 = literals.data.GetByIndex(1) as CSharpGraph;
				}
				
				string reference = gr1.ShortName;
				string oprtr 	 = gr2.ShortName;
				
				reference = reference.Replace("\"", "");
				oprtr = oprtr.Replace("\"", "");
				

				// Heuristic: Convert the version (e.g.: 2.8.3.1) to a double for easy comparison
				//   result: 2.008.003.001 
				
				// First round: Convert the reference version
				bool bResult = false;
				Double version = 0.0;
				int digitsright = 0;
				Regex rgx = new Regex(@"^([0-9]*)([^0-9]*.*)");

				string[] ver_str = Regex.Split(reference, @"\.");
				for (int num = 0; num < ver_str.Length; num++)
				{
					Match match = rgx.Match(ver_str[num]);
					if (match.Success)
					{
						string numpart = match.Groups[1].Value;
						string strpart = match.Groups[2].Value;
						
						int numpartint = 0;

						if (numpart.Length > 0)
						{
							numpartint = Convert.ToInt16(numpart);
						}

						// Add here checking strpart and handilng "RC1","beta12", etc
						version += ((double) numpartint) / Math.Pow(10, digitsright);
						digitsright += 3;
					}
					else
					{
					}
				}
				Double dblReference = version;


				// Second round: Convert the current version
				version = 0.0;
				digitsright = 0;
				ver_str = Regex.Split(current, @"\.");
				for (int num = 0; num < ver_str.Length; num++)
				{
					Match match = rgx.Match(ver_str[num]);
					if (match.Success)
					{
						string numpart = match.Groups[1].Value;
						string strpart = match.Groups[2].Value;
						int numpartint = 0;

						if (numpart.Length > 0)
						{
							numpartint = Convert.ToInt16(numpart);
						}

						// Add here checking strpart and handilng "RC1","beta12", etc

						version += ((double) numpartint) / Math.Pow(10, digitsright);
						digitsright += 3;
					}
					else
					{
					}
				}

				Double dblCurrent = version;

				bResult = false;
				
				if (oprtr.Equals("=")) {				
					bResult = (dblCurrent != dblReference);
				} else if (oprtr.Equals(">=")) {
					bResult = (dblCurrent <= dblReference);
				} else if (oprtr.Equals("~>")) {
					bResult = (dblCurrent <= dblReference);
				} else if (oprtr.Equals("")) {
					bResult = (dblCurrent <= dblReference);
				} else { // Special case for extra parameter:
					bResult = (dblCurrent <= dblReference);
				}

				
				if (!bResult) {
					res.Add(binaryexp);
				}
				
				
			}


		}

	}
	
	result = res;
	
}