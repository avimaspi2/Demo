// Find all the errstr that appear in a "die" method
CxList die = Find_Methods().FindByShortName("die");
CxList errorMessages = All.FindByShortName("errstr").GetByAncs(die);

// All the eval statements
CxList eval = Find_Conditions().FindByShortName("EVAL").GetAncOfType(typeof(IfStmt));

// Ignoring die's in eval
errorMessages -= errorMessages.GetByAncs(eval);

// For each result, show it with the corresponding "die" method
foreach (CxList message in errorMessages)
{
	result.Add(message.Concatenate(die.FindByParameters(message)));
}