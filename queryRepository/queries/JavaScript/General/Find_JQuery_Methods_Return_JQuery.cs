List < string > AmbivalentNamesMethods = new List<string>(new string[]{
	"addClass","attr","prop","removeAttr","removeClass","removeProp","toggleClass","$","removeClass",
	"toggleClass","clearQueue","dequeue","removeData","andSelf","error","die","live","load","toggle",
	"unload","animate","delay","fadeIn","fadeOut","fadeTo","fadeToggle","finish","hide","show",
	"slideDown","queue","slideToggle","slideUp","stop","bind","blur","change","click",
	"contextmenu","dblclick","delegate","focus","focusin","focusout","hover","keydown","keypress",
	"keyup","mousedown","mouseenter","mouseleave","mousemove","mouseout","mouseover","mouseup",
	"off","on","one","ready","resize","scroll","select","submit","trigger","unbind","undelegate",
	"after","append","appendTo","before","clone","detach","empty","insertAfter","insertBefore",
	"prepend","prependTo","remove","replaceAll","replaceWith","toggleClass","unwrap","wrap","wrapAll",
	"wrapInner","each","get","add","addBack","andSelf","children","closest","contents","end","eq",
	"filter","find","first","has","is","last","map","next","nextAll","nextUntil","not","offsetParent",
	"parent","parents","parentsUntil","prev","prevAll","prevUntil","siblings","slice"});

CxList methods = Find_Methods();
CxList parameters = Find_Parameters();

List < string > AmbivalentMembers = new List<string>(new string[]{
	"currentTarget","delegateTarget",
	"relatedTarget","target","context"});

CxList AmbivalentToParams = methods.FindByShortNames(AmbivalentNamesMethods);
CxList MembersThatReturnElement = Find_MemberAccesses().FindByShortNames(AmbivalentMembers);

List < string > withOneParamInvokes = new List<string>(new string[]{
	"html","val","height","width","innerHeight","innerWidth",
	"outerHeight","outerWidth","offset","scrollLeft","scrollTop"});

CxList withOneParam = methods.FindByShortNames(withOneParamInvokes);
withOneParam -= withOneParam.FindByParameters(parameters.GetParameters(withOneParam, 1));

List < string > twoParameters = new List<string>(new string[]{"css","data"});
CxList withTwoParams = methods.FindByShortNames(twoParameters);
withTwoParams = withTwoParams.FindByParameters(parameters.GetParameters(withTwoParams, 1));


result.Add(AmbivalentToParams);
result.Add(withOneParam); 
result.Add(withTwoParams);
result.Add(MembersThatReturnElement);