result = Find_Console_Outputs();
result.Add(Find_Web_Outputs());

if(Find_Android_Settings().Count > 0)
{
	result.Add(Find_Android_Outputs());
}

result -= Find_Dead_Code_Contents();