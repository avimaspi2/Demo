CxList methods = Find_Methods();

result.Add(methods.FindByMemberAccesses(new string[]{
	// java.io.DataInputStream
	"DataInputStream.readLine",
	// java.io.ObjectInputStream
	"ObjectInputStream.readLine",
	// java.io.ObjectOutputStream.PutField
	"PutField.write"}));

// java.io.ByteArrayOutputStream.toString with int is deprecated:
result.Add(methods.FindByMemberAccess("ByteArrayOutputStream.toString").FindByParameters(Find_Integers()));