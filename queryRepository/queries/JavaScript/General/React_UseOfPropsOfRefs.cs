CxList reactAll = React_Find_All();
CxList classComponents = React_Find_ClassComponent();

CxList useOfRefs = reactAll.FindByShortName("refs").GetByAncs(classComponents);
CxList useOfProps = useOfRefs.GetMembersOfTarget().GetMembersOfTarget().FindByShortName("props");

result = useOfProps;