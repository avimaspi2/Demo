CxList uiWidgets = All.FindAllReferences(Find_UI_Widgets());

CxList personal = Find_Personal_Info();

CxList uiWidgetsPersonal = uiWidgets * personal;

result = uiWidgets.DataInfluencedBy(personal);
result.Add(uiWidgetsPersonal);