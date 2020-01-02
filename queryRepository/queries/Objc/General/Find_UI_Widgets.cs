CxList uiWidgets = All.FindByType("UITextField");
uiWidgets.Add(All.FindByType("UILabel"));
uiWidgets.Add(All.FindByType("UIView"));
uiWidgets.Add(All.FindByType("UITextView"));
uiWidgets.Add(All.FindByType("UITableViewCell"));

uiWidgets.Add(All.InheritsFrom(All.FindDefinition(uiWidgets)));
uiWidgets.Add(All.InheritsFrom("UITextField"));
uiWidgets.Add(All.InheritsFrom("UILabel"));
uiWidgets.Add(All.InheritsFrom("UIView"));
uiWidgets.Add(All.InheritsFrom("UITextView"));
uiWidgets.Add(All.InheritsFrom("UITableViewCell"));
result = uiWidgets;