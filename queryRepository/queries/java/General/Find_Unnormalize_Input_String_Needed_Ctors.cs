if(param.Length == 1)
{
	CxList allConstructors = param[0] as CxList;

	result = allConstructors.FindByShortNames(new List<string> {
			"File",
			"FileReader",
			"FileOutputStream",
			"FilterOutputStream",
			"ByteArrayOutputStream",
	
			"FileInputStream",
			"ByteArrayInputStream",
			"FilterInputStream",
			"StringBufferInputStream",
			"SequenceInputStream"});
}