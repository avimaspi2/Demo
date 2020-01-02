/*
	1. get all addTrailers,setHeader and writeHead methods
	2. get those methods as MethodDecl with name anonySomeHash
	3. Get ...
		
*/
CxList allINHTTP = NodeJS_Get_All_HTTP();
CxList allMD = allINHTTP.FindByType(typeof (MethodDecl));
CxList uRef = allINHTTP.FindByType(typeof (UnknownReference));
CxList ma = allINHTTP.FindByType(typeof (MemberAccess));

CxList urMA = All.NewCxList();
urMA.Add(uRef);
urMA.Add(ma);

CxList urMAInRight = urMA - urMA.FindByAssignmentSide(CxList.AssignmentSide.Left);

//1. get all addTrailers and writeHead methods
CxList writeToHeaderMeth = allINHTTP.FindByShortNames(new List<string>{"addTrailers","writeHead"});

//2. get those methods as MethodDecl with name anonySomeHash
CxList methParams = allINHTTP.GetByAncs(writeToHeaderMeth);

CxList onlyParams = allINHTTP.FindByType(typeof (Param));
CxList headersAsMD = onlyParams.GetParameters(methParams);	//parameter is of type anonySomeHash
headersAsMD.Add(All.FindAllReferences(All.FindByFathers(headersAsMD)).GetAssigner());

//3.
CxList allSetHeader = allINHTTP.FindByShortName("setHeader");
CxList setWriteVals = urMA.GetParameters(allSetHeader);
setWriteVals.Add(urMAInRight.GetByAncs(headersAsMD));

result = setWriteVals;
if(Hapi_Find_Server_Instance().Count > 0)
{
	result.Add(Hapi_Find_Header_Set());
}