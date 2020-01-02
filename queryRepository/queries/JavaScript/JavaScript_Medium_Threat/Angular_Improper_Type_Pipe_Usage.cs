CxList pipes = Angular_Find_Pipes();

Func < CxList, string[], CxList > filterByDeclaredTypes = (searchSpace, types) => {
		CxList filtered = All.NewCxList();
		CxList definitions = All.FindDefinition(searchSpace);
		foreach( string t in types) {
			filtered.Add(definitions.FindByRegex(@"\s*:\s*" + t));
		}
		return searchSpace.FindAllReferences(filtered);
	};

Func<string[], string[], CxList> improperTypeFinder = (pipeClassNames, expectedTypeNames) =>
	{
	CxList pipeTransformCalls = All.NewCxList();
	foreach (var pipeClassName in pipeClassNames)
	{
		pipeTransformCalls.Add(pipes.FindByMemberAccess(pipeClassName + ".transform"));
	}
	CxList firstParamToTransformCall = All.GetParameters(pipeTransformCalls, 0).FindByType(typeof(UnknownReference));	
	CxList isExpectedType = filterByDeclaredTypes(firstParamToTransformCall,expectedTypeNames);
	CxList isNotExpectedType = firstParamToTransformCall - isExpectedType;
	
	return isNotExpectedType;
	};


result.Add(improperTypeFinder(new string[] { "CurrencyPipe", "DecimalPipe", "PercentPipe" },
	new string[] { "number", "string" }));

result.Add(improperTypeFinder(new string[] { "I18nSelectPipe", "LowerCasePipe", "UpperCasePipe" },
	new string[] { "string" }));

result.Add(improperTypeFinder(new string[] { "AsyncPipe" },
	new string[] { "Promise" }));

result.Add(improperTypeFinder(new string[] { "DatePipe" },
	new string[] { "Date", "string", "number" }));

result.Add(improperTypeFinder(new string[] { "I18nPluralPipe" },
	new string[] { "number" }));

result.Add(improperTypeFinder(new string[] { "SlicePipe" },
	new string[] { "string", "Array" }));