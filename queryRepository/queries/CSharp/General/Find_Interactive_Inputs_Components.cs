// Telerik.Windows.Controls
result.Add(All.FindByMemberAccesses(new string[]{
	"ExtendedTextBox.Text",
	"RadMaskedTextBox.Value",
	"WatermarkedTextBox.Text",
	"DistinctValuesList.SelectedItem",
	"DistinctValuesList.SelectedValue",
	"Cells.Content",
	"GridViewCell.Value",
	"GridViewCell.Content",
	"GridViewCell.GetValue",
	"GridViewCell.SetValue",
	"GridViewCellBase.GetValue",
	"GridViewCellBase.SetValue",
	"GridViewHeaderCell.GetValue", 
	"GridViewHeaderCell.SetValue",
	"GridViewRow.Cells",

	// ComponentArt
	"MaskedInput.Text",
	"NumberInput.Value",  // numbers only
	"GridCell.Text",
	"GridCell.Value",
	"TreeViewNode.Text",
	"TreeViewNode.Value",
	"Calendar.formatDate",  // Date only
	"Editor.ContentText", 
	"Editor.ContentHTML",
	"DataGridCell.Text",
	"DataGridCell.Value",
	"DataGridGroup.Value",
	"DataGridGroup.ValueText",
	"UploadItem.FileName",
	
	// Infragistics
	"GroupByAreaFieldListBox.SelectedItem",
	"GroupByAreaFieldListBox.SelectedValue",
	"CellValuePresenter.Text"}));
CxList grp_Cell = All.FindByType("Cell", true);
result.Add(grp_Cell.GetMembersOfTarget().FindByShortNames(new List <string> {"Text","Value","ConvertedValue"}));
result.Add(All.FindByMemberAccesses(new string[]{
	"CellAutomationPeer.Value",
	"IGControlBase.SetValue",
	"IGControlBase.GetValue"}));
	
// FarPoint
CxList grp_Cells = All.FindByType("Cells", true);
result.Add(grp_Cells.GetMembersOfTarget().FindByShortNames(new List <string> {"Text","Value"}));
result.Add(All.FindByMemberAccesses(new string[]{
	"FpBoolean.CheckState",  // Boolean
	"FpCalendar.CurrentDate",
	"FpClock.CurrentTime",
	"FpCurrency.Text",
	"FpCurrency.Value",
	"FpDateTime.Text",
	"FpDateTime.Value",
	"FpDouble.Text",
	"FpDouble.Value",
	"FpInteger.Text",
	"FpInteger.Value",
	"FpMask.Text",
	"FpMask.Value",
	"FpPercent.Text",
	"FpPercent.Value",
	"FpSuperEdit.Text",
	"FpSuperEdit.Value",
	"FpText.Text",
	"FpText.Value"}));