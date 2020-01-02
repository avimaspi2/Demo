CxList objectCreate = Find_Object_Create();

/*
CxList io = 	
	All.FindByType("*.AudioInputStream") +
	All.FindByType("*.BufferedInputStream") +
	All.FindByType("*.BufferedOutputStream") +
	All.FindByType("*.BufferedReader") +
	All.FindByType("*.BufferedWriter") +
	All.FindByType("*.CharArrayReader") +
	All.FindByType("*.CipherInputStream") +
	All.FindByType("*.CipherOutputStream") +
	All.FindByType("*.DataInputStream") +
	All.FindByType("*.DataOutputStream") +
	All.FindByType("*.DeflaterInputStream") +
	All.FindByType("*.DeflaterOutputStream") +
	All.FindByType("*.FileInputStream") +
	All.FindByType("*.FileOutputStream") +
	All.FindByType("*.FilterInputStream") +
	All.FindByType("*.FilterOutputStream") +
	All.FindByType("*.FileReader") +
	All.FindByType("*.FileWriter") +
	All.FindByType("*.InflaterInputStream") +
	All.FindByType("*.InflaterOutputStream") +
	All.FindByType("*.InputStreamReader") +
	All.FindByType("*.LineNumberReader") +
	All.FindByType("*.ObjectInputStream") +
	All.FindByType("*.ObjectOutputStream") +
	All.FindByType("*.OutputStreamWriter") +
	All.FindByType("*.PipedInputStream") +
	All.FindByType("*.PipedOutputStream") +
	All.FindByType("*.PipedReader") +
	All.FindByType("*.PipedWriter") +
	All.FindByType("*.ProgressMonitorInputStream") +
	All.FindByType("*.PushbackInputStream") +
	All.FindByType("*.PushbackReader") +
	All.FindByType("*.PrintStream") +
	All.FindByType("*.PrintWriter") +
	All.FindByType("*.SequenceInputStream") +
	All.FindByType("*.StringReader");

	All.FindByType("AudioInputStream") +
	All.FindByType("BufferedInputStream") +
	All.FindByType("BufferedOutputStream") +
	All.FindByType("BufferedReader") +
	All.FindByType("BufferedWriter") +
	All.FindByType("CharArrayReader") +
	All.FindByType("CipherInputStream") +
	All.FindByType("CipherOutputStream") +
	All.FindByType("DataInputStream") +
	All.FindByType("DataOutputStream") +
	All.FindByType("DeflaterInputStream") +
	All.FindByType("DeflaterOutputStream") +
	All.FindByType("FileInputStream") +
	All.FindByType("FileOutputStream") +
	All.FindByType("FilterInputStream") +
	All.FindByType("FilterOutputStream") +
	All.FindByType("FileReader") +
	All.FindByType("FileWriter") +
	All.FindByType("InflaterInputStream") +
	All.FindByType("InflaterOutputStream") +
	All.FindByType("InputStreamReader") +
	All.FindByType("LineNumberReader") +
	All.FindByType("ObjectInputStream") +
	All.FindByType("ObjectOutputStream") +
	All.FindByType("OutputStreamWriter") +
	All.FindByType("PipedInputStream") +
	All.FindByType("PipedOutputStream") +
	All.FindByType("PipedReader") +
	All.FindByType("PipedWriter") +
	All.FindByType("PrintStream") +
	All.FindByType("PrintWriter") +
	All.FindByType("ProgressMonitorInputStream") +
	All.FindByType("PushbackInputStream") +
	All.FindByType("PushbackReader") +
	All.FindByType("SequenceInputStream") +
	All.FindByType("StringReader") +

	objectCreate.FindByShortName("AudioInputStream") +
	objectCreate.FindByShortName("BufferedInputStream") +
	objectCreate.FindByShortName("BufferedOutputStream") +
	objectCreate.FindByShortName("BufferedReader") +
	objectCreate.FindByShortName("BufferedWriter") +
	objectCreate.FindByShortName("CharArrayReader") +
	objectCreate.FindByShortName("CipherInputStream") +
	objectCreate.FindByShortName("CipherOutputStream") +
	objectCreate.FindByShortName("DataInputStream") +
	objectCreate.FindByShortName("DataOutputStream") +
	objectCreate.FindByShortName("DeflaterInputStream") +
	objectCreate.FindByShortName("DeflaterOutputStream") +
	objectCreate.FindByShortName("FileInputStream") +
	objectCreate.FindByShortName("FileOutputStream") +
	objectCreate.FindByShortName("FilterInputStream") +
	objectCreate.FindByShortName("FilterOutputStream") +
	objectCreate.FindByShortName("FileReader") +
	objectCreate.FindByShortName("FileWriter") +
	objectCreate.FindByShortName("InflaterInputStream") +
	objectCreate.FindByShortName("InflaterOutputStream") +
	objectCreate.FindByShortName("InputStreamReader") +
	objectCreate.FindByShortName("LineNumberReader") +
	objectCreate.FindByShortName("ObjectInputStream") +
	objectCreate.FindByShortName("ObjectOutputStream") +
	objectCreate.FindByShortName("OutputStreamWriter") +
	objectCreate.FindByShortName("PipedInputStream") +
	objectCreate.FindByShortName("PipedOutputStream") +
	objectCreate.FindByShortName("PipedReader") +
	objectCreate.FindByShortName("PipedWriter") +
	objectCreate.FindByShortName("PrintStream") +
	objectCreate.FindByShortName("PrintWriter") +
	objectCreate.FindByShortName("ProgressMonitorInputStream") +
	objectCreate.FindByShortName("PushbackInputStream") +
	objectCreate.FindByShortName("PushbackReader") +
	objectCreate.FindByShortName("SequenceInputStream") +
	objectCreate.FindByShortName("StringReader");
*/


string[] objTypes = {
	"AudioInputStream",
	"BufferedInputStream",
	"BufferedOutputStream",
	"BufferedReader",
	"BufferedWriter",
	"CharArrayReader",
	"CipherInputStream",
	"CipherOutputStream",
	"DataInputStream",
	"DataOutputStream",
	"DeflaterInputStream",
	"DeflaterOutputStream",
	"FileInputStream",
	"FileOutputStream",
	"FilterInputStream",
	"FilterOutputStream",
	"FileReader",
	"FileWriter",
	"InflaterInputStream",
	"InflaterOutputStream",
	"InputStream",
	"InputStreamReader",
	"LineNumberReader",
	"ObjectInputStream",
	"OutputStream",
	"ObjectOutputStream",
	"OutputStreamWriter",
	"PipedInputStream",
	"PipedOutputStream",
	"PipedReader",
	"PipedWriter",
	"PrintStream",
	"PrintWriter",
	"ProgressMonitorInputStream",
	"PushbackInputStream",
	"PushbackReader",
	"SequenceInputStream",
	"StringReader"};

CxList io = All.NewCxList();

foreach (string str in objTypes)
{
	io.Add(All.FindByType("*." + str));
}

io.Add(All.FindByTypes(objTypes));

io.Add(objectCreate.FindByShortNames(new List<string>(objTypes)));

CxList socket = All.FindByType("*.DatagramSocket");
socket.Add(All.FindByType("*.ServerSocket"));
socket.Add(All.FindByType("*.Socket"));
socket.Add(All.FindByType("*.SSLSocket"));
socket.Add(All.FindByType("DatagramSocket"));
socket.Add(All.FindByType("ServerSocket"));
socket.Add(All.FindByType("Socket"));
socket.Add(All.FindByType("SSLSocket"));
socket.Add(objectCreate.FindByShortNames(new List<string> { 
		"DatagramSocket", "ServerSocket", "Socket", "SSLSocket"}));

//	objectCreate.FindByShortName("DatagramSocket") + 
//	objectCreate.FindByShortName("ServerSocket") +
//	objectCreate.FindByShortName("Socket") +
//	objectCreate.FindByShortName("SSLSocket");

CxList connection = All.FindByType("*.Connection");
connection.Add(All.FindByType("Connection"));
connection.Add(objectCreate.FindByShortName("Connection"));

result.Add(io);
result.Add(socket);
result.Add(connection);