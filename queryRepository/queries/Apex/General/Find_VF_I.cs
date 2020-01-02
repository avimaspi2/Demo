//result = All.FindByFathers(Find_VF_Pages().FindByShortName("$$i*"));
CxList o = Find_VF_Pages().FindByShortName("$i*");
int counter = 0;
while (o.Count > 0 && counter++ < 100)
{
	result.Add(o.FindByType(typeof(MethodInvokeExpr)));
	o = o.GetMembersOfTarget();
}