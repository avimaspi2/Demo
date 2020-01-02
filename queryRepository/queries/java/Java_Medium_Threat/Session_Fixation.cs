CxList temp = Find_Session_Create();
if(temp.Count > 0 && Find_Session_Fixation_Sanitize().Count == 0)
{
	result = temp;
}