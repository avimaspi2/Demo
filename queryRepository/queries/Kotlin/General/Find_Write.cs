string[] memberAccesses = new string[] {
	"File.writeText",
	"File.appendText",
	"FileWriter.write",
	"PrintWriter.append",
	"PrintWriter.format",
	"PrintWriter.print*",
	"PrintWriter.write",
	"FileOutputStream.write",
	"FileWriter.write",
	"FileWriter.append",
	"RandomAccessFile.write*",
	"FileChannel.write"
	};

CxList methods = Find_Methods();
foreach (string s in memberAccesses)
	result.Add(methods.FindByMemberAccess(s));

string[] writerMethiods = new string[] {
	"BufferedWriter.write",
	"BufferedWriter.append",
	"DataOutputStream.write*",
	"FilterOutputStream.write"
	};

CxList streamWriters = All.NewCxList();
foreach (string wm in writerMethiods)
	result.Add(methods.FindByMemberAccess(wm));

CxList streamsWritingToFile = All.FindByType("File").DataInfluencingOn(streamWriters);
result.Add(streamsWritingToFile.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));