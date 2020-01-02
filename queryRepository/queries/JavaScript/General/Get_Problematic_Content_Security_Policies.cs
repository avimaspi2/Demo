/**
*this general query is used 
*to get the permissive content security
*policy html headers 
*/
if(param.Length > 0){
	CxList tagsHtml = param[0] as CxList;
		
	if(tagsHtml.Count > 0){
		string tagFullValue, contentValue = null;
		//CSP Header to check
		List<string> cxListResults = new List<string>(new string[] { 
			"default-src", "script-src", "style-src", 
			"img-src", "connect-src", 
			"font-src", "object-src", 
			"media-src", "frame-src", 
			"form-action", "frame-ancestors"});
		
		foreach(CxList tag in tagsHtml){
			//get all vlaue from content			
			tagFullValue = tag.GetFirstGraph().FullName;
			Regex contentValueRegex = new Regex("content=\"(?<cont>[^\"]+)\"");
			var cont = contentValueRegex.Match(tagFullValue);
			contentValue = cont.Groups["cont"].Value;
			
			foreach(string stringTest in cxListResults){									
				if(contentValue.Contains(stringTest)){
					//get all value from a specific keyword									
					Regex keywordValueRegex = new Regex("" + stringTest + " (?<keyContent>[^;]+)");
					var keywordValue = keywordValueRegex.Match(contentValue);
					var myListTokens = keywordValue.Groups["keyContent"].Value;	
										
					if(myListTokens.Contains("'*'")){									
						result.Add(tag);
					}										
				}
			}
		}
	}
}