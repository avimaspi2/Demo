CxList filter = Potential_ReDoS_In_Match();
filter.Add(Potential_ReDoS_In_Replace());
filter.Add(Potential_ReDoS_In_Static_Field());

CxList evilString = Find_Evil_Strings();

result = evilString - filter - evilString.DataInfluencingOn(filter);