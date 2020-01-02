CxList methods = Find_Methods();
CxList atomically = methods.FindByShortName("writeToFile:atomically:*");
atomically.Add(methods.FindByShortName("writeToURL:atomically:*"));

CxList atomicallyParams = All.GetParameters(atomically, 1);

CxList options = methods.FindByShortName("writeToFile:options:*");
options.Add(methods.FindByShortName("writeToURL:options:*"));
options.Add(methods.FindByShortName("write:options:*"));

CxList optionsParams = All.GetParameters(options, 1);

CxList atomicallyNumber = atomicallyParams.FindByType(typeof(IntegerLiteral));

CxList atomicallyYes = atomicallyNumber - atomicallyNumber.FindByShortName("0");
atomicallyYes.Add(atomicallyParams.FindByShortName("true", false));
CxList atomic = All.FindByShortNames(new List<string>{"dataWritingAtomic","atomic", "NSDataWritingAtomic", "NSAtomicWrite"});
atomicallyYes.Add(optionsParams * atomic.GetAncOfType(typeof(ArrayInitializer)));
atomicallyYes.Add(optionsParams * atomic);

CxList atomicalObjects = All.FindByParameters(atomicallyYes).GetTargetOfMembers();

CxList personalInfo = Find_Personal_Info();

CxList tempResult = personalInfo.DataInfluencingOn(atomicalObjects);

// fix reduce flow...
//result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);

foreach (CxList t in tempResult.GetCxListByPath())
{
	CxList method = t.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly).GetMembersOfTarget();
	CxList atom = All.GetParameters(method, 1);
	atom = atom.FindByType(typeof(Param));
	result.Add(t.ConcatenatePath(atom));
}