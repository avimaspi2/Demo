/* Given a set of UnknownReference-type objects, returns the set of their
   corresponding MethodDecl-type objects. 

   This query extends the functionality of the .FindDefinition built-in
   by also handling "anony*" objects resulting from the usage of 
   "function () {...}" expressions.
*/
if (param.Length == 1 && null != param[0] as CxList)
{
	CxList methodDecls = Find_MethodDecls();
	CxList declarators = Find_Declarators();
		
	CxList inUnkRef = param[0] as CxList;
	CxList inUnkRefMethodDecl = methodDecls.FindDefinition(inUnkRef);
	CxList inUnkRefDeclarator = declarators.FindDefinition(inUnkRef.FindByShortName("anony*"));
	foreach (CxList d in inUnkRefDeclarator)
	{
		string dName = d.GetName(); // anony...var
		string dMethodName = dName.Substring(0, dName.Length - 3); // remove the suffix 'var'
		inUnkRefMethodDecl.Add(methodDecls.FindByShortName(dMethodName));
	}
	
	result.Add(inUnkRefMethodDecl);
}