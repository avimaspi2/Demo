result = NodeJS_Find_Integers();
result.Add(Find_Integers());
if(Hapi_Find_Server_Instance().Count > 0)
{
	result.Add(Hapi_Find_Sanitize());
}