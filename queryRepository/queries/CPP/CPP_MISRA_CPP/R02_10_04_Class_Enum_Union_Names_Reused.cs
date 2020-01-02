/*
MISRA CPP RULE 2-10-4
------------------------------
This query searches for class, enum and union names that are reused anywhere in the code

	The Example below shows code with vulnerability: 

class Base{
	
};

void f1 ( ) 
{ 
   Base Boo; 
}
void f2 ( ) 
{ 
   string Base;   // Non-compliant 
}

*/

CxList identifiers = Find_Identifiers();

// find tag and enum names
CxList classes = identifiers.FindByType(typeof(ClassDecl));
CxList enums = identifiers.FindByType(typeof(EnumDecl)) + classes.FindByRegex(@"enum\s");
classes -= enums;

//remove class methods implementations - classDecls from the type: "Classname::methodname()"
CxList memberTags = All.FindByRegex(@"::").GetAncOfType(typeof(ClassDecl));
CxList relevantClassDecls = classes.FindByRegex(@"class\s"); 
CxList irrelevantTags = classes - relevantClassDecls;
CxList ctorsAndDtors = All.FindByType(typeof(DestructorDecl)) + All.FindByType(typeof(ConstructorDecl));
CxList badIdentifiers = irrelevantTags + ctorsAndDtors + memberTags; 
identifiers -= badIdentifiers;
classes -= memberTags;


//search union names
CxList extendedResult = All.NewCxList();
SortedList identNames = new SortedList(identifiers.Count);
ArrayList unionNames = new ArrayList();
CxList unions = All.FindByRegex(@"union[^{]*{[^}]*[^}]*}", false, false, false, extendedResult);
foreach (CxList cur in extendedResult){
	Comment curDirective = cur.data.GetByIndex(0) as Comment;
	String unionContent = curDirective.FullName;
	if (curDirective == null || unionContent == null) {
		continue;
	}
	//get the union content
	unionContent = unionContent.Remove(0, "enum".Length + 1).Trim();
	string unionName = unionContent.Remove(unionContent.IndexOf('{')).Trim();
	if (!identNames.Contains(unionName)){
		identNames.Add(unionName, null);		
	}
	if (!unionNames.Contains(unionName)){
		unionNames.Add(unionName);		
	}
}

// search for duplicates 
CxList duplicateNames = All.NewCxList();
CxList duplicateNamesOfUnions = All.NewCxList();

foreach(CxList curIdentifier in identifiers)
{
	string curName = (curIdentifier.data.GetByIndex(0) as CSharpGraph).ShortName;
	if (!identNames.Contains(curName)){
		identNames.Add(curName, null);
		
	}
	else{
		duplicateNames.Add(curIdentifier);
	}
	
	if (unionNames.Contains(curName)){
		duplicateNamesOfUnions.Add(curIdentifier);
	}
}

//get the duplicates
CxList duplicateTags = classes.FindByShortName(duplicateNames) + enums.FindByShortName(duplicateNames);
//only the identifiers duplicated with union names will be added to the list, the unions will not be added
CxList duplicateUnions = identifiers.FindByShortName(duplicateNamesOfUnions) - duplicateTags;
result = identifiers.FindByShortName(duplicateTags) + duplicateUnions;