CxList methods = Find_Methods();
CxList apex = Find_Apex_Files();

CxList str = apex.FindByType(typeof(StringLiteral));
CxList selectParameters = apex.GetParameters(methods.FindByShortName("select"));
CxList sel = str.FindByShortName("select*").GetByAncs(selectParameters);

sel -= sel.FindByShortName("*where*");
sel -= sel.FindByShortName("*limit*");

result = sel;

result -= Find_Test_Code();