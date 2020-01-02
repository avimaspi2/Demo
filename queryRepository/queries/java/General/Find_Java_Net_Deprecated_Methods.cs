CxList methods = Find_Methods();

result.Add(methods.FindByMemberAccesses(new string[]{
	// java.net.DatagramSocketImpl
	"DatagramSocketImpl.getTTL",
	"DatagramSocketImpl.setTTL",
	// java.net.MulticastSocket
	"MulticastSocket.getTTL",
	"MulticastSocket.send",
	"MulticastSocket.setTTL",
	// java.net.URLConnection
	"URLConnection.getDefaultRequestProperty",
	"URLConnection.setDefaultRequestProperty"}));

// java.net.URLDecoder.decode(String s) is deprecated, URLDecoder.decode(String s, String enc) is not
CxList URLDecode = methods.FindByMemberAccess("URLDecoder.decode");
// java.net.URLEncoder.encode(String s) is deprecated, URLEncoder.encode(String s, String enc) is not
CxList URLEencode = methods.FindByMemberAccess("URLEncoder.encode");

CxList URLEncodeDecode = All.NewCxList();
URLEncodeDecode.Add(URLDecode);
URLEncodeDecode.Add(URLEencode);

CxList nonDeprecated = URLEncodeDecode.FindByParameters(All.GetParameters(URLEncodeDecode, 1));
result.Add(URLEncodeDecode - nonDeprecated);

// java.net.URLStreamHandler.setURL(URL, String, String, int, String, String) is deprecated (was replaced with a 9-args equivalent)
CxList setURL = methods.FindByMemberAccess("URLStreamHandler.setURL");
CxList setURL_7th_Params = All.GetParameters(setURL, 6);
result.Add(setURL - setURL.FindByParameters(setURL_7th_Params));