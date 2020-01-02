CxList methods = Find_Methods();

result.Add(methods.FindByMemberAccesses(new string[]{
	// org.omg.CORBA.Any
	"Any.insert_Principal",
	"Any.extract_Principal",
	// org.omg.CORBA.ORB
	"ORB.create_basic_dyn_any",
	"ORB.create_dyn_any",
	"ORB.create_dyn_array",
	"ORB.create_dyn_enum",
	"ORB.create_dyn_sequence",
	"ORB.create_dyn_struct",
	"ORB.create_dyn_union",
	"ORB.create_recursive_sequence_tc",
	"ORB.get_current",
	// org.omg.CORBA.portable.InputStream
	"InputStream.read_Principal",
	// org.omg.CORBA.portable.OutputStream
	"OutputStream.write_Principal",
	// org.omg.CORBA.ServerRequest
	"ServerRequest.except",
	"ServerRequest.op_name",
	"ServerRequest.params",
	"ServerRequest.result"}));