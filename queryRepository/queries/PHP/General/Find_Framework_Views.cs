CxList view = All.FindByFileName(@"*views\*")+All.FindByFileName(@"*view\*");
view.Add(
	All.FindByFileName("*.phtml") + 
	All.FindByFileName("*.phtm") +
	Find_Ctp_Files() +
	Find_Twig()	);
result.Add(view);