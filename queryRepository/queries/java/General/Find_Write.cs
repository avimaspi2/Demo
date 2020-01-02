string[] memberAccesses = new string[] {
	"BufferedOutputStream.write*",
	"BufferedWriter.write*",
	"BufferedWriter.append*",
	"ByteArrayOutputStream.write*",
	"DataOutputStream.write*",
	"FileOutputStream.write*",
	"Files.write",
	"Files.append",
	"FileWriter.write*",
	"FilterOutputStream.write*",
	"ObjectOutputStream.write*",
	"OutputStream.print*",
	"OutputStream.write*",
	"OutputStreamWriter.write*",
	"PipedOutputStream.write*",
	"PrintStream.write*",
	"PrintStream.append*",
	"PrintWriter.write*",
	"PrintWriter.append*",
	"PrintWriter.format*", 
	"CharArrayWriter.write*",
	"CharArrayWriter.append*",
	"FilterWriter.write*",
	"PipedWriter.write*",
	"StringWriter.write*",
	"StringWriter.append*",
	"Writer.print*",
	"Writer.append*"
	};

foreach (string s in memberAccesses)
	result.Add(All.FindByMemberAccess(s));

// Addition of writes in Cloud Services
result.Add(Find_Cloud_Storage_Out());


result -= Find_Dead_Code_Contents();