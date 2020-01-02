CxList inputs = Find_Interactive_Inputs();
CxList obj = All.FindByType(typeof(UnknownReference));
obj.Add(All.FindByType(typeof(Declarator)));
CxList files = obj.FindByType("FileStream", false);
files.Add(obj.FindByType("FileInfo", false));
CxList file = All.FindByName("*File.*", false);
CxList whitelist = All.FindByType(typeof(MemberAccess));
//words that contain file in it
CxList whitelistTarget = All.FindByShortNames(new List<string>{"Profile","Defilements","Defilement","Alfilerias"
		,"Filefishes","Interfiled","Interfiles","Alfileria","Filenames","Interfile","Profilers","Undefiled","Defilers","Fileable",
"Filefish","Filename","Fileting","Misfiled","Misfiles","Prefiled","Prefiles","Profiled","Profiler","Profiles","Subfiles",
"Defiled","Defiler","Defiles","Filemot","Fileted","Misfile","Prefile","Refiled","Refiles","Subfile","Defile","Filers",
		"Filets","Refile","Filed","Filer","Filet"}, false);

//Heuristical number 3 to get up to 3 members of a target 
//ex: Profile.Address.Street.Number
file -= whitelist.GetMembersWithTargets(whitelistTarget, 3);
files.Add(file);

CxList sanitize = All.FindByName("*Server.MapPath", false);
sanitize.Add(All.FindByName("*Request.MapPath", false)); 
result = files.InfluencedByAndNotSanitized(inputs, sanitize);