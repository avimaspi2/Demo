// Find_Template_Outputs
CxList tplOutputs = All.NewCxList();
CxList varOcurrences = All.NewCxList();
CxList references = All.NewCxList();

List<string> templateMethods = new List<string> {"New","Must","ParseFiles","ParseGlob"};
CxList members = All.FindByMemberAccess("\"text/template\".*").FindByShortNames(templateMethods);

members.Add(All.FindByMemberAccess("\"html/template\".*").FindByShortNames(templateMethods));
varOcurrences.Add(members.GetAssignee());
varOcurrences.Add(members.GetFathers().GetAssignee()); 
references.Add(All.FindAllReferences(varOcurrences)); 

String[] methods = new string[] {"Execute", "ExecuteTemplate", "Parse"}; 
foreach(String m in methods){
	tplOutputs.Add(references.GetMembersOfTarget().FindByShortName(m));
}

result.Add(tplOutputs);

//template.HTML is a dangerous method
result.Add(All.FindByMemberAccess("\"html/template\".HTML"));