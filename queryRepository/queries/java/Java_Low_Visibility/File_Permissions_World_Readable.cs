//File permission World readable

CxList fileReadable = All.FindByMemberAccess("File.setReadable");
            
CxList firstPrm = All.GetParameters(fileReadable, 0).FindByType(typeof(BooleanLiteral)); // true: File Readable
CxList secondPrm=All.GetParameters(fileReadable, 1).FindByType(typeof(BooleanLiteral));// true: Readable for the owner
CxList analyzePermissions = All.NewCxList();
if (firstPrm.FindByAbstractValue(abstractValue => abstractValue is TrueAbstractValue).Count > 0)
{
	analyzePermissions.Add(firstPrm);
}
if (secondPrm.FindByAbstractValue(abstractValue => abstractValue is FalseAbstractValue).Count > 0)
{
	analyzePermissions.Add(secondPrm);
}
if (analyzePermissions.Count==2) 
{
	result = firstPrm;
}