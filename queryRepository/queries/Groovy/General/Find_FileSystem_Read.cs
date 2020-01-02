/*
	Find_FileSystem_Read
	This query looks for method invocations that read meta-information from
	the file system.
*/
result.Add(Find_FileSystem_Read_Attributes());
result.Add(Find_FileSystem_Read_Date());
result.Add(Find_FileSystem_Read_Permissions());
result.Add(Find_FileSystem_Read_Sizes());
result.Add(Find_FileSystem_Listings());