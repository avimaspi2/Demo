/*
This query will look for an execution of an update on the DB
It will look for the executeUpdate of $.hdb and the prepareStatement which has a DB modifyng SQL call
*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	CxList update = XSAll.FindByShortName("executeUpdate");
	result.Add(update);
	CxList dbUpdate = XSAll.FindByShortName("prepareStatement");
	CxList sl = XSAll.FindByType(typeof(StringLiteral));
	List<string> dbModifiers = new List<string>(new string[]{"UPDATE*","INSERT*","DELETE*","DROP*","REMOVE*",
		"DESTROY*","CREATE*"});
	sl = sl.FindByShortNames(dbModifiers, false);
	result.Add(dbUpdate.DataInfluencedBy(sl));
	CxList prm = XSAll.FindByType(typeof(Param));
	//mark the parameters of the calls
	result = (XSAll - prm).GetParameters(result);
	//add the db set* that are influenced by previous finidings as an update as well
	result.Add(XS_Find_DB_Setters().DataInfluencedBy(result));
}