CxList evil = Find_Evil_Strings();

CxList filter = Potential_ReDoS_In_Code() +
	Potential_ReDoS_In_Static_Field() +
	All.FindByType("ValidationExpression");

result = evil - filter - evil.DataInfluencingOn(filter);