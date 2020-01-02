// Query - Insecure Data Storage
// -----------------------------
// This risk addresses the obvious concern of sensitive data being stored on mobile devices
// Code example:
// 			String outFilename = "/sdcard/file.txt";
// How to Resolve:
// 			String outFilename = "/data/data/file.txt";
//

// The purpose of the query is to detect any attempt to write information to external storage
// The encrypted information which is stored on externel storage will be detected as well

CxList strings = Find_Strings();

CxList sd = strings.FindByName(@"*/sdcard/*", false);
sd.Add(All.FindByMemberAccess("Environment.getExternalStorageDirectory"));
sd.Add(All.FindByMemberAccess("Environment.getExternalStoragePublicDirectory"));
sd.Add(All.FindByMemberAccess("Context.getExternalCacheDir"));
sd.Add(All.FindByMemberAccess("Context.getExternalFilesDir"));

CxList allWrites = Find_Write();
allWrites.Add(Find_FileSystem_Write());

result = allWrites.DataInfluencedBy(sd);