//	The function checks whether a method parameter (or a list of parameters) is in the specified position.

//	CxList Find_By_Parameter_Position(CxList Method, int ParamNo, CxList ParamList)
//	Parameters: 
//	  ParamNo: Zero - based index of the parameter
//    ParamList : CxList of method parameters. 
//	  Return Value :A subset of “this” instance with methods whose parameters are given in the list in the specified position.


if (param.Length == 3)		
{	
	try	{
		
		CxList method = param[0] as CxList;
		int paramNo = (int)param[1];
		CxList paramList = param[2] as CxList;
		
		result = method.FindByParameters(All.GetParameters(method, paramNo) * paramList); 
	}
		
	catch(Exception ex){
		cxLog.WriteDebugMessage(ex);
	}	
}