CxList methods = Find_Methods();

result.Add(methods.FindByMemberAccesses(new string[]{
	// javax.swing.AbstractButton
	"AbstractButton.getLabel",
	"AbstractButton.setLabel",
	// javax.swing.FocusManager
	"FocusManager.disableSwingFocusManager",
	"FocusManager.isFocusManagerEnabled",
	// javax.swing.JComponent
	"JComponent.disable",
	"JComponent.enable",
	"JComponent.getNextFocusableComponent",
	"JComponent.isManagingFocus",
	"JComponent.requestDefaultFocus",
	"JComponent.reshape?",
	"JComponent.setNextFocusableComponent",
	// javax.swing.JInternalFrame
	"JInternalFrame.getMenuBar",
	"JInternalFrame.setMenuBar",
	// javax.swing.JList<E>
	"JList.getSelectedValues",
	// javax.swing.JMenuBar
	"JMenuBar.getComponentAtIndex",
	// javax.swing.JPasswordField
	"JPasswordField.getText",
	// javax.swing.JPopupMenu
	"JPopupMenu.getComponentAtIndex",
	// javax.swing.JRootPane
	"JRootPane.getMenuBar",
	"JRootPane.setMenuBar",
	// javax.swing.JTable
	"JTable.createScrollPaneForTable",
	"JTable.sizeColumnsToFit",
	// javax.swing.JViewport
	"JViewport.isBackingStoreEnabled",
	"JViewport.setBackingStoreEnabled",
	// javax.swing.plaf.basic.BasicSplitPaneUI
	"BasicSplitPaneUI.getDividerBorderSize",
	// javax.swing.plaf.metal.MetalComboBoxUI
	"MetalComboBoxUI.editablePropertyChanged",
	"MetalComboBoxUI.removeListeners",
	// javax.swing.ScrollPaneLayout
	"ScrollPaneLayout.getViewportBorderBounds",
	// javax.swing.SwingUtilities
	"SwingUtilities.findFocusOwner",
	// javax.swing.table.TableColumn
	"TableColumn.disableResizedPosting",
	"TableColumn.enableResizedPosting",
	// javax.swing.text.html.HTMLEditorKit.InsertHTMLTextAction
	"InsertHTMLTextAction.insertAtBoundry",
	// javax.swing.text.LabelView
	"LabelView.getFontMetrics",
	// javax.swing.text.TableView
	"TableView.createTableCell",
	// javax.swing.text.TableView.TableCell
	"TableCell.getColumnCount",
	"TableCell.getGridColumn",
	"TableCell.getGridRow",
	"TableCell.getRowCount",
	"TableCell.setGridLocation"}));

// javax.swing.plaf.basic.BasicSplitPaneUI
CxList temp = methods.FindByMemberAccess("BasicSplitPaneUI.createKeyboard*");
result.Add(temp.FindByShortNames(new List<string>{
	"createKeyboardDownRightListener",
	"createKeyboardEndListener",
	"createKeyboardHomeListener",
	"createKeyboardResizeToggleListener",
	"createKeyboardUpLeftListener"}));

// javax.swing.KeyStroke
CxList methodCalls = methods.FindByMemberAccess("KeyStroke.getKeyStroke");
CxList methodParameters = All.GetParameters(methodCalls, 1);
CxList toAdd = methodParameters.FindByType("bool");
toAdd.Add(methodParameters.FindByType(typeof(BooleanLiteral)));
result.Add(methodCalls.FindByParameters(toAdd));

// javax.swing.text.View.modelToView(int, Shape) is deprecated 
CxList modelToView = methods.FindByMemberAccess("View.modelToView");
CxList modelToView_3rd_Params = All.GetParameters(modelToView, 2);
result.Add(modelToView - modelToView.FindByParameters(modelToView_3rd_Params));

// javax.swing.text.View.viewToModel(float, float, Shape) is deprecated (was replaced with a 4-args equivalent)
CxList viewToModel = methods.FindByMemberAccess("View.viewToModel");
CxList viewToModel_4th_Params = All.GetParameters(viewToModel, 3);
result.Add(viewToModel - viewToModel.FindByParameters(viewToModel_4th_Params));