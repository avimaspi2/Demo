CxList methods = Find_Methods();

result.Add(methods.FindByMemberAccesses(new string[]{
	// java.awt.CheckboxGroup
	"CheckboxGroup.getCurrent",
	"CheckboxGroup.setCurrent",
	// java.awt.Choice
	"Choice.countItems",
	// java.awt.Container
	"Container.countComponents",
	"Container.deliverEvent",
	"Container.insets",
	"Container.layout",
	"Container.locate",
	"Container.minimumSize",
	"Container.preferredSize",
	// java.awt.DataFlavor
	"DataFlavor.normalizeMimeType",
	"DataFlavor.normalizeMimeTypeParameter",
	// java.awt.Font
	"Font.getPeer",
	// java.awt.FontMetrics
	"FontMetrics.getMaxDecent",
	// java.awt.Frame
	"Frame.getCursorType",
	"Frame.setCursor",
	// java.awt.Graphics
	"Graphics.finalize",
	"Graphics.getClipRect",
	// java.awt.event.KeyEvent
	"KeyEvent.getKeyModifiersText",
	"KeyEvent.setModifiers",
	// java.awt.image.renderable.RenderContext
	"RenderContext.concetenateTransform",
	"RenderContext.preConcetenateTransform",
	// java.awt.Menu
	"Menu.countItems",
	// java.awt.MenuBar
	"MenuBar.countMenus",
	// java.awt.MenuComponent
	"MenuComponent.getPeer",
	"MenuComponent.postEvent",
	// java.awt.MenuContainer
	"MenuContainer.postEvent",
	// java.awt.MenuItem
	"MenuItem.disable",
	"MenuItem.enable",
	// java.awt.Polygon
	"Polygon.getBoundingBox",
	"Polygon.inside",
	// java.awt.Rectangle
	"Rectangle.inside",
	"Rectangle.move",
	"Rectangle.reshape",
	"Rectangle.resize",
	// java.awt.Scrollbar
	"Scrollbar.getLineIncrement",
	"Scrollbar.getPageIncrement",
	"Scrollbar.getVisible",
	"Scrollbar.setLineIncrement",
	"Scrollbar.setPageIncrement",
	// java.awt.ScrollPane
	"ScrollPane.layout",
	// java.awt.TextArea
	"TextArea.appendText",
	"TextArea.insertText",
	"TextArea.minimumSize",
	"TextArea.preferredSize",
	"TextArea.replaceText",
	// java.awt.TextField
	"TextField.minimumSize",
	"TextField.preferredSize",
	"TextField.setEchoCharacter",
	// java.awt.Toolkit
	"Toolkit.getFontList",
	"Toolkit.getFontMetrics",
	"Toolkit.getFontPeer",
	"Toolkit.getMenuShortcutKeyMask",
	// java.awt.Window
	"Window.applyResourceBundle",
	"Window.hide",
	"Window.postEvent",
	"Window.reshape",
	"Window.show"}));

// java.awt.Component
var temp = methods.FindByMemberAccess("Component.*");
result.Add(temp.FindByShortNames(new List<string>{
	"action",
	"bounds",
	"deliverEvent",
	"disable",
	"enable",
	"getPeer",
	"gotFocus",
	"handleEvent",
	"hide",
	"inside",
	"isFocusTraversable",
	"keyDown",
	"keyUp",
	"layout",
	"locate",
	"location",
	"lostFocus",
	"minimumSize",
	"mouseDown",
	"mouseDrag",
	"mouseEnter",
	"mouseExit",
	"mouseMove",
	"mouseUp",
	"move",
	"nextFocus",
	"postEvent",
	"preferredSize",
	"reshape",
	"resize",
	"show",
	"size"}));

// Referrent to java.awt.List, not java.util.List
temp = methods.FindByMemberAccess("List.*");
result.Add(temp.FindByShortNames(new List<string>{
	"addItem",
	"allowsMultipleSelections",
	"countItems",
	"delItem",
	"delItems",
	"isSelected",
	"minimumSize",
	"preferredSize",
	"setMultipleSelections"}));

// java.awt.BorderLayout.addLayoutComponent(string, Component) is deprecated
CxList BL_addLayoutComponent = methods.FindByMemberAccess("BorderLayout.addLayoutComponent");
CxList BL_addLayoutComponent_1st_Param = All.GetParameters(BL_addLayoutComponent, 0).FindByType("Component"); 
result.Add(BL_addLayoutComponent - BL_addLayoutComponent.FindByParameters(BL_addLayoutComponent_1st_Param));

// java.awt.CardLayout.addLayoutComponent(string, Component) is deprecated
CxList CL_addLayoutComponent = methods.FindByMemberAccess("CardLayout.addLayoutComponent");
CxList CL_addLayoutComponent_1st_Param = All.GetParameters(CL_addLayoutComponent, 0).FindByType("Component"); 
result.Add(CL_addLayoutComponent - CL_addLayoutComponent.FindByParameters(CL_addLayoutComponent_1st_Param));

// java.awt.ComponentOrientation.getOrientation(ResourceBundle) is deprecated
CxList getOrientation = methods.FindByMemberAccess("ComponentOrientation.getOrientation");
CxList getOrientation_1st_Param = All.GetParameters(getOrientation, 0).FindByType("ResourceBundle"); 
result.Add(getOrientation.FindByParameters(getOrientation_1st_Param));

// java.awt.DataFlavor.equals( string ) is deprecated
CxList DF_equals = methods.FindByMemberAccess("DataFlavor.equals");
CxList DF_equals_1st_Param = All.GetParameters(DF_equals, 0).FindByType("StringLiteral"); 
DF_equals_1st_Param.Add(All.GetParameters(DF_equals, 0).FindByType("String"));
result.Add(DF_equals.FindByParameters(DF_equals_1st_Param));