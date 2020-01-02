CxList outputs = NodeJS_Find_Interactive_Outputs();
CxList inputs = NodeJS_Find_DB_Out();
inputs.Add(NodeJS_Find_Read());

CxList sanitize = NodeJS_Find_XSS_Sanitize();

// If SWIG is not prone to XSS (auto escaped) then remove SWIG outputs.
if(NodeJS_Find_Swig_Autoescape_False().Count == 0)
{
	outputs -= NodeJS_Find_Swig_Interactive_Outputs();
}

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
result.Add(inputs * outputs - sanitize);