CxList methods = Find_Methods();

CxList readers = methods.FindByMemberAccess("AudioInputStream.read*");
readers.Add(methods.FindByMemberAccess("BufferedInputStream.read*"));
readers.Add(methods.FindByMemberAccess("BufferedReader.read*"));
readers.Add(methods.FindByMemberAccess("ByteArrayInputStream.read*"));
readers.Add(methods.FindByMemberAccess("CharArrayReader.read*"));
readers.Add(methods.FindByMemberAccess("DataInputStream.read*"));
readers.Add(methods.FindByMemberAccess("FilterInputStream.read*"));
readers.Add(methods.FindByMemberAccess("InputStream.read*"));
readers.Add(methods.FindByMemberAccess("InputStreamReader.read*"));
readers.Add(methods.FindByMemberAccess("LineNumberReader.read*"));
readers.Add(methods.FindByMemberAccess("ObjectInputStream.read*"));
readers.Add(methods.FindByMemberAccess("PipedInputStream.read*"));
readers.Add(methods.FindByMemberAccess("PipedReader.read*"));
readers.Add(methods.FindByMemberAccess("SequenceInputStream.read*"));
readers.Add(methods.FindByMemberAccess("ServletInputStream.read*"));
readers.Add(methods.FindByMemberAccess("StringBufferInputStream.read*"));
readers.Add(methods.FindByMemberAccess("StringReader.read*"));
readers.Add(methods.FindByMemberAccess("Files.readAllBytes"));
readers.Add(methods.FindByMemberAccess("Files.readAllLines"));
readers.Add(methods.FindByMemberAccess("FileInputStream.read*"));
readers.Add(methods.FindByMemberAccess("FileReader.read*"));

CxList parameters = All.GetParameters(readers, 0);
parameters -= parameters.FindByType(typeof(Param));

List <string> fileExtMethodsNames = new List<string> {
		"bufferedReader",
		"CopyRecursively",
		"copyTo",
		"forEachBlock",
		"forEachLine",
		"inputStream",
		"readBytes",
		"readLines",
		"readText",
		"reader",
		"useLines"
		};

CxList fileExtensions = methods.FindByMemberAccess("File.*").FindByShortNames(fileExtMethodsNames);

result = parameters;
result.Add(fileExtensions);
if(!All.isWebApplication)
{
	result.Add(All.FindByMemberAccess("System.getenv"));
}