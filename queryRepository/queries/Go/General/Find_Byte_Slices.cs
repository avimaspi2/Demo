/*
 * This query will return all slice variables with the type byte.
 */

CxList declarators = Find_Declarators().FindByType("byte");
CxList rankSpecifier = All.FindByType(typeof(RankSpecifier));
CxList VariableDecl = rankSpecifier.GetAncOfType(typeof(VariableDeclStmt));

result.Add(declarators.GetByAncs(VariableDecl));