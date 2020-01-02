/*
MISRA C RULE 5-4
------------------------------
This query searches for tag names that are reused anywhere in the code

	The Example below shows code with vulnerability: 

struct rgb
{
    signed     int   r:2;
    unsigned short   g:3;
    unsigned   int   b:3;
};

int rgb;

*/

CxList identifiers = Find_Identifiers();

// find tag names
CxList tags = identifiers.FindByType(typeof(ClassDecl));


// search for duplicates 
SortedList identNames = new SortedList(identifiers.Count);
CxList duplicateNames = All.NewCxList();
foreach(CxList curIdentifier in identifiers)
{
	string curName = (curIdentifier.data.GetByIndex(0) as CSharpGraph).ShortName;
	if (!identNames.Contains(curName)){
		identNames.Add(curName, null);
	}
	else{
		duplicateNames.Add(curIdentifier);
	}
}

result = identifiers.FindByShortName(tags.FindByShortName(duplicateNames));