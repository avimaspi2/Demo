//This query finds attempts to open file while using not normalized file name
CxList inputs = Find_Interactive_Inputs();
CxList obj = All.FindByType(typeof(UnknownReference)) + All.FindByType(typeof(Declarator));
CxList files = obj.FindByTypes(new String[] {"FileStream", "FileInfo", 
	"*.Filestream", "*.FileInfo"});
//files.Add(All.FindByName("*File.*"));
files.Add(All.FindByName("*.File.*"));

CxList filesMethods = files.GetMembersOfTarget();;
filesMethods = filesMethods.FindByShortName("Close") + filesMethods.FindByShortName("Dispose");
files -= filesMethods.GetTargetOfMembers();

CxList sanitize = All.FindByName("*Server.MapPath") + All.FindByName("*Request.MapPath"); 

result = files.InfluencedByAndNotSanitized(inputs, sanitize);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);