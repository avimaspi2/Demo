if (param.Length == 1) {
	result = base.Find_Members(param[0]);
} else if(param.Length == 2) {
	result = base.Find_Members(param[0], param[1]);
}