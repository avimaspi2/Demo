string[] memberAccesses = new string[] {
	"BufferedOutputStream.write*",
	"BufferedWriter.write*",
	"ByteArrayOutputStream.write*",
	"DataOutputStream.write*",
	"FileOutputStream.write*",
	"Files.write",
	"FileWriter.write*",
	"FilterOutputStream.write*",
	"ObjectOutputStream.write*",
	"OutputStream.print*",
	"OutputStream.write*",
	"OutputStreamWriter.write*",
	"PipedOutputStream.write*",
	"PrintStream.write*",
	"PrintWriter.write*",
	"CharArrayWriter.write*",
	"FilterWriter.write*",
	"PipedWriter.write*",
	"StringWriter.write*",
	"Writer.print*",
	"Output.write*"
	};

foreach (string s in memberAccesses)
	result.Add(All.FindByMemberAccess(s));