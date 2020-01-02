CxList methods = All.NewCxList();
methods.Add(All.FindByMemberAccess("AudioInputStream.read*"));
methods.Add(All.FindByMemberAccess("BufferedInputStream.read*"));
methods.Add(All.FindByMemberAccess("BufferedReader.read*"));
methods.Add(All.FindByMemberAccess("ByteArrayInputStream.read*"));
methods.Add(All.FindByMemberAccess("CharArrayReader.read*"));
methods.Add(All.FindByMemberAccess("DataInputStream.read*"));
methods.Add(All.FindByMemberAccess("Files.readAllBytes"));
methods.Add(All.FindByMemberAccess("Files.readAllLines"));
methods.Add(All.FindByMemberAccess("FileInputStream.read*"));
methods.Add(All.FindByMemberAccess("FileReader.read*"));
methods.Add(All.FindByMemberAccess("FilterInputStream.read*"));
methods.Add(All.FindByMemberAccess("InputStream.read*"));
methods.Add(All.FindByMemberAccess("InputStreamReader.read*"));
methods.Add(All.FindByMemberAccess("LineNumberReader.read*"));
methods.Add(All.FindByMemberAccess("ObjectInputStream.read*"));
methods.Add(All.FindByMemberAccess("PipedInputStream.read*"));
methods.Add(All.FindByMemberAccess("PipedReader.read*"));
methods.Add(All.FindByMemberAccess("SequenceInputStream.read*"));
methods.Add(All.FindByMemberAccess("ServletInputStream.read*"));
methods.Add(All.FindByMemberAccess("StringBufferInputStream.read*"));
methods.Add(All.FindByMemberAccess("StringReader.read*"));
//methods.Add(All.FindByMemberAccess("Call.invoke")); // org.apache.axis.client.Call

////////////////////////////-
//deal differently with Call.invoke (from org.apache.axis.client.Call)
//since the first argument may be an array of objects without relevance to reading purposes.
CxList call_invoke = All.FindByMemberAccess("Call.invoke");
CxList call_invoke_params = All.GetParameters(call_invoke, 0);

CxList find_array_params = call_invoke_params.FindByType(typeof(ArrayCreateExpr));
find_array_params.Add(call_invoke_params.InfluencedBy(base.Find_ArrayCreateExpr()));

call_invoke_params -= find_array_params;
////////////////////////////-

CxList parameters = All.GetParameters(methods, 0);
parameters.Add(call_invoke_params);
CxList findIntegers = Find_Integers();
parameters -= findIntegers;
parameters -= findIntegers.GetFathers();
parameters -= parameters.FindByType(typeof(Param));

// remove the methods that have parameters, because we are using the parameters
// (if there was no parameter for the method, then it stays)
//methods -= methods.FindByParameters(parameters); 

methods.Add(call_invoke); 
methods.Add(// add methods that their parameters are not inputs
	All.FindByMemberAccess("System.getProperty"));
methods.Add(All.FindByMemberAccess("System.getProperties"));
methods.Add(All.FindByMemberAccess("SAXReader.read*"));

if(!All.isWebApplication)
{
	methods.Add(All.FindByName("System.getenv"));
	methods.Add(All.FindByMemberAccess("Properties.getProperty"));
}

result = methods;
result.Add(parameters);

// Addition of reads from Cloud services
result.Add(Find_Cloud_Storage_In());


result -= Find_Plain_Interactive_Inputs();