CxList sessionCreation = Find_Session_Create();
if(sessionCreation.Count > 0 && Find_Session_Fixation_Sanitize().Count == 0)
{
	result = sessionCreation;
}