//first parameter is the CxList you want to run the FindByShortName
//the second parameter is the string or a List of strings
//the third parameter in caseSensitive true/false
if(param.Length == 3)
{
	CxList lookIn = param[0] as CxList;
	string name = param[1] as string;
	bool caseSensitive = true;
	try{
		caseSensitive = (bool) param[2];
	}catch(Exception e){cxLog.WriteDebugMessage(e);}
	
	List<string> tempList = new List<string>();
	if(!String.IsNullOrEmpty(name)){
		tempList.Add(name);
	}
	else{ 
		try{
			tempList = param[1] as List<string>;
			if(tempList == null){
				tempList = new List<string>();
			}	
		}catch(Exception e){cxLog.WriteDebugMessage(e);}
	}
	
	List<string> finalList = new List<string>();
	foreach(string thename in tempList){
		if(!String.IsNullOrEmpty(thename)){
				// Add the element
			finalList.Add(thename);
			if(thename.StartsWith("*")){
				if(!thename.EndsWith("*")){
					// add string delimited right
					finalList.Add(thename + "\"");
					finalList.Add(thename + "\'");
				}
			}
			else if(thename.EndsWith("*") && !thename.StartsWith("*")){
				// add string delimited left
				finalList.Add("\"" + thename);
				finalList.Add("\'" + thename);
			}
			else{
				// add string delimited both
				finalList.Add("\"" + thename + "\"");
				finalList.Add("\'" + thename + "\'");
			}
		}
	}
	
	result.Add(lookIn.FindByShortNames(finalList, caseSensitive));
}