/*
	The query Validate_HSTS_Header receives as parameter a CxList
containing a string and checks if the value "max-age" is not lower than 31536000
seconds(one year) and if the flag "includeSubDomains" is set.
 	The parameter string has one of the following syntaxes:
		"max-age=<number>; includeSubDomains; preload"
		"max-age=<number>; includeSubDomains"
		"max-age=<number>; preload"
		"includeSubDomains; preload"
		"preload"
	(every one of these examples can also have the prefix "Strict-Transport-Security : ")
*/
if(param.Length == 1){
	CxList header = (CxList) param[0];
	string val = header.GetName();
	char [] charsToTrim = {'\'','\"',' '};
	val = val.Trim(charsToTrim);
	string [] keyValue = val.Split(':');
	if(keyValue.Length > 1){
		val = keyValue[1];
	}
	string [] values = val.Split(';');
	if(values.Length == 1){
		result = header;
	}
	else if(values.Length > 1){			
		if(values[0].Trim(' ').StartsWith("max-age")){
			string [] maxAgeAssign = values[0].Split('=');
			if(maxAgeAssign.Length > 1){
				string maxAgeValue = maxAgeAssign[1];
				int maxAge;
				if(Int32.TryParse(maxAgeValue, out maxAge)){
					if(maxAge < 31536000){
						result = header;
					}
				}
				else{
					result = header;
				}
			}
			else result = header;
		}
		if(!values[1].Trim(' ').Equals("includeSubDomains")){
			result = header;
		}
	}
}