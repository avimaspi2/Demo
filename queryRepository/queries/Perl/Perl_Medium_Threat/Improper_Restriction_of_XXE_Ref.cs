CxList inputs = Find_Inputs();
CxList LibXML = All.FindByMemberAccess("XML.LibXML");
CxList parsers = LibXML.GetMembersOfTarget().FindByShortNames(new List<string> {"Reader", "Parser"});
CxList newInvoke = parsers.GetMembersOfTarget();

//Add: XML::LibXML->new || XML::LibXML->load_xml
newInvoke.Add(LibXML.GetMembersOfTarget() - parsers);

CxList prm = All.FindByType(typeof(Param));
CxList safeInvoke = All.NewCxList();

//Find the safe invokes
CxList zero = All.FindByShortName("0");
CxList ur = All.FindByType(typeof(Declarator)) + All.FindByType(typeof(UnknownReference));
zero.Add(All.FindAllReferences(ur.FindByAssignmentSide(CxList.AssignmentSide.Left).GetByAncs(zero.GetFathers())));

foreach(CxList invoke in newInvoke)
{
	CxList par = prm.GetParameters(invoke);
	CxList loadExists = par.FindByShortName("load_ext_dtd");
	CxList expandExists = par.FindByShortName("expand_entities");
	if(loadExists.Count > 0 && expandExists.Count > 0)
	{ 
		int i = 0;
		for( i = 0; i < 20; i++)
		{
			if(loadExists.GetParameters(invoke, i).Count > 0)
			{
				break;
			}
		}
		
		CxList foundSanitizierLoad = zero.GetParameters(invoke, i + 1);
		int j = 0;
		for( j = 0; j < 20; j++)
		{
			if(expandExists.GetParameters(invoke, j).Count > 0)
			{
				break;
			}
		}	
		CxList foundSanitizerExpand = zero.GetParameters(invoke, j + 1);
		
		if(foundSanitizierLoad.Count > 0 && foundSanitizerExpand.Count > 0)
		{
			safeInvoke.Add(invoke);
		}
	}
}
result = inputs.InfluencingOn(newInvoke - safeInvoke);