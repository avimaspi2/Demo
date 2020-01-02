CxList vfPages = Find_VF_Pages();
CxList actions = 
	vfPages.FindByName("cx_actionfunction") +
	vfPages.FindByName("cx_actionpoller") +
	vfPages.FindByName("cx_actionsupport") +
	vfPages.FindByName("cx_commandbutton") +
	vfPages.FindByName("cx_commandlink")
	;
actions = vfPages.GetMethod(actions);
actions = Find_Methods().GetByAncs(actions);

result = actions;