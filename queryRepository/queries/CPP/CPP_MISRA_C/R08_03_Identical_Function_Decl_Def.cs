/*
MISRA C RULE 8-3
------------------------------
This query searches for decleration/definitions of functions which differ in the return type or paramate types

	The Example below shows code with vulnerability:

int sum (char a, char b);

int sum (int a, int b){
	return (a+b);
}

*/

CxList methDecls = All.FindByType(typeof(MethodDecl));
CxList typeRef = All.FindByType(typeof(TypeRef));
//get all method definitions:
CxList methDef = methDecls - All.FindByType(typeof(StatementCollection)).FindByFathers(methDecls).GetFathers();

CxList allMethodsWithSameName = All.FindByType(typeof(MethodDecl)).FindByShortName(methDef);
CxList allMdParams = All.FindByType(typeof(ParamDecl)).GetParameters(methDef);
CxList allMwsnParams = All.FindByType(typeof(ParamDecl)).GetParameters(allMethodsWithSameName);
CxList allMdReturnType = typeRef.FindByFathers(methDef);
CxList allMwsnReturnType = typeRef.FindByFathers(allMethodsWithSameName);
CxList mwsnTypeRef = typeRef.GetByAncs(allMwsnParams);
typeRef = typeRef.GetByAncs(allMdParams);

foreach(CxList md in methDef)
{
	CxList ParamTypes = typeRef.GetByAncs(allMdParams.GetParameters(md));
	CxList returnType = allMdReturnType.FindByFathers(md);		
	CSharpGraph g = md.data.GetByIndex(0) as CSharpGraph;
	CSharpGraph rt = returnType.data.GetByIndex(0) as CSharpGraph;	
	CxList methodsWithSameName = allMethodsWithSameName.FindByShortName(g.ShortName) - md;
	CxList nonCompliant = All.NewCxList();
	foreach(CxList mwsn in methodsWithSameName)
	{
		CxList retTypeOfOthers = allMwsnReturnType.FindByFathers(mwsn);
		CSharpGraph rtoo = retTypeOfOthers.data.GetByIndex(0) as CSharpGraph;
		
		if(rtoo.ShortName != rt.ShortName)
		{
			result.Add(mwsn + md);
			nonCompliant.Add(mwsn);
			allMethodsWithSameName -= mwsn + md;
		}	
	}
	methodsWithSameName -= nonCompliant;
	foreach(CxList mwsn in  methodsWithSameName)
	{
		CxList otherParamTypes = mwsnTypeRef.GetByAncs(allMwsnParams.GetParameters(mwsn));
		if(otherParamTypes.Count != ParamTypes.Count)
		{
			result.Add(mwsn + md);
			allMethodsWithSameName -= mwsn + md;
			continue;
		}
		if(otherParamTypes.Count == 0 && ParamTypes.Count == 0)
		{
			continue;
		}
		if((otherParamTypes.FindByShortName("void").Count > 0 && ParamTypes.Count == 0) ||
			(ParamTypes.FindByShortName("void").Count > 0 && otherParamTypes.Count == 0) ||
			(ParamTypes.FindByShortName("void").Count > 0 && otherParamTypes.FindByShortName("void").Count > 0))
		{	
			continue;
		}
		
		CxList defParamColl = ParamTypes.GetAncOfType(typeof(ParamDeclCollection));
		CxList declParamColl = otherParamTypes.GetAncOfType(typeof(ParamDeclCollection));
		for (int i = 0; i < defParamColl.Count; i++)
		{
			CSharpGraph def = ParamTypes.data.GetByIndex(i) as CSharpGraph;
			CSharpGraph decl = otherParamTypes.data.GetByIndex(i) as CSharpGraph;
			if(def.ShortName != decl.ShortName)
			{
				result.Add(mwsn + md);
				allMethodsWithSameName -= mwsn + md;
			}
		}
	}	
}