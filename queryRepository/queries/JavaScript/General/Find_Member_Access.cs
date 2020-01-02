if (param.Length > 0)
{
	string parameters = param[0] as string;
	string param1 = parameters.Substring(0, parameters.IndexOf('.'));
	string param2 = parameters.Substring(parameters.IndexOf('.') + 1);

	CxList createObj = Find_ObjectCreations();
	CxList param1Create = createObj.FindByShortName(param1);
	result.Add(Find_Command_From_Http(param1Create, param2));
}