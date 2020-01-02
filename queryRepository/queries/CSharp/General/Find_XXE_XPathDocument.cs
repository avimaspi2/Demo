const int SUPPORTED_RUNTIME = 400;
const int SKU_VERSION = 452;
const int NET_VERSION = 452;

CxList methods = Find_Methods();
CxList xml_parse_mthds = All.NewCxList();

/*get the run time vertion*/
CxList appConfig = All.FindByFileName("*.config");
var reg = new Regex("");

CxList elemetsConfig = All.NewCxList();
if(!All.isWebApplication){
	reg = new Regex(@"v([0-9]).([0-9])(?:.([0-9]))?");
	elemetsConfig += appConfig.FindByType(typeof(MemberAccess)).FindByShortName("VERSION").GetAssigner();
}else{
	reg = new Regex(@"([0-9]).([0-9])(?:.([0-9]))?");
	elemetsConfig += appConfig.FindByType(typeof(MemberAccess)).FindByShortName("TARGETFRAMEWORK").GetAssigner();
}


var strSuppRuntime = string.Empty;
if(reg.IsMatch(elemetsConfig.GetName())){		
	var m = reg.Match(elemetsConfig.GetName());
	strSuppRuntime += m.Groups[1];
	strSuppRuntime += m.Groups[2];	
	bool exists = m.Groups[3].Success;
	if(exists == true){
		strSuppRuntime += m.Groups[3];
	}else{
		strSuppRuntime += "0";
	}	
}


int suppRuntime;
if (!string.IsNullOrEmpty(strSuppRuntime) && Int32.TryParse(strSuppRuntime, out suppRuntime)){	

	if(suppRuntime < SUPPORTED_RUNTIME){		
		result = methods.FindByMemberAccess("XPathDocument.CreateNavigator");				
	}else{		
		CxList skuConfig = appConfig.FindByType(typeof(MemberAccess)).FindByShortName("SKU").GetAssigner();
		if(skuConfig.Count > 0)
		{
			var strSKu = string.Empty;
			if(!All.isWebApplication){
				var regSku = new Regex(@".NETFramework,Version=v([0-9]).([0-9])(?:.([0-9]))?");		
		
				if(regSku.IsMatch(skuConfig.GetName())){		
					var m = regSku.Match(skuConfig.GetName());
					strSKu += m.Groups[1];
					strSKu += m.Groups[2];
					bool exists = m.Groups[3].Success;
					if(exists == true){
						strSKu += m.Groups[3];
					}else{
						strSKu += "0";
					}		
				}
			}else{
				strSKu = strSuppRuntime;
			}

			int versionSku;			
			if(!string.IsNullOrEmpty(strSKu) && Int32.TryParse(strSKu, out versionSku)){
				if(versionSku < SKU_VERSION ){
					result = methods.FindByMemberAccess("XPathDocument.CreateNavigator");
				}
				
			}else{
				result = methods.FindByMemberAccess("XPathDocument.CreateNavigator");
			}		
		}
	}
}
else{
	result = methods.FindByMemberAccess("XPathDocument.CreateNavigator");
}