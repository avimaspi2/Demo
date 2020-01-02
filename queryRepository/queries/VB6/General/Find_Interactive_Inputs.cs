result = 
	Find_Methods().FindByShortName("InputBox", false) + 
	All.FindByName("*.text");

CxList textFields = All.FindByRegex(@"(?<=Begin(\s)+VB\.TextBox(\s)+).+");
CxList comboFields = All.FindByRegex(@"(?<=Begin(\s)+VB\.ComboBox(\s)+).+");
CxList listFields = All.FindByRegex(@"(?<=Begin(\s)+VB\.ListBox(\s)+).+");

result += All.FindAllReferences(textFields + comboFields + listFields);