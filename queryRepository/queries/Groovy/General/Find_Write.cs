result = 
	All.FindByMemberAccess("BufferedOutputStream.write*") +
	All.FindByMemberAccess("BufferedWriter.write*") +
	All.FindByMemberAccess("ByteArrayOutputStream.write*") +
	All.FindByMemberAccess("DataOutputStream.write*") +
	All.FindByMemberAccess("FileOutputStream.write*") +
	All.FindByMemberAccess("FileWriter.write*") +
	All.FindByMemberAccess("FilterOutputStream.write*") +
	All.FindByMemberAccess("ObjectOutputStream.write*") +
	All.FindByMemberAccess("OutputStream.print*") +  
	All.FindByMemberAccess("OutputStream.write*") +
	All.FindByMemberAccess("OutputStreamWriter.write*") +
	All.FindByMemberAccess("PipedOutputStream.write*") +
	All.FindByMemberAccess("PrintStream.write*") +
	All.FindByMemberAccess("PrintWriter.write*") +
	All.FindByMemberAccess("CharArrayWriter.write*") +
	All.FindByMemberAccess("FilterWriter.write*") +
	All.FindByMemberAccess("PipedWriter.write*") +
	All.FindByMemberAccess("StringWriter.write*") +
	All.FindByMemberAccess("Writer.print*") +
	//Groovy helper methods for OutputStream:
	All.FindByMemberAccess("OutputStream.withWriter*") + 
	All.FindByMemberAccess("OutputStream.withPrintWriter*") + 
	All.FindByMemberAccess("OutputStream.withStream*") +
	All.FindByMemberAccess("OutputStream.withObjectOutputStream*");

result -= Find_Dead_Code_Contents();