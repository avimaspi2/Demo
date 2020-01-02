/*The query will look for any modification operation on the database and will return result if there are
No Log writing in the same class context*/

//look for all DB in operations
CxList dbIn = general.Find_DB_In();
CxList dbModifyingCommand = general.Find_DB_Alteration_Command();

CxList stringLiterals = general.Find_String_Literal();
List<string> modifierList = new List<string>{"insert*","update*","delete*","drop","create*","alter*"};
//find strings that start with db modification related query
CxList dbModifiers = stringLiterals.FindByShortNames(modifierList, false);


//look only for db which is related to db modification
dbModifiers = dbIn.DataInfluencedBy(dbModifiers);

dbModifiers.Add(dbIn.FindByShortNames(modifierList, false));
dbModifiers.Add(dbIn.GetTargetOfMembers().FindByShortNames(modifierList, false));
dbModifiers.Add(dbModifyingCommand);
 
CxList eliminateQuery = dbModifiers.FindByShortName("createQuery", false);
dbModifiers -= eliminateQuery;
//find all writes to the log
CxList logs = general.Find_Log_Outputs();

//check whether the writes to the log and the db modification operation share the same class.
CxList encapsulatingClassOfLogWrite = logs.GetAncOfType(typeof(ClassDecl));
CxList encapsulatingClassOfDBMod = dbModifiers.GetAncOfType(typeof(ClassDecl));
CxList anonymousClasses = general.Find_Anonymous_Classes();
// If the db modification operation is inside an anonymous class - try to get the encapsulating class instead
CxList dbModifiersInAnonymousClasses = anonymousClasses * encapsulatingClassOfDBMod;
encapsulatingClassOfDBMod -= dbModifiersInAnonymousClasses;
encapsulatingClassOfDBMod.Add(dbModifiersInAnonymousClasses.GetFathers().GetAncOfType(typeof(ClassDecl)));

encapsulatingClassOfDBMod -= encapsulatingClassOfLogWrite;
result = dbModifiers.GetByAncs(encapsulatingClassOfDBMod);