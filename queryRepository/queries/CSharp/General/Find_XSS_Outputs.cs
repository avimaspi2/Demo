CxList outputsInvoke = All.NewCxList();
outputsInvoke.Add(All.FindByMemberAccesses(new string[]{
	"AttributeCollection.Add",
	"AttributeCollection.Item",
	"CheckBoxList*.Items",
	"HtmlTextWriter.Write",
	"HtmlTextWriter.WriteLine",
	"HttpResponse.*",
	"RadioButtonList.Items",
	"Response.Write",
	"ViewData.*",
	"ViewBag.*"}));

CxList outputsAssignmentSide = All.FindByMemberAccesses(new string[]{
	"AdRotator.ID",
	"AdRotator.Target",
	"applet.ID",
	"applet.InnerHtml",
	"applet.TagName",
	"body.ID",
	"body.InnerHtml",
	"body.TagName",
	"BulletedList.ID",
	"Button.ID",
	"Calendar.Caption",
	"Calendar.ID",
	"Calendar.NextMonthText",
	"Calendar.PrevMonthText",
	"CheckBox.ID",
	"CheckBox.Text",
	"CheckBoxList.ID",
	"Control.ID",
	"div.ID",
	"div.InnerHtml",
	"DropDownList.ID",
	"embed.ID",
	"embed.InnerHtml",
	"embed.TagName",
	"FileUpload.ID",
	"hidden.ID",
	"HiddenField.ID",
	"hr.ID",
	"hr.InnerHtml",
	"html.ID",
	"html.InnerHtml",
	"html.TagName",
	"HtmlContainerControl.InnerHtml",
	"HtmlContainerControl.InnerText",
	"HtmlInputControl.Value",
	"HtmlInputFile.ID",
	"HtmlInputPassword.ID",
	"HtmlInputText.ID",
	"HtmlSelect.ID",
	"HtmlTextArea.ID",
	"HyperLink.ID",
	"HyperLink.NavigateUrl",
	"HyperLink.Target",
	"HyperLink.Text",
	"iframe.ID",
	"iframe.InnerHtml",
	"iframe.TagName",
	"Image.ID",
	"Image.ImageUrl",
	"ImageButton.ID",
	"ImageMap.ID",
	"img.ID",
	"img.Src",
	"input.ID",
	"label.ID"});

CxList lables = All.FindByMemberAccess("label.Text");
lables.Add(All.FindByMemberAccess("label.Text")); 

CxList windowsFormLables = All.FindByType("*forms.label");
CxList formTargets = lables.GetMembersWithTargets(windowsFormLables);
outputsAssignmentSide.Add(lables - formTargets);

outputsAssignmentSide.Add(All.FindByMemberAccesses(new string[]{
	"LinkButton.ID",
	"LinkButton.Text",
	"ListBox.ID",
	"Literal.ID",
	"Literal.Text",
	"Localize.Text",
	"meta.ID",
	"Panel.GroupingText",
	"Panel.ID",
	"password.ID",
	"RadioButton.GroupName",
	"RadioButton.ID",
	"RadioButton.Text",
	"RadioButtonList.ID",
	"select.ID",
	"Table.Caption",
	"Table.ID",
	"table.ID",
	"TableCell.Text",
	"text.ID",
	"textarea.ID",
	"TextBox.ID",
	"Wizard.HeaderText",
	"Wizard.ID",
	"DataList.DataSource",
	"DataGrid.DataSource",
	"HtmlGenericControl.InnerHtml",
	"Button.PostBackUrl"}));

CxList outputs = All.NewCxList();	
outputs = All.GetAssignee(outputsAssignmentSide);

outputs.Add(Find_Web_Outputs_Components());
outputs.Add(outputsInvoke);

CxList fathersWithoutAssignment = outputs.GetFathers() - 
	outputs.GetFathers().FindByType(typeof(AssignExpr));

result.Add(outputs.FindByAssignmentSide(CxList.AssignmentSide.Left));
result.Add(outputs.FindByFathers(fathersWithoutAssignment));
result.Add(Find_Response());

result.Add(All.FindByName("gridview2_aspx.AuthorsGridView_RowUpdating.e.NewValues"));
result.Add(All.FindAllReferences(All.FindDefinition(
	All.FindByName("gridview2_aspx.AuthorsGridView_RowUpdating.e.NewValues"))));

CxList stream = All.FindByTypes(new string[] {"BufferedStream","Stream","StreamWriter"});

CxList currentOutputs = All.NewCxList();
currentOutputs.Add(result);

CxList assign = stream.GetAncOfType(typeof(AssignExpr));

CxList assignAndStream = All.NewCxList();
assignAndStream.Add(assign);
assignAndStream.Add(stream);

CxList outInStream = currentOutputs.GetByAncs(assignAndStream);
CxList newOutputs = All.NewCxList();
int cnt = 10;
while (outInStream.Count > 0 && cnt-- > 0)
{
	newOutputs.Add(stream * outInStream);
	outInStream = outInStream.GetFathers();
}

result.Add(All.FindAllReferences(newOutputs).GetMembersOfTarget().FindByShortName("Write*"));


CxList membersOfCS = All.FindByMemberAccess("ClientScript.Register*").FindByType(typeof (MethodInvokeExpr));
membersOfCS.Add(All.FindByMemberAccess("ClientScriptManager.Register*").FindByType(typeof (MethodInvokeExpr)));

CxList thirdParamMethods = membersOfCS.FindByShortName("RegisterStartupScript");
thirdParamMethods.Add(membersOfCS.FindByShortName("RegisterClientScriptBlock"));
thirdParamMethods.Add(membersOfCS.FindByShortName("RegisterOnSubmitStatement"));

CxList thirdParam = All.GetParameters(thirdParamMethods, 2);

CxList firstAndSecParamMeth = membersOfCS.FindByShortName("RegisterArrayDeclaration");
CxList firstAndSecParam = All.GetParameters(firstAndSecParamMeth);

CxList SecParamMeth = membersOfCS.FindByShortName("RegisterClientScriptInclude");
CxList SecParam = All.GetParameters(SecParamMeth, 1);

CxList SecAndThirdParamMeth = membersOfCS.FindByShortName("RegisterExpandoAttribute");
CxList SecAndThirdParam = All.GetParameters(SecAndThirdParamMeth, 1);
SecAndThirdParam.Add(All.GetParameters(SecAndThirdParamMeth, 2));

CxList ClientScriptOut = All.NewCxList();
ClientScriptOut.Add(thirdParam);
ClientScriptOut.Add(firstAndSecParam);
ClientScriptOut.Add(SecParam);
ClientScriptOut.Add(SecAndThirdParam);

result.Add(ClientScriptOut);
result.Add(Find_ASP_MVC_Outputs());

result.Add(Find_ASP_MVC_HtmlHelper_Outputs());

CxList methods = Find_Methods();
CxList objCreate = Find_ObjectCreations();
CxList safeNetComponents = All.NewCxList(); 
safeNetComponents.Add(methods.FindByShortName("response.AddHeader", false));
safeNetComponents.Add(methods.FindByShortName("resp.AddHeader", false)); 
safeNetComponents.Add(methods.FindByMemberAccess("Response.Headers.Add"));
safeNetComponents.Add(methods.FindByMemberAccess("Response.ContentType"));
result -= safeNetComponents;
result.Add(objCreate.FindByTypes(new String[]{"Microsoft.AspNetCore.Html.HtmlString","Microsoft.AspNetCore.Html.HtmlFormattableString"}));
result -= result.InfluencedBy(Find_Safe_Response());