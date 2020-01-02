// $ASP*

CxList root = All;	// for VBScript client side: AllVbs()

// VBScript/JavaScript server side: Server.CreateObject (but can go without the "server"
// JavaScript client side: new ActiveXObject
// VBScript client side: CreateObject

CxList activex = 
	root.FindByShortName("CreateObject", false) + 		// VBScript
	root.FindByShortName("ActiveXObject", false);		// JavaScript (untested)

CxList inputs = Find_Inputs();

result = activex.DataInfluencedBy(inputs);