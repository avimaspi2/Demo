CxList inputs = All.FindByMemberAccess("CheckBoxList.SelectedItem", false);
inputs.Add(All.FindByMemberAccess("CheckBoxList.SelectedValue", false)); 
inputs.Add(All.FindByMemberAccess("CommandEventArgs.CommandArgument", false));
inputs.Add(All.FindByMemberAccess("CommandEventArgs.CommandArgument", false).GetMembersOfTarget());
inputs.Add(All.FindByMemberAccess("Console.Read*", false));
inputs.Add(All.FindByMemberAccess("DataBoundLiteralControl.Text", false)); 
inputs.Add(All.FindByMemberAccess("DataRow.Item", false)); 
inputs.Add(All.FindByMemberAccess("DropDownList.SelectedItem", false)); 
inputs.Add(All.FindByMemberAccess("DropDownList.selectedvalue", false)); 
inputs.Add(All.FindByMemberAccess("File.Value", false));
inputs.Add(All.FindByMemberAccess("FileUpload.FileName", false)); 
inputs.Add(All.FindByMemberAccess("Hidden.Value", false));
inputs.Add(All.FindByMemberAccess("HiddenField.Value", false)); 
inputs.Add(All.FindByMemberAccess("HtmlInputFile.Value", false)); 
inputs.Add(All.FindByMemberAccess("HtmlInputHidden.Value", false)); 
inputs.Add(All.FindByMemberAccess("HtmlInputPassword.Value", false)); 
inputs.Add(All.FindByMemberAccess("HtmlInputText.Value", false));
inputs.Add(All.FindByMemberAccess("HtmlSelect.Value", false)); 
inputs.Add(All.FindByMemberAccess("HtmlTextArea.Value", false));
inputs.Add(All.FindByMemberAccess("HttpPostedFile.ContentType", false)); 
inputs.Add(All.FindByMemberAccess("HttpPostedFile.FileName", false)); 
inputs.Add(All.FindByMemberAccess("HttpRequest.*", false));
inputs.Add(All.FindByMemberAccess("ListBox.SelectedItem", false)); 
inputs.Add(All.FindByMemberAccess("ListBox.SelectedValue", false)); 
inputs.Add(All.FindByMemberAccess("ListControl.SelectedValue", false)); 
inputs.Add(All.FindByMemberAccess("ListControl.Text", false)); 
inputs.Add(All.FindByMemberAccess("ListItem.Value", false)); 
inputs.Add(All.FindByMemberAccess("Password.Value", false));
inputs.Add(All.FindByMemberAccess("RadioButtonList.SelectedItem", false)); 
inputs.Add(All.FindByMemberAccess("RadioButtonList.SelectedValue", false)); 
inputs.Add(All.FindByMemberAccess("Request.BinaryRead", false)); 
inputs.Add(All.FindByMemberAccess("Request.BinaryRead", false).GetMembersOfTarget()); 
inputs.Add(All.FindByMemberAccess("Request.Cookies*", false)); 
inputs.Add(All.FindByMemberAccess("Request.Cookies*", false).GetMembersOfTarget()); 
inputs.Add(All.FindByMemberAccess("Request.Form*", false)); 
inputs.Add(All.FindByMemberAccess("Request.Form*", false).GetMembersOfTarget()); 
inputs.Add(All.FindByMemberAccess("Request.Headers", false));
inputs.Add(All.FindByMemberAccess("Request.Headers", false).GetMembersOfTarget());
inputs.Add(All.FindByMemberAccess("Request.Item", false)); 
inputs.Add(All.FindByMemberAccess("Request.Item", false).GetMembersOfTarget()); 
inputs.Add(All.FindByMemberAccess("Request.Params", false)); 
inputs.Add(All.FindByMemberAccess("Request.Params", false).GetMembersOfTarget()); 
inputs.Add(All.FindByMemberAccess("Request.QueryString*", false)); 
inputs.Add(All.FindByMemberAccess("Request.QueryString*", false).GetMembersOfTarget()); 
inputs.Add(All.FindByMemberAccess("Request.RawUrl", false)); 
inputs.Add(All.FindByMemberAccess("Request.RawUrl", false).GetMembersOfTarget()); 
inputs.Add(All.FindByMemberAccess("Request.Url", false)); 
inputs.Add(All.FindByMemberAccess("Request.Url", false).GetMembersOfTarget()); 
inputs.Add(All.FindByMemberAccess("Request.UrlReferrer", false)); 
inputs.Add(All.FindByMemberAccess("Request.UrlReferrer", false).GetMembersOfTarget()); 
inputs.Add(All.FindByMemberAccess("Request.UserLanguages", false)); 
inputs.Add(All.FindByMemberAccess("Request.UserLanguages", false).GetMembersOfTarget()); 
inputs.Add(All.FindByMemberAccess("RichTextBox.Text", false)); 
inputs.Add(All.FindByMemberAccess("Select.Value", false));
inputs.Add(All.FindByMemberAccess("SelectedItem.Text", false));
inputs.Add(All.FindByMemberAccess("SelectedItem.Value", false));
inputs.Add(All.FindByMemberAccess("TcpClient.GetStream", false));
inputs.Add(All.FindByMemberAccess("TcpClient.AcceptSocket", false)); 
inputs.Add(All.FindByMemberAccess("Text.Value", false));
inputs.Add(All.FindByMemberAccess("TextBox.Text", false)); 
inputs.Add(All.FindByName("Page.Request", false)); 
inputs.Add(All.FindByName("Page.Request.ClientCertificate", false)); 
inputs.Add(All.FindByName("Page.Request.Cookies*", false));
inputs.Add(All.FindByName("Page.Request.Form", false));
inputs.Add(All.FindByName("Page.Request.Headers", false));
inputs.Add(All.FindByName("Page.Request.Params", false));
inputs.Add(All.FindByName("Page.Request.QueryString*", false));
inputs.Add(All.FindByName("Page.Request.RawUrl", false));
inputs.Add(All.FindByName("Page.Request.Url", false));
inputs.Add(All.FindByName("Page.Request.UrlReferrer", false));
inputs.Add(All.FindByName("Request", false)); 
inputs.Add(All.FindByName("Request.ClientCertificate", false));
inputs.Add(All.FindByName("Request.Cookies*", false)); 
inputs.Add(All.FindByName("Request.Form", false));
inputs.Add(All.FindByName("Request.Headers", false));
inputs.Add(All.FindByName("Request.Params", false));
inputs.Add(All.FindByName("Request.QueryString*", false));
inputs.Add(All.FindByName("Request.RawUrl", false));
inputs.Add(All.FindByName("Request.Url", false));
inputs.Add(All.FindByName("Request.UrlReferrer", false));
inputs.Add(Find_Interactive_Inputs_Components());
/* do not include "ViewState" if in web.config exists the following line
<configuration>
	<system.web>
    	<pages ViewStateEncryptionMode  =  "Always" />
	</system.web >
</configuration >
*/

String bindNameStr = string.Empty;
CxList webConfig = Find_Web_Config();

CxList webConfigSyswebPages = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.PAGES.VIEWSTATEENCRYPTIONMODE",false); 

CxList tagBindValue = webConfig.GetByAncs(webConfigSyswebPages.GetFathers()).FindByAssignmentSide(CxList.AssignmentSide.Right);

if (tagBindValue.Count > 0)
{
	StringLiteral bindName = tagBindValue.data.GetByIndex(0) as StringLiteral;
	if (bindName != null)
		bindNameStr = bindName.ShortName.Trim('"');
}

if (string.Compare(bindNameStr, "Always", true) != 0)
{
	CxList The_Right = All.FindByAssignmentSide(CxList.AssignmentSide.Right);
	CxList The_RightAncs = All.GetByAncs(The_Right);
	CxList VS_On_The_Right = The_RightAncs.FindByShortName("ViewState_*",false);
	VS_On_The_Right.Add(The_RightAncs.FindByShortName("ViewState",false).GetMembersOfTarget().FindByShortName("Item",false));
	//inputs = inputs + VS_On_The_Right;
	inputs.Add(VS_On_The_Right);
}

if(!All.isWebApplication)
{
	inputs.Add(All.GetParameters(All.FindByName("*.Main", false).FindByType(typeof(MethodDecl))
		.FindByFieldAttributes(Modifiers.Public | Modifiers.Static)));
}

result = inputs - inputs.FindByAssignmentSide(CxList.AssignmentSide.Left);
result.Add(Find_WS_Inputs());
result.Add(Find_Remoting_Inputs());
result.Add(Find_ASP_MVC_Inputs());

CxList toRemove = Not_Interactive_Inputs();
toRemove.Add(All.FindByMemberAccess("Request.*", false).GetTargetOfMembers());
// Host cannot be controlled by the user
toRemove.Add(result.FindByMemberAccess("Request.Url.Host", false));

result -= toRemove;
//HTTPContext
result.Add(All.FindByMemberAccess("Context.Request", false));
result.Add(All.FindByMemberAccess("HttpContext.Request", false));