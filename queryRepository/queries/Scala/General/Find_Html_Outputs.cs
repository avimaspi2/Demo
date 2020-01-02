CxList classDecl = Find_Class_Decl();
CxList htmlObject = Find_Object_Create();

string [] strTypes = new string [] {

	"Abbr",
	"Acronym",
	"Address",
	"Applet",
	"Area",
	"Base",
	"BaseFont",
	"Bdo",
	"Big",
	"Blink",
	"BlockQuote",
	"Body",
	"BR",
	"Button",
	"Caption",
	"Center",
	"Cite",
	"Code",
	"Col",
	"ColGroup",
	"Comment",
	"DD",
	"Del",
	"Dfn",
	"Div",
	"DL",
	"DT",
	"Em",
	"FieldSet",
	"Font",
	"Form",
	"Frame",
	"FrameSet",
	"H1",
	"H2",
	"H3",
	"H4",
	"H5",
	"H6",
	"Head",
	"HR",
	"Html",
	"IFrame",
	"IMG",
	"Ins",
	"Kbd",
	"Label",
	"Legend",
	"LI",
	"Link",
	"Meta",
	"NOBR",
	"NoFrames",
	"NoScript",
	"ObjectElement",
	"OL",
	"OptGroup",
	"Param",
	"PRE",
	"Samp",
	"Script",
	"Select",
	"Small",
	"Span",
	"Strike",
	"Strong",
	"Style",
	"Sub",
	"Sup",
	"Table",
	"TBody",
	"TD",
	"TextArea",
	"TFoot",
	"TH",
	"THead",
	"Title",
	"TR",
	"TT",
	"UL",
	"Var",
	"StringElement"};

List<string> strList = new List<string> (strTypes);
strList.Add("P");
strList.Add("Input");

CxList Html = htmlObject.FindByShortNames(strList);

CxList Html2 = All.FindByTypes(strTypes);

CxList Html2Def = All.FindDefinition(Html2);

CxList fathers = Html.GetFathers();
CxList addelFathers = All.FindByShortName("addElement").GetByAncs(fathers) - fathers;
addelFathers = addelFathers.FindByType(typeof(MethodInvokeExpr));
addelFathers -= addelFathers.DataInfluencedBy(addelFathers);

Html.Add(Html2Def);

CxList removeHtml = All.NewCxList();
foreach (CxList h in Html)
{
	try{
		CSharpGraph g = h.data.GetByIndex(0) as CSharpGraph;
		if(g != null && g.TypeName != null)
		{
			removeHtml.Add(classDecl.FindByShortName(g.TypeName));
		}
	}catch(Exception e)
	{
		cxLog.WriteDebugMessage(e.ToString());
	}
}

foreach (CxList h in removeHtml)
{
	try{
		CSharpGraph g = h.data.GetByIndex(0) as CSharpGraph;
		if(g != null && g.ShortName != null)
		{
			Html -= Html.FindByType(g.ShortName) + Html.FindByShortName(g.ShortName);
		}
	}catch(Exception e)
	{
		cxLog.WriteDebugMessage(e.ToString());
	}
}

result = Html;
result.Add(addelFathers);

//Akka HTTP outputs
CxList methods = Find_Methods();
// Methods that return an http response
result.Add(methods.FindByType("HttpResponse", false));
// Entities that are used in an HttpResponse
CxList parameter = All.FindByType(typeof(Param));
result.Add(All.GetParameters(methods.FindByMemberAccess("HttpResponse.withEntity")) - parameter);