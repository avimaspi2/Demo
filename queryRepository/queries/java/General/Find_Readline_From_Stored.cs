CxList inputs = Find_Interactive_Inputs();
CxList readLines = inputs.FindByShortName("read*", false);
inputs -= readLines;
CxList readLineInfluencedByInputs = readLines.DataInfluencedBy(result).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
readLines -= readLineInfluencedByInputs;
result = readLines;