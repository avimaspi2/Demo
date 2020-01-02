// Frames should not have id's and names, or they are open to frame injection attack

CxList methods = Find_Methods();
CxList nameAndId = methods.FindByName("cx_iframe_id")
	+ methods.FindByName("cx_iframe_name");
result = All.GetParameters(nameAndId).FindByType(typeof(UnknownReference));

result -= Find_Test_Code();