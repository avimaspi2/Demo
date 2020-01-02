CxList lowCost = Find_Not_In_Range("botan/bcrypt.h", "generate_bcrypt", 2, 10, null);
lowCost.Add(Find_Not_In_Range("botan/ffi.h", "botan_bcrypt_generate", 4, 10, null));

CxList pathsToLowCost = All.FindDefinition(lowCost).DataInfluencingOn(lowCost);

CxList missingLowCosts = lowCost - pathsToLowCost.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

result = pathsToLowCost;
result.Add(missingLowCosts);