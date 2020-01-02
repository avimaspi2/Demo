CxList imports = Find_Imports();
CxList integers = Find_Integers();
CxList methods = Find_Methods();

// Remove integers used as slices from sanitizers
integers -= integers.GetByAncs(methods.FindByShortName("slice"));

//HTML sanitizers
//bleach (https://pypi.python.org/pypi/bleach)
String[] bleach_methods = new string[]{"*clean","*linkify"};
CxList bleach = Find_Methods_By_Import("bleach", bleach_methods, imports);

//django html sanitizer (https://pypi.python.org/pypi/django-html_sanitizer)
String[] django_methods = new string[]{"*SanitizedCharField","*SanitizedTextField"};
CxList django = Find_Methods_By_Import("sanitizer*", django_methods, imports);

//lxml.html.clean (http://lxml.de/lxmlhtml.html#cleaning-up-html)
String[] lxml_methods = new string[]{"*clean_html"};
CxList lxml = Find_Methods_By_Import("lxml*", lxml_methods, imports);

//BeautifulSoup (http://www.crummy.com/software/BeautifulSoup/)
String[] beautifulSoup_methods = new string[]{"*BeautifulSoup"};
CxList beautifulSoup = Find_Methods_By_Import("BeautifulSoup", beautifulSoup_methods, imports);

//html5lib (https://github.com/html5lib/html5lib-python)
String[] html5lib_methods = new string[]{"*sanitizer.HTMLSanitizer"};
CxList html5lib = Find_Methods_By_Import("html5lib", html5lib_methods, imports);

//filterHtml (https://github.com/dcollien/FilterHTML)
String[] filterHtml_methods = new string[]{"*filter_html"};
CxList filterHtml = Find_Methods_By_Import("FilterHTML", filterHtml_methods, imports);

CxList htmlSanitizers = All.NewCxList();
htmlSanitizers.Add(bleach);
htmlSanitizers.Add(django);
htmlSanitizers.Add(lxml);
htmlSanitizers.Add(beautifulSoup);
htmlSanitizers.Add(html5lib);
htmlSanitizers.Add(filterHtml);

CxList unittestClasses = All.InheritsFrom("unittest.TestCase");
CxList methodsFromUnittestClasses = methods.GetByAncs(unittestClasses);
CxList unittestAsserts = methodsFromUnittestClasses.FindByShortName("assert*");
unittestAsserts.Add(All.GetByAncs(unittestAsserts)); 

result = integers;
result.Add(htmlSanitizers);
result.Add(unittestAsserts);