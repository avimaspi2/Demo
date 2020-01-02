//Look inside .xml files
const string AndroidNameSpace = "http://schemas.android.com/apk/res/android";
const string Attribute = "inputType";
const string TextNoSuggestions = "textNoSuggestions";
const string TextVisiblePassword = "textVisiblePassword";
const string TextPassword = "textPassword";
const string EditTextWidget = "EditText";
const string XmlFilesExtension = ".xml";

foreach (CxXmlDoc doc in cxXPath.GetXmlFiles("*" + XmlFilesExtension)) 
{ 
	XPathNavigator navigator = doc.CreateNavigator();
	XPathNodeIterator nodeIterator = navigator.Select("*/" + EditTextWidget);
	while (nodeIterator.MoveNext())
	{
		XPathNavigator currentNodeNavigator = nodeIterator.Current;
		string attributeValue = currentNodeNavigator.GetAttribute(Attribute, AndroidNameSpace);
		bool isTextPassword = attributeValue.Contains(TextPassword);
		bool isCacheBlocked = attributeValue.Contains(TextNoSuggestions) && attributeValue.Contains(TextVisiblePassword);
		if(!isTextPassword && !isCacheBlocked)
		{
			result.Add(cxXPath.CreateXmlNode(currentNodeNavigator, doc, 2, false));
		}
	}
}

//Look inside Java files
CxList editTexts = All.FindByType(EditTextWidget);
CxList password = Find_All_Passwords();
password.Add(Find_Personal_Info());
CxList passwordsEditText = editTexts * password;
CxList inputTypeMethods = passwordsEditText.GetMembersOfTarget().FindByShortName("setInputType");
CxList memberAccesses = Find_MemberAccess();
CxList binaryExpression = Find_BinaryExpr();
CxList potentialParameters = All.NewCxList();
potentialParameters.Add(memberAccesses);
potentialParameters.Add(binaryExpression);

CxList cacheBlocked = memberAccesses.FindByShortNames(new List<string>{"TYPE_TEXT_FLAG_NO_SUGGESTIONS",
		"TYPE_TEXT_VARIATION_VISIBLE_PASSWORD"});
CxList passwordField = memberAccesses.FindByShortName("TYPE_TEXT_VARIATION_PASSWORD");

foreach(CxList inputTypeMethod in inputTypeMethods)
{
	CxList inputType = memberAccesses.GetByAncs(potentialParameters.GetParameters(inputTypeMethod, 0));
	bool isTextPassword = (passwordField * inputType).Count > 0;
	bool isCacheBlocked = (cacheBlocked * inputType).Count > 1;
	if(!isTextPassword && !isCacheBlocked)
	{
		result.Add(inputTypeMethod);
	}
}