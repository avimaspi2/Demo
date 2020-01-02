/* This query is meant to find methods that encode html characters (such as '<', '>', '&', ... ) to prevent
 * them from being interpreted as HTML when injected in a page
 */
CxList methods = Find_Methods();
result = methods.FindByShortName("encode", false);
result.Add(Find_Builtin_Types() - Find_Builtin_Char_Types());