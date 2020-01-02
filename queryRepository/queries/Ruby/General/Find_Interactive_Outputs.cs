CxList view = Find_View_Code();
CxList responseWrite = view.FindByShortName("responseWrite");

result = view.GetParameters(responseWrite) + Find_Console_Outputs();