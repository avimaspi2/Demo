CxList methods = Find_Methods();
CxList strings = Find_Strings();
CxList outputs = Find_Outputs();

//1st part - Output of html anchor strings with the target="_blank" vulnerability

//<a...
string anchorP = @"<a\s+[^>]+";
//target="_blank"
string targetP = @"target\s*=\s*('|""|\\"")_?blank('|""|\\"")";
//rel="noopener" or rel="noopener noreferrer" (sanitizer)
string relP = @"rel\s*=\s*('|""|\\"")noopener|noopener\s+noreferrer('|""|\\"")";

CxList targetMatches = strings.FindByRegex(targetP);
//Remove FP (may occur with php embed in html)
foreach (CxList tM in targetMatches)
{
	CSharpGraph csg = tM.TryGetCSharpGraph<CSharpGraph>();
	if(!csg.FullName.Contains("blank"))
	{
		targetMatches -= tM;
	}
}

//Find MethodInvokes and AssignExprs which are Ancs of target="_blank" strings
CxList targetMatchesAncs = targetMatches.GetAncOfType(typeof(MethodInvokeExpr));
targetMatchesAncs.Add(targetMatches.GetAncOfType(typeof(AssignExpr)));

foreach (CxList anc in targetMatchesAncs)
{
	//Get all string children, in case of String concat
	CxList strs = All.GetByAncs(anc).FindByType(typeof(StringLiteral));

	//MethodInvokes and AssignExprs which are Ancs of <a... strings
	CxList anchorMatches = strs.FindByRegex(anchorP);
	CxList anchorMatchesAncs = anchorMatches.GetAncOfType(typeof(MethodInvokeExpr));
	anchorMatchesAncs.Add(anchorMatches.GetAncOfType(typeof(AssignExpr)));

	CxList possibleVuln = anchorMatchesAncs * targetMatchesAncs;
	//Are there anchors and target="_blank" which share the same father?
	if (possibleVuln.Count > 0)
	{
		//If so find Ancs of sanitizers (rel)
		CxList relMatches = strs.FindByRegex(relP);
		CxList relMatchesAncs = relMatches.GetAncOfType(typeof(MethodInvokeExpr));
		relMatchesAncs.Add(relMatches.GetAncOfType(typeof(AssignExpr)));

		CxList vuln = possibleVuln * relMatchesAncs;
		//Do (anchors and target="_blank") share father with sanitizer?
		if (vuln.Count == 0)
		{
			//If no, it's a vulnerability
			CxList v = All.GetByAncs(anc).FindByType(typeof(StringLiteral)).FindByRegex(targetP);
			result.Add(outputs.InfluencedBy(v));
		}
	}
}

//2nd part - Creation of DOMDocuments with the target="_blank" vulnerability

//DOMDocument methods
CxList createElem = methods.FindByShortNames(new List<string>{ "createElement", "DOMElement" });
CxList createAttr = methods.FindByShortName("createAttribute");
CxList appendChd = methods.FindByShortName("appendChild");

//Anchor creation
CxList anchors = All.GetParameters(createElem).FindByShortName("a");
//With href attribute
CxList hrefs = All.GetParameters(createAttr).FindByShortName("href");
//with target attribute
CxList targets = All.GetParameters(createAttr).FindByShortName("target");
//with rel atrtibute
CxList rels = All.GetParameters(createAttr).FindByShortName("rel");

//'_blank' value of target attribute
CxList _blank = strings.FindByShortName("_blank");
//'noopener' value of rel attribute
CxList noopener = strings.FindByShortName("noopener");
//'noopener noreferrer' value of rel attribute
CxList noopener_noreferrer = strings.FindByShortName("noopener noreferrer");

//Get assignee of the anchor elements
CxList aVars = All.GetByAncs(All.FindByParameters(anchors).GetTargetOfMembers().GetAncOfType(typeof(AssignExpr)))
	.FindByAssignmentSide(CxList.AssignmentSide.Left);

//for each anchor element
foreach (CxList a in aVars)
{
	CxList aVarRefs = All.FindAllReferences(a);
	//Check if it has href attribute
	CxList hasHref = aVarRefs.InfluencedBy(hrefs);
	if (hasHref.Count > 0)
	{
		//Check if it has target attribute
		CxList hasTarget = aVarRefs.InfluencedBy(targets);
		if (hasTarget.Count > 0)
		{
			//Check if it has '_blank' value
			CxList isBlank = aVarRefs.InfluencedBy(_blank);
			if (isBlank.Count > 0)
			{
				//Check if it's missing rel attribute with 'noopener' or 'noopener noreferrer' value
				CxList hasRel = aVarRefs.InfluencedBy(rels);
				CxList isNoOpener = aVarRefs.InfluencedBy(noopener);
				CxList isNoOpenerNoReferrer = aVarRefs.InfluencedBy(noopener_noreferrer);
				if (hasRel.Count == 0 || (isNoOpener.Count == 0 && isNoOpenerNoReferrer.Count == 0))
				{
					//Possible vulnerability
					result.Add(isBlank);
				}
			}
		}
	}
}