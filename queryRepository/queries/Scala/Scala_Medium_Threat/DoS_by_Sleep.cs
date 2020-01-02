CxList Inputs = Find_Interactive_Inputs();
CxList delay = All.FindByName("*Thread.sleep");

CxList inputStreamRead = All.FindByMemberAccess("InputStream.read");
CxList readFirstParam = All.GetParameters(inputStreamRead, 0);
CxList readThirdParam = All.GetParameters(inputStreamRead, 2);
result = (readThirdParam + delay).DataInfluencedBy(readFirstParam + Inputs);