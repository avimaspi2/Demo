CxList filter = Potential_ReDoS_In_Match() + 
	Potential_ReDoS_In_Replace() + 
	Potential_ReDoS_In_Static_Field();

result = Find_Evil_Strings() - filter - Find_Evil_Strings().DataInfluencingOn(filter);