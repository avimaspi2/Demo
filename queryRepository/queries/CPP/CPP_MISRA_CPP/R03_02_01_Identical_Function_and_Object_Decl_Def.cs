/*
MISRA CPP RULE 3-2-1
------------------------------
This query searches for decleration/definitions of functions which differ in the return type or paramater types
and of other objects.

                The Example below shows code with vulnerability:

int sum (char a, char b);

int sum (int a, int b){
                return (a+b);
}

*/

CxList methDecls = All.FindByType(typeof(MethodDecl));
CxList typeRef = All.FindByType(typeof(TypeRef));
CxList parameters = All.FindByType(typeof(ParamDecl));
CxList voidType = All.FindByShortName("void");
//get all method definitions:
CxList methDef = methDecls - All.FindByType(typeof(StatementCollection)).FindByFathers(methDecls).GetFathers();
CxList allMethodsWithSameName = methDecls.FindByShortName(methDef);
CxList allMdParams = parameters.GetParameters(methDef);

CxList allMwsnParams = allMdParams.GetParameters(allMethodsWithSameName);
CxList allMdReturnType = typeRef.FindByFathers(methDef);
CxList allMwsnReturnType = typeRef.FindByFathers(allMethodsWithSameName);
CxList mwsnTypeRef = typeRef.FindByFathers(allMwsnParams);

typeRef = typeRef.FindByFathers(allMdParams);

CxList updatedTR = typeRef.FindByFathers(allMdParams.GetParameters(methDef));
CxList rtype = allMdReturnType.FindByFathers(methDef);



foreach(CxList md in methDef)
{
	CxList ParamTypes = updatedTR.FindByFathers(allMdParams.GetParameters(md));
	CxList returnType = rtype.FindByFathers(md);   
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
	foreach(CxList mwsn in methodsWithSameName)
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
		if(((otherParamTypes * voidType).Count > 0 && ParamTypes.Count == 0) ||
			((ParamTypes * voidType).Count > 0 && otherParamTypes.Count == 0) ||
			((ParamTypes * voidType).Count > 0 && (voidType * otherParamTypes).Count > 0))
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

CxList decls = Find_All_Declarators();
typeRef = All.FindByType(typeof(TypeRef)).GetByAncs(decls);
CxList cltrs = typeRef.GetByAncs(decls);
// go over all declarators with a name that appears twice
foreach (CxList decl in decls) {
	CxList curType = cltrs.GetByAncs(decl);
	// go over all methods with same name as the current reference
	CxList sameNameDecls = decls.FindByShortName(decl) - decl;
	CxList sameTypeRef = typeRef.GetByAncs(sameNameDecls);
	foreach(CxList compDecl in sameNameDecls) {
		CxList compType = sameTypeRef.GetByAncs(compDecl);
		if (compType.FindByName(curType).Count == 0) {
			result.Add(sameNameDecls + decl);
			decls -= sameNameDecls + decl;
			typeRef -= sameTypeRef;
			break;
		}
	}
}