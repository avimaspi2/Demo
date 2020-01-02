/*
MISRA C RULE 6-4
------------------------------
This query searches for bit fields of type other than signed/unsigned int

	The Example below shows code with vulnerability: 

int i;

*/

CxList fd = All.FindByType(typeof(FieldDecl));
CxList tr = All.FindByType(typeof(TypeRef));


CxList unrf = All.FindByType(typeof(UnknownReference));
CxList methodInv = All.FindByType(typeof(MethodInvokeExpr));

CxList typedefs = All.FindByName("CX_TYPEDEF").FindByType(typeof(StringLiteral));
CxList tdIntegralTypes = tr.GetByAncs(typedefs.GetAncOfType(typeof(FieldDecl)));

tdIntegralTypes = tdIntegralTypes.FindByShortName("int");
CxList newName = tdIntegralTypes.GetAncOfType(typeof(FieldDecl));
newName = newName.FindByExtendedType("unsigned") + newName.FindByExtendedType("signed");

CxList potentialBf = fd.FindByRegex(@"[\}|\w]+?\s*?:\s*?\d+?\s*?;", false, false, false);
potentialBf = tr.FindByFathers(potentialBf);

CxList integral = potentialBf.FindByShortName("int");
CxList nonIntegral = (potentialBf) - integral;


foreach(CxList temp in newName)
{
	CxList ok = potentialBf.FindByShortName(temp);
	nonIntegral -= ok;
}
result.Add(nonIntegral);
CxList fieldDecls = integral.GetFathers() * fd;
CxList unsigned = fieldDecls.FindByExtendedType("unsigned");
CxList signed = fieldDecls.FindByExtendedType("signed");

result.Add(fieldDecls - (signed + unsigned));