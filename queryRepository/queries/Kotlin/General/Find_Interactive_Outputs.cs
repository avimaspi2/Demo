result = Find_Console_Outputs();

if(Find_Android_Settings().Count > 0)
{
	result.Add(Find_Android_Outputs());
}