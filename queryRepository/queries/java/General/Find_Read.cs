CxList read = Find_Read_NonDB();

CxList dbRead = Find_Read_DB();

// If it is an Android project - Add Android inputs
CxList androidReads = All.NewCxList();
if(Find_Android_Settings().Count > 0)
{
	androidReads.Add(Find_Android_Read());
}

result = read;
result.Add(dbRead);
result.Add(androidReads);