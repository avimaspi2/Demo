if (param.Length == 1)
{
	CxList allInputs = param[0] as CxList;
	CxList pairs = Add_Get_Pairs();

       
	CxList temp = allInputs.InfluencingOn(pairs, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
	
	result.Add(temp);
	temp = result.InfluencingOn(pairs, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
	result.Add(temp);
}