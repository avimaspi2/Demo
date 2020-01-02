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
//	"Input",
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
	"Option",
//	"P",
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
// Remove references from Link.valueOf, as this does not belongs to Jakarta ECS
Html2 -= Html2.DataInfluencedBy(Find_Methods().FindByMemberAccess("Link.valueOf").GetTargetOfMembers());

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
		CSharpGraph g = h.TryGetCSharpGraph<CSharpGraph>();
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
		CSharpGraph g = h.TryGetCSharpGraph<CSharpGraph>();
		if(g != null && g.ShortName != null)
		{
			// "*" in below call is to detect full-name types, e.g. com.accor.asa.commun.metier.Address
			Html -= Html.FindByType("*" + g.ShortName);
			Html -= Html.FindByShortName(g.ShortName);
		}
	}catch(Exception e)
	{
		cxLog.WriteDebugMessage(e.ToString());
	}
}

result = Html;
result.Add(addelFathers);
result -= Find_Dead_Code_Contents();