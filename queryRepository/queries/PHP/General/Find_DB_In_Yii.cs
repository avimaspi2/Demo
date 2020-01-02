/* 
 *  Finds DB methods for Yii framework
 *  yii\db\Command list at http://www.yiiframework.com/doc-2.0/yii-db-command.html
 *  yii\db\Connection options and available syntax at http://www.yiiframework.com/doc-2.0/yii-db-command.html
 *  Query Builders at http://www.yiiframework.com/doc-2.0/yii-db-querybuilder.html
 */
CxList vars = All.FindByType(typeof(UnknownReference));
CxList leftSideVars = vars.FindByAssignmentSide(CxList.AssignmentSide.Left);
 
//find all \Yii::$app and Yii::app() occurrences 
//(including db method)
CxList yiiAppOccur = All.FindByMemberAccess("Yii.app");
//find all \\yii\\db\\Connection
CxList yiiConnections = All.FindByShortName("\\yii\\db\\Connection");

//find all variables assigned to yiiApp occurrences
CxList leftSideAncs = yiiAppOccur.GetAncOfType(typeof(AssignExpr));
CxList yiiConnectionsAncs = yiiConnections.GetAncOfType(typeof(FieldDecl));
CxList yiiVars = leftSideVars.GetByAncs(leftSideAncs) +
	leftSideVars.FindByName(yiiConnectionsAncs);

//find all occurrences of the variables assigner to yiiApp occurrences
CxList yiiVarsOccurs = vars.FindAllReferences(yiiVars);

//find all Yii methods
CxList yiiMethods = yiiAppOccur.GetMembersOfTarget().GetMembersOfTarget()
	+ yiiVarsOccurs.GetMembersOfTarget();

//filter by DB methods
//->createCommand("<a sql query>")
CxList yiiCreateCommand = yiiMethods.FindByShortName("createCommand");
//->createCommand-><a query builder>
CxList yiiQueryBuilders = yiiCreateCommand.GetMembersOfTarget();
//Remove createCommands with queryBuilders
yiiCreateCommand -= yiiQueryBuilders.GetTargetOfMembers();

result = yiiCreateCommand + yiiQueryBuilders;