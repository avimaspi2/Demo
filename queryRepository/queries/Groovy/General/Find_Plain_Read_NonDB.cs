CxList methods = All.FindByMemberAccess("AudioInputStream.read*");
	methods.Add( All.FindByMemberAccess("BufferedInputStream.read*")          );
	methods.Add( All.FindByMemberAccess("BufferedReader.read*")               );
	methods.Add( All.FindByMemberAccess("ByteArrayInputStream.read*")         );
	methods.Add( All.FindByMemberAccess("CharArrayReader.read*")              );
	methods.Add( All.FindByMemberAccess("DataInputStream.read*")              );
	methods.Add( All.FindByMemberAccess("FileInputStream.read*")              );
	methods.Add( All.FindByMemberAccess("FileReader.read*")                   );
	methods.Add( All.FindByMemberAccess("FilterInputStream.read*")            );
	methods.Add( All.FindByMemberAccess("InputStream.read*")                  );
	methods.Add( All.FindByMemberAccess("InputStreamReader.read*")            );
	methods.Add( All.FindByMemberAccess("LineNumberReader.read*")             );
	methods.Add( All.FindByMemberAccess("ObjectInputStream.read*")            );
	methods.Add( All.FindByMemberAccess("PipedInputStream.read*")             );
	methods.Add( All.FindByMemberAccess("PipedReader.read*")                  );
	methods.Add( All.FindByMemberAccess("SequenceInputStream.read*")          );
	methods.Add( All.FindByMemberAccess("ServletInputStream.read*")           );
	methods.Add( All.FindByMemberAccess("StringBufferInputStream.read*")      );
	methods.Add( All.FindByMemberAccess("StringReader.read*")                 );
	//Groovy helper methods for InputStream:
	methods.Add( All.FindByMemberAccess("InputStream.withReader*")            );
	methods.Add( All.FindByMemberAccess("InputStream.withStream*")            );
	methods.Add( All.FindByMemberAccess("InputStream.withObjectInputStream*") );

CxList parameters = All.GetParameters(methods, 0);
parameters -= Find_Integers();
parameters -= Find_Integers().GetFathers();
parameters -= parameters.FindByType(typeof(Param));

// remove the methods that have parameters, because we are using the parameters
// (if there was no parameter for the method, then it stays)
//methods -= methods.FindByParameters(parameters); 

// add methods that their parameters are not inputs
methods.Add(All.FindByMemberAccess("System.getProperty"));
methods.Add(All.FindByMemberAccess("System.getProperties"));
methods.Add(All.FindByMemberAccess("SAXReader.read*"));

if(!All.isWebApplication)
{
	methods.Add(All.FindByName("System.getenv"));
	methods.Add(All.FindByMemberAccess("Properties.getProperty"));
}

result = methods + parameters;

/* getAttribute */
result.Add(Add_Get_Attribute(result));
/* end getAttribute */

result -= Find_Plain_Interactive_Inputs();