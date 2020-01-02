// This query finds all strings in "Match" that are influenced by input
CxList inputs = Find_Inputs();

CxList allMatchStrings = Find_Strings_In_Match();
		
result = allMatchStrings.DataInfluencedBy(inputs);