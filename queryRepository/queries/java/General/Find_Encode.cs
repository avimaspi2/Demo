CxList encode = Find_HTML_Encode();
encode.Add(Find_URL_Encode());

CxList methods = Find_Methods();

encode.Add(methods.FindByShortName("*encode*", false));
encode -= methods.FindByShortName("*unencode*", false);

// org.keyczar.util.Base64Coder.encode not a trusted sanitizer
CxList targets = encode.GetTargetOfMembers(); 
CxList notSanitizers = targets.FindByType("Base64Coder");

encode -= notSanitizers.GetMembersOfTarget();

result = encode;