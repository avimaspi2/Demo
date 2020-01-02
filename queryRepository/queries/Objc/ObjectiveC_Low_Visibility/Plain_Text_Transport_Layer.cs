CxList pureHttp = Find_Pure_Http();
CxList newOutputs  =  Find_Insufficient_Out_Transport_Layer();
result = newOutputs.InfluencedBy(pureHttp);