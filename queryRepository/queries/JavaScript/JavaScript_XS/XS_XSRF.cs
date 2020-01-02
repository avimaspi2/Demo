/*This query will look for flow from inputs to db-update that is not sanitized by xsaccess prevent_xsrf token*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	//look for interactive inputs
	CxList inputs = XS_Find_Interactive_Inputs();
	//look for all types of XS db- update
	CxList dbUpdate = XS_Find_DB_Update();
	dbUpdate.Add(XSDS_Find_DB_Update());
	
	//we will work on flow that is potentialy vulnerable (until proven otherwise) between inputs and db-update
	CxList potentialVul = dbUpdate.DataInfluencedBy(inputs);
	//we are interested only on the source of the flow, since if there is a prevent_xsrf token, it will be applied on the
	// input source.
	CxList source = potentialVul.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	
	//the following builds a hierarchial dictionary for all potential XSRF sources

	//create a new dictionary which we will later share
	Dictionary<string,HashSet<int>> xsaccessMappingDictionary = new Dictionary<string,HashSet<int>>();
		
	//we will use the outer encapsulation namespace of a Typescript file (CxJsNS) to mark the .xsaccess file
	//and to mark the file with the vulnerable source
		
	CxList namespaceDecl = XSAll.FindByType(typeof(NamespaceDecl));
	CxList cxjsns = namespaceDecl.FindByShortName("CxJSNS*");
	CxList accessFiles = cxjsns.FindByFileName("*.xsaccess");

	CxList outerNSOfVulnerableCode = All.NewCxList();
	
	//for each Typescript file check whether we have a potential vulnerable source inside, in case we do,
	//	add the encapsulating outer namespace of the source to "outerNSOfVulnerableCode"
	foreach(CxList outerNS in cxjsns)
	{
		if(source.GetByAncs(outerNS).Count > 0)
		{
			outerNSOfVulnerableCode.Add(outerNS);
		}
	}
	//we will iterate over outer class of vulnerable code - we do this in order to eliminate the case we run
	//multiple times on a file in case there are number of vulnerabile sources in the file
	foreach(CxList outer in outerNSOfVulnerableCode)
	{
		try{
			CSharpGraph outerNS = outer.GetFirstGraph();
			if(outerNS == null || outerNS.LinePragma == null)
			{
				continue;                             
			}
			//extract the file name of the source
			string name = outerNS.LinePragma.FileName;
			//if dictionary doesn't already contain the name of the file
			if(!xsaccessMappingDictionary.ContainsKey(name))
			{
				//we will add the fileName as a key to our \tonary  
				xsaccessMappingDictionary.Add(name, new HashSet<int>());
				string fileName = outerNS.LinePragma.FileName;
				int lastIndexOfBackSlash = fileName.LastIndexOf("\\");
				if(lastIndexOfBackSlash < 0)
				{
					continue;
				}
				string father = fileName.Remove(lastIndexOfBackSlash);
				string originalFileFolder = father.ToString();
				string projectPath = cxScan.GetScanProperty("projectPath");				
				//we will start trimming the path from bottom up
				//we don't want to go upper than our project folder				
				while(father.ToLower().StartsWith(projectPath.ToLower()))
				{   
					//look if our project has an xsaccess file under this path
					CxList inCurrentFolder = accessFiles.FindByFileName(father + "*");                                             
					//in case such found add it as a value to our dicionary, and continue going up with the path.
					foreach(CxList aFile in inCurrentFolder)
					{
						CSharpGraph g = aFile.GetFirstGraph();
						string accessFileName = g.LinePragma.FileName;
						string accessFolderName = accessFileName.Remove(accessFileName.LastIndexOf("\\"));
						if(originalFileFolder.Contains(accessFolderName))
						{						
							xsaccessMappingDictionary[name].Add(g.LinePragma.GetFileId());
						}                                         
					}                                              
					lastIndexOfBackSlash = father.LastIndexOf("\\");
					father = father.Remove(lastIndexOfBackSlash);
				}                                                                                                             
			}
		}catch(Exception e)
		{
			cxLog.WriteDebugMessage(e);
		} 
	}
	
	//look for prevent_xsrf tokens.
	CxList tr = XSAll.FindByShortName("prevent_xsrf").GetByAncs(XSAll.FindByShortName("true").GetFathers());
	CxList fl = XSAll.FindByShortName("prevent_xsrf").GetByAncs(XSAll.FindByShortName("false").GetFathers());
	
	//iterate over all potential vulnerable XSRF flows
	foreach(CxList pr in potentialVul.GetCxListByPath())
	{
		CSharpGraph g = pr.TryGetCSharpGraph<CSharpGraph>();
		//check if the dictionary contains source's file name key inside, if not- this means this file
		//uses the default which is false-> in this case we will add the flow to vulnerable results
		if(!xsaccessMappingDictionary.ContainsKey(g.LinePragma.FileName))     
		{
			result.Add(pr);
			
		}else
		{
			//otherwise, the file has a relevant xsaccess file which is either set to true or false
			// since the dictionary's order matters (goes bottom up) it's correct to see the first and then break from the loop
			bool sanitizerFound = false;
			//iterate over the relevant xsaccess files group for my source
			foreach(int influencingXSFile in xsaccessMappingDictionary[g.LinePragma.FileName])
			{
				//if token is set - we have a sanitized source
				if(tr.FindByFileId(influencingXSFile).Count > 0)
				{
					sanitizerFound = true;
					break;
					//otherwise if we found token that is set to false -> we don't want to look further
					//and the source is not sanitized	
				}else if(fl.FindByFileId(influencingXSFile).Count > 0)
				{
					break;
				}
			}
			if(!sanitizerFound)
			{
				result.Add(pr);
			}
		}
	                
	}
	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
}