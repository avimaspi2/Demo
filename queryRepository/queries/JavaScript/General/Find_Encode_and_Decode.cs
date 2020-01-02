//////////////////////////////////////////////////////-/
//  The query finds ways to encode and decode strings  /
//////////////////////////////////////////////////////-/

CxList methods = Find_Methods();
CxList strings = Find_String_Literal();
CxList objects = Find_ObjectCreations();
CxList charEncoding = strings.FindByShortNames(new List<string> {"base64", "utf8", "utf-8", "ascii", "binary",
		"hex", "ucs2", "utf16le"});
/*//-   Find Encoding methods   //-*/

//Encode_Base64
CxList weakCrypt = Find_Encode_Base64(); // Base64

//Encode by the constructor of the NodeJS Buffer
CxList newBuffer = objects.FindByShortName("Buffer");

//In order to add  only the 'methodInvoke' type and not the 'creationObject' type to the weakCrypt 
if (newBuffer.Count > 0) 
{
	weakCrypt.Add(Find_By_Parameter_Position(methods.FindByShortName("Buffer"), 1, charEncoding));
}

// Encode by constructor: Buffer.from - Buffer.from(string[, encoding])
CxList bufferFrom = methods.FindByMemberAccess("Buffer", "from");
if (bufferFrom.Count > 0)
{
	newBuffer.Add(bufferFrom);
	weakCrypt.Add(Find_By_Parameter_Position(bufferFrom, 1, charEncoding));
}

// Encode by Constructor: Buffer.alloc - Buffer.alloc(size[, fill[, encoding]])
CxList bufferAlloc = methods.FindByMemberAccess("Buffer", "alloc");
if (bufferAlloc.Count > 0)
{
	newBuffer.Add(bufferAlloc);
	weakCrypt.Add(Find_By_Parameter_Position(bufferAlloc, 2, charEncoding));
}

//Find the Encode methods whose related to the bufferObject - Write(), toString()
CxList bufferObject = All.FindAllReferences(All.DataInfluencedBy(newBuffer));

CxList toString = bufferObject.FindByType(typeof(MethodInvokeExpr));
CxList bufMembers = bufferObject.GetMembersOfTarget();
toString.Add(bufMembers);
toString = toString.FindByShortName("toString");
weakCrypt.Add(Find_By_Parameter_Position(toString, 0, charEncoding));

CxList Write = bufferObject.FindByType(typeof(MethodInvokeExpr));
Write.Add(bufMembers);
Write = Write.FindByShortName("write");
weakCrypt.Add(Find_By_Parameter_Position(Write, 1, charEncoding));
weakCrypt.Add(Find_By_Parameter_Position(Write, 2, charEncoding));

//Adding more Encode methods
weakCrypt.Add(methods.FindByShortName("escape"));
weakCrypt.Add(methods.FindByShortName("parseInt")); // Binary/Octal/Hex
weakCrypt.Add(methods.FindByShortName("encodeURI")); 
weakCrypt.Add(methods.FindByShortName("encodeURIComponent")); 
/*//-   Find Decoding methods  //-*/

weakCrypt.Add(methods.FindByShortName("unescape"));
weakCrypt.Add(methods.FindByShortName("decodeURI")); 
weakCrypt.Add(methods.FindByShortName("decodeURIComponent"));  

//StringDecoder
CxList requires = Find_Import();

CxList requireStringDecoder = All.NewCxList();
CxList requireDecoderRing = All.NewCxList();
foreach (CxList imp in requires){
	Import import = imp.TryGetCSharpGraph<Import>();
	if(import.ImportedFilename.Equals("string_decoder")){
		requireStringDecoder.Add(imp);	
	}
	else if(import.ImportedFilename.Equals("decoder-ring")){
		requireDecoderRing.Add(imp);
	}
}

requireStringDecoder.Add(All.FindByName("require.StringDecoder"));	
if (requireStringDecoder.Count > 0)
{
	CxList newStringDecoder = objects.FindByShortName("StringDecoder");
	CxList decoderWrite = methods.FindByShortName("write");
	CxList stringDecoder = newStringDecoder.DataInfluencingOn(decoderWrite).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	weakCrypt.Add(stringDecoder);
}

//DecoderRing
requireDecoderRing.Add(All.FindByName("require.decoderRing"));
if (requireDecoderRing.Count > 0)
{
	CxList newRingDecoder = objects.FindByShortName("DecoderRing");
	CxList ringMembers = methods.FindByShortNames(new List<string> {"decode", "encode"});
	CxList ringDecoder = newRingDecoder.DataInfluencingOn(ringMembers).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

	weakCrypt.Add(ringDecoder);
}

result = weakCrypt;