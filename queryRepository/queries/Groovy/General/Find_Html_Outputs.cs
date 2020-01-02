CxList classDecl = Find_Class_Decl();
CxList htmlObject = Find_Object_Create();

CxList Html = 
	//htmlObject.FindByShortName("A") +
	htmlObject.FindByShortName("Abbr") +
	htmlObject.FindByShortName("Acronym") +
	htmlObject.FindByShortName("Address") +
	htmlObject.FindByShortName("Applet") +
	htmlObject.FindByShortName("Area") +
	//htmlObject.FindByShortName("B") +
	htmlObject.FindByShortName("Base") +
	htmlObject.FindByShortName("BaseFont") +
	htmlObject.FindByShortName("Bdo") +
	htmlObject.FindByShortName("Big") +
	htmlObject.FindByShortName("Blink") +
	htmlObject.FindByShortName("BlockQuote") +
	htmlObject.FindByShortName("Body") +
	htmlObject.FindByShortName("BR") +
	htmlObject.FindByShortName("Button") +
	htmlObject.FindByShortName("Caption") +
	htmlObject.FindByShortName("Center") +
	htmlObject.FindByShortName("Cite") +
	htmlObject.FindByShortName("Code") +
	htmlObject.FindByShortName("Col") +
	htmlObject.FindByShortName("ColGroup") +
	htmlObject.FindByShortName("Comment") +
	htmlObject.FindByShortName("DD") +
	htmlObject.FindByShortName("Del") +
	htmlObject.FindByShortName("Dfn") +
	htmlObject.FindByShortName("Div") +
	htmlObject.FindByShortName("DL") +
	htmlObject.FindByShortName("DT") +
	htmlObject.FindByShortName("Em") +
	htmlObject.FindByShortName("FieldSet") +
	htmlObject.FindByShortName("Font") +
	htmlObject.FindByShortName("Form") +
	htmlObject.FindByShortName("Frame") +
	htmlObject.FindByShortName("FrameSet") +
	htmlObject.FindByShortName("H1") +
	htmlObject.FindByShortName("H2") +
	htmlObject.FindByShortName("H3") +
	htmlObject.FindByShortName("H4") +
	htmlObject.FindByShortName("H5") +
	htmlObject.FindByShortName("H6") +
	htmlObject.FindByShortName("Head") +
	htmlObject.FindByShortName("HR") +
	htmlObject.FindByShortName("Html") +
	//htmlObject.FindByShortName("I") +
	htmlObject.FindByShortName("IFrame") +
	htmlObject.FindByShortName("IMG") +
	htmlObject.FindByShortName("Input") +
	htmlObject.FindByShortName("Ins") +
	htmlObject.FindByShortName("Kbd") +
	htmlObject.FindByShortName("Label") +
	htmlObject.FindByShortName("Legend") +
	htmlObject.FindByShortName("LI") +
	htmlObject.FindByShortName("Link") +
	htmlObject.FindByShortName("Meta") +
	htmlObject.FindByShortName("NOBR") +
	htmlObject.FindByShortName("NoFrames") +
	htmlObject.FindByShortName("NoScript") +
	htmlObject.FindByShortName("ObjectElement") +
	htmlObject.FindByShortName("OL") +
	htmlObject.FindByShortName("OptGroup") +
	htmlObject.FindByShortName("Option") +
	//htmlObject.FindByShortName("P") +
	htmlObject.FindByShortName("Param") +
	htmlObject.FindByShortName("PRE") +
	//htmlObject.FindByShortName("Q") +
	//htmlObject.FindByShortName("S") +
	htmlObject.FindByShortName("Samp") +
	htmlObject.FindByShortName("Script") +
	htmlObject.FindByShortName("Select") +
	htmlObject.FindByShortName("Small") +
	htmlObject.FindByShortName("Span") +
	htmlObject.FindByShortName("Strike") +
	htmlObject.FindByShortName("Strong") +
	htmlObject.FindByShortName("Style") +
	htmlObject.FindByShortName("Sub") +
	htmlObject.FindByShortName("Sup") +
	htmlObject.FindByShortName("Table") +
	htmlObject.FindByShortName("TBody") +
	htmlObject.FindByShortName("TD") +
	htmlObject.FindByShortName("TextArea") +
	htmlObject.FindByShortName("TFoot") +
	htmlObject.FindByShortName("TH") +
	htmlObject.FindByShortName("THead") +
	htmlObject.FindByShortName("Title") +
	htmlObject.FindByShortName("TR") +
	htmlObject.FindByShortName("TT") +
	//htmlObject.FindByShortName("U") +
	htmlObject.FindByShortName("UL") +
	htmlObject.FindByShortName("Var") +
	htmlObject.FindByShortName("StringElement");

CxList Html2 = 
	//All.FindByType("A") +
	All.FindByType("Abbr") +
	All.FindByType("Acronym") +
	All.FindByType("Address") +
	All.FindByType("Applet") +
	All.FindByType("Area") +
	//All.FindByType("B") +
	All.FindByType("Base") +
	All.FindByType("BaseFont") +
	All.FindByType("Bdo") +
	All.FindByType("Big") +
	All.FindByType("Blink") +
	All.FindByType("BlockQuote") +
	All.FindByType("Body") +
	All.FindByType("BR") +
	All.FindByType("Button") +
	All.FindByType("Caption") +
	All.FindByType("Center") +
	All.FindByType("Cite") +
	All.FindByType("Code") +
	All.FindByType("Col") +
	All.FindByType("ColGroup") +
	All.FindByType("Comment") +
	All.FindByType("DD") +
	All.FindByType("Del") +
	All.FindByType("Dfn") +
	All.FindByType("Div") +
	All.FindByType("DL") +
	All.FindByType("DT") +
	All.FindByType("Em") +
	All.FindByType("FieldSet") +
	All.FindByType("Font") +
	All.FindByType("Form") +
	All.FindByType("Frame") +
	All.FindByType("FrameSet") +
	All.FindByType("H1") +
	All.FindByType("H2") +
	All.FindByType("H3") +
	All.FindByType("H4") +
	All.FindByType("H5") +
	All.FindByType("H6") +
	All.FindByType("Head") +
	All.FindByType("HR") +
	All.FindByType("Html") +
	//All.FindByType("I") +
	All.FindByType("IFrame") +
	All.FindByType("IMG") +
	// All.FindByType("Input") +
	All.FindByType("Ins") +
	All.FindByType("Kbd") +
	All.FindByType("Label") +
	All.FindByType("Legend") +
	All.FindByType("LI") +
	All.FindByType("Link") +
	All.FindByType("Meta") +
	All.FindByType("NOBR") +
	All.FindByType("NoFrames") +
	All.FindByType("NoScript") +
	All.FindByType("ObjectElement") +
	All.FindByType("OL") +
	All.FindByType("OptGroup") +
	All.FindByType("Option") +
	//All.FindByType("P") +
	All.FindByType("Param") +
	All.FindByType("PRE") +
	//All.FindByType("Q") +
	//All.FindByType("S") +
	All.FindByType("Samp") +
	All.FindByType("Script") +
	All.FindByType("Select") +
	All.FindByType("SmAll") +
	All.FindByType("Span") +
	All.FindByType("Strike") +
	All.FindByType("Strong") +
	All.FindByType("Style") +
	All.FindByType("Sub") +
	All.FindByType("Sup") +
	All.FindByType("Table") +
	All.FindByType("TBody") +
	All.FindByType("TD") +
	All.FindByType("TextArea") +
	All.FindByType("TFoot") +
	All.FindByType("TH") +
	All.FindByType("THead") +
	All.FindByType("Title") +
	All.FindByType("TR") +
	All.FindByType("TT") +
	//All.FindByType("U") +
	All.FindByType("UL") +
	All.FindByType("Var") +
	All.FindByType("StringElement");

CxList Html2Def = All.FindDefinition(Html2);

CxList fathers = Html.GetFathers();
CxList addelFathers = All.GetByAncs(fathers).FindByShortName("addElement") - fathers;
addelFathers = addelFathers.FindByType(typeof(MethodInvokeExpr));
addelFathers -= addelFathers.DataInfluencedBy(addelFathers);

Html.Add(Html2Def);

CxList removeHtml = All.NewCxList();
foreach (CxList h in Html)
{
	CSharpGraph g = h.data.GetByIndex(0) as CSharpGraph;
	removeHtml.Add(classDecl.FindByShortName(g.TypeName));
}

foreach (CxList h in removeHtml)
{
	CSharpGraph g = h.data.GetByIndex(0) as CSharpGraph;
	Html -= Html.FindByType(g.ShortName) + Html.FindByShortName(g.ShortName);
}

result = Html + addelFathers;
result -= Find_Dead_Code_Contents();