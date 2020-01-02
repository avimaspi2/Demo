CxList inputs = All.NewCxList();    
inputs.Add(All.FindByMemberAccesses(new string[]{
	"CheckBoxList.SelectedItem",
	"CheckBoxList.SelectedValue",
	"CommandEventArgs.CommandArgument",
	"Console.Read*",
	"DataBoundLiteralControl.Text",
	"DataRow.Item",
	"DropDownList.SelectedItem",
	"DropDownList.SelectedValue",
	"file.value",
	"FileUpload.FileContent",
	"FileUpload.FileName",
	"FileUpload.FileBytes",
	"FileUpload.PostedFile*",
	"hidden.value",
	"hidden.Value",
	"HiddenField.Value",
	"HtmlInputFile.Value",
	"HtmlInputHidden.Value",
	"HtmlInputPassword.Value",
	"HtmlInputText.Value",
	"HtmlSelect.Value",
	"HtmlTextArea.Value",
	"HttpPostedFile.ContentType",
	"HttpPostedFile.FileName",
	"ListBox.SelectedItem",
	"ListBox.SelectedValue",
	"ListControl.SelectedValue",
	"ListControl.Text",
	"ListItem.Value",
	"Panel.GroupingText",
	"Panel.ID",
	"password.ID",
	"password.value",
	"password.Value",
	"RadioButton.GroupName",
	"RadioButton.ID",
	"radioButton.Text",
	"RadioButton.Text",
	"RadioButtonList.ID",
	"RadioButtonList.Items",
	"RadioButtonList.SelectedItem",
	"RadioButtonList.SelectedValue",
	"RichTextBox.Text",
	"select.ID",
	"select.value",
	"SelectedItem.Text",
	"SelectedItem.Value",
	"Table.Caption",
	"Table.ID",
	"table.ID",
	"TableCell.Text",
	"TcpClient.GetStream",
	"TcpListener.AcceptSocket",
	"text.ID",
	"text.value",
	"text.Value",
	"textarea.ID",
	"TextBox.ID",
	"TextBox.Text",
	"TreeView.SelectedValue",
	"Wizard.HeaderText",
	"Wizard.ID"}));

inputs.Add(All.FindByMemberAccess("TreeView.SelectedNode").GetMembersOfTarget());
inputs.Add(All.FindByMemberAccess("CommandEventArgs.CommandArgument").GetMembersOfTarget());

CxList requests = Find_Request();
CxList adds = requests.FindByShortName("Add");
CxList adds_targets = adds.GetTargetOfMembers();
adds.Add(adds_targets);
requests -= adds;
inputs.Add(requests);

inputs.Add(Find_Interactive_Inputs_Components());
    
CxList httpRequest = All.FindByType("HttpRequest");
inputs.Add(httpRequest.GetByAncs(httpRequest.FindByType(typeof(IndexerRef))).FindByType(typeof(UnknownReference)));

/* do not include "ViewState" if in web.config exists the following line
<configuration>
    <system.web>
        <pages ViewStateEncryptionMode  =  "Always" />
    </system.web >
</configuration >
*/

String bindNameStr = string.Empty;
CxList webConfig = Find_Web_Config();
CxList webConfigSyswebPages = webConfig.FindByName("CONFIGURATION.SYSTEM.WEB.PAGES.VIEWSTATEENCRYPTIONMODE"); 
CxList tagBindValue = webConfig.GetByAncs(webConfigSyswebPages.GetFathers()).FindByAssignmentSide(CxList.AssignmentSide.Right);

if (tagBindValue.Count > 0)
{
	StringLiteral bindName = tagBindValue.data.GetByIndex(0) as StringLiteral;
	if (bindName != null)
		bindNameStr = bindName.ShortName.Trim('"');
}

CxList The_Right = All.FindByAssignmentSide(CxList.AssignmentSide.Right) + All.FindByType(typeof(ReturnStmt));
The_Right = All.GetByAncs(The_Right);

CxList VS_On_The_Right = All.NewCxList(); 

if (string.Compare(bindNameStr, "Always", true) != 0)
{
	VS_On_The_Right = The_Right.FindByShortName("ViewState_*");
	VS_On_The_Right.Add(The_Right.FindByName("ViewState.Item"));
}
// end of changed/added part


CxList items = The_Right.FindByType(typeof(UnknownReference)) * The_Right.GetByAncs(All.FindByType(typeof(IndexerRef)));
VS_On_The_Right.Add(items.FindByType("DataRow"));

inputs = inputs + VS_On_The_Right;

if(!All.isWebApplication)
{
	inputs.Add(All.GetParameters(All.FindByName("*.Main").FindByType(typeof(MethodDecl))
		.FindByFieldAttributes(Modifiers.Public | Modifiers.Static)));
	inputs.Add(All.FindByMemberAccess("Environment.GetCommandLineArgs"));
}

result = inputs - inputs.FindByAssignmentSide(CxList.AssignmentSide.Left) + 
	Find_WS_Inputs() + Find_ASP_MVC_Inputs()
	- Not_Interactive_Inputs() - All.FindByMemberAccess("Request.*").GetTargetOfMembers();

//HTTPContext
result.Add(All.FindByMemberAccess("Context.Request") + All.FindByMemberAccess("HttpContext.Request"));

result.Add(Find_ASP_MVC_HtmlHelper_Inputs());
result.Add(Find_ASP_MVC_Model_Inputs());

//Get its parameters
CxList allParameters = All.GetParameters(Find_ASP_MVC_Controllers());
result.Add(allParameters);

// Add input parameters of http methods (POST, GET, DELETE, PUT, HEAD, OPTIONS, PATCH)
CxList customAttributes = Find_CustomAttribute().FindByShortNames(new List<string>{"HttpPost", "HttpGet", "HttpPut", "HttpDelete", "HttpPatch", "HttpOptions", "HttpHead"});

// Add input from AjaxMethod
customAttributes.Add(Find_CustomAttribute().FindByShortName("AjaxMethod"));

result.Add(Find_ParamDecl().GetParameters(customAttributes.GetAncOfType(typeof(MethodDecl))));

//find all assign on the left and remove them
CxList assignsExpr = result.GetAncOfType(typeof(AssignExpr));
CxList left = All.FindByAssignmentSide(CxList.AssignmentSide.Left).FindByFathers(assignsExpr);
result -= result.GetByAncs(left);