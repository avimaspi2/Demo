result = Find_Read_NonDB();
result.Add(Find_Read_DB());

// If it is an Android project - Add Android inputs
if(Find_Android_Settings().Count > 0)
{
	result.Add(Find_Android_Read());
}