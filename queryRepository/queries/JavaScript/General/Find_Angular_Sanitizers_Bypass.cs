// This query searches for Angular methods that bypass its default sanitizers

CxList domSanitizers = All.FindByType("DomSanitizer");
CxList methods = Find_Methods(); 

List<string> angularSanitizersBypass = new List<string>() {"bypassSecurityTrustHtml", "bypassSecurityTrustScript", 
		"bypassSecurityTrustStyle", "bypassSecurityTrustUrl", 
		"bypassSecurityTrustResourceUrl"};

CxList domSanitizersMembers = domSanitizers.GetMembersOfTarget();
CxList sanitizersBypass = methods.FindByShortNames(angularSanitizersBypass) * domSanitizersMembers;
result = All.InfluencedBy(sanitizersBypass);