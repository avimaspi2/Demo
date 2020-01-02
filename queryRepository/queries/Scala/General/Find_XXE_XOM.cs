CxList methods = Find_Methods();

CxList xml_parse_mthds = 
	methods.FindByMemberAccess("Builder.build");

CxList sanitizing_methods = All.FindByType(typeof(ObjectCreateExpr)).FindByName("Builder");

CxList fst_param_all = All.GetParameters(sanitizing_methods, 0);
CxList snd_param_all = All.GetParameters(sanitizing_methods, 1);

CxList sanitizers = All.NewCxList();
foreach(CxList sanitize in sanitizing_methods){

	CxList fst_param = fst_param_all.GetByAncs(sanitize);
	CxList snd_param = snd_param_all.GetByAncs(sanitize);
	
	//if the constructor has no parameters, then it is by default sanitized
	if(fst_param.Count == 0){
		sanitizers.Add(sanitize);
	}
	else { 
		//if the constructor has only 1 param, it shall be false so the xml builder is sanitized
		if(fst_param.Count > 0 && snd_param.Count == 0 && fst_param.FindByShortName("false").Count > 0){
			sanitizers.Add(sanitize);
		}
		else {
			//if the constructor has 2 or more parameters, than the snd one shall be false
			if(snd_param.Count > 0 && snd_param.FindByShortName("false").Count > 0){
				sanitizers.Add(sanitize);
			}	
		}

	}
}



result = xml_parse_mthds - (xml_parse_mthds.InfluencedBy(sanitizers));