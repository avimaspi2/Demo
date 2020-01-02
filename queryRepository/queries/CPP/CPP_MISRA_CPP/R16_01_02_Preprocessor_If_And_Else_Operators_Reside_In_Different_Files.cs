/*
MISRA CPP RULE 16-1-2
------------------------------
This query searches for usage of preprocessing directives relating to same object but not in same file

	The Example below shows code with vulnerability: 

#if A
#endif
#endif

int main(int argc, char *argv[])
{
...
}

*/

// find all relevant directives, by order
CxList extendedResult = All.NewCxList();
CxList res = All.FindByRegex(@"#if|#elif|#else|#endif", false, false, false, extendedResult);

CxList notInSameFileAsTheIf = All.NewCxList();

// go over each file, count ifs/endifs to know if structure is legal
string oldFileName = "";
CxList lastDirectiveChecked = All.NewCxList();
int curIfCount = 0;

foreach (CxList cur in extendedResult){
	Comment curDirective = cur.data.GetByIndex(0) as Comment;
	if (curDirective == null || curDirective.FullName == null) {
		continue;
	}
	string curFileName = curDirective.LinePragma.FileName;

	// if new file starts, process old file data for open #ifs, and initialize data for current file
	if (String.Compare(curFileName, oldFileName) != 0){
		// this is where we find open #ifs
		if (curIfCount > 0){
			notInSameFileAsTheIf.Add(lastDirectiveChecked);
		}
		// new file data
		oldFileName = curFileName;
		curIfCount = 0;
	}

	// #if - adjust current #if count
	if (curDirective.FullName.IndexOf("#if") != -1){
		curIfCount++;
	}
	else{
		// #endif - adjust current #if count
		if (curDirective.FullName.IndexOf("#endif") != -1){
			curIfCount--;

			// negative if count means we're trying to close an #if form another file
			if (curIfCount < 0){
				notInSameFileAsTheIf.Add(cur);
			}
		}
		// #elif|#else do not affect an #if being open/closed
	}
	lastDirectiveChecked = cur;
}

result = All.FindByRegexSecondOrder(@".", notInSameFileAsTheIf);