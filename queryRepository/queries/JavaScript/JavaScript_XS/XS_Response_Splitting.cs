/*
Response splitting query will look for any flow from inputs to potential vulnerable http header fields 
(that do not check for validity of the assigned variable),
The access to HTTP header fields will happen in XS $.net Mail api.
*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	//those are the vulnerable fields
	List<string> names = new List<string>(new string[]{"type","encoding","contentType"});
	List<string> MailFields = new List<string>(new string[]{"subject","subjectEncoding"});
	CxList potentialHeaderSplittingFieldsOfPart = XSAll.FindByShortNames(names);
	CxList potentialHeaderSplittingFieldsOfMail = XSAll.FindByShortNames(MailFields);
	CxList onLeftPart = potentialHeaderSplittingFieldsOfPart.FindByAssignmentSide(CxList.AssignmentSide.Left);
	CxList onLeftMail = potentialHeaderSplittingFieldsOfMail.FindByAssignmentSide(CxList.AssignmentSide.Left);
	
	//we want only those of $.net Mail 
	CxList XSNewObjects = XSAll.FindByType(typeof(ObjectCreateExpr));
	CxList Fields = onLeftPart.DataInfluencingOn(XSNewObjects.FindByName("*Mail.Part"));
	Fields.Add(onLeftMail.DataInfluencingOn(XSNewObjects.FindByName("*net.Mail")));
	
	/*
	 * This part handle the following case:
	 * var thirdPart = new $.net.Mail.Part();
	 * thirdPart.type =input;
	 */
	Fields.Add(onLeftPart.FindByMemberAccess("Part.*"));
	Fields.Add(onLeftMail.FindByMemberAccess("Mail.*"));
		
	//look for all flows to vulnerable fields from inputs
	result.Add(Fields.DataInfluencedBy(XS_Find_Interactive_Inputs()));
}