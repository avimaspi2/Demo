/*
 MISRA CPP RULE 7-1-2
 ------------------------------
 This query searches for pointer or reference to object that is not modified,
 but doesn't have the const keyword.
 
 The Example below shows code with vulnerability: 

	      void myfunc(int * param1,			//Compliant
						const int * param2,	//Compliant
						int * param3,		//Non-compliant
						int * const param4)	//Non-compliant
			{
				*param1 = *param2 + *param3 + *param4;
			};

*/

//Find pointer parameters.
CxList typerefs = All.FindByType(typeof(TypeRef));
CxList pointers = typerefs.FindByRegex(@"\w+\s*\*", false, false, false);
CxList potPoint = typerefs.FindByRegex(@"\w+\s+const\s*\*",false,false,false) + 
	typerefs.FindByRegex(@"(?<=const\s+)(\w+::)?\w+\s*\*",false,false,false);
pointers = pointers - potPoint;
pointers = pointers.GetAncOfType(typeof(ParamDecl));
pointers -= pointers.FindByShortName("");

//Get prototype parameters
CxList protoMethods = All.FindByType(typeof(MethodDecl));
protoMethods -= All.FindByFathers(protoMethods).FindByType(typeof(StatementCollection)).GetFathers();
CxList protoParams = All.GetParameters(protoMethods);
pointers -= protoParams;

//Find modified pointer objects.
CxList modified = All.FindAllReferences(pointers) - pointers;
CxList unarys = modified.GetFathers().FindByType(typeof(UnaryExpr));
unarys = unarys.FindByAssignmentSide(CxList.AssignmentSide.Left);
modified = pointers.FindDefinition(All.FindByFathers(unarys));

//Add modification with << operator and usage of object's functions.
CxList doubleLeft = All.GetByBinaryOperator(BinaryOperator.ShiftLeft);
CxList sanitizeLeft = All.NewCxList();
foreach(CxList curr in doubleLeft) {
	CSharpGraph left = ((BinaryExpr) curr.data.GetByIndex(0)).Left;
	sanitizeLeft.Add(All.FindById(left.NodeId));
}
doubleLeft = sanitizeLeft.FindByType(typeof(UnknownReference));
CxList members = All.FindByMemberAccess(".*").GetTargetOfMembers();
members.Add(All.FindByFathers(members.FindByType(typeof(IndexerRef))));
doubleLeft.Add(members.FindByType(typeof(UnknownReference)));

//Find referenced parameters.
CxList refs = typerefs.FindByRegex(@"\w+\s*&", false, false,false).GetAncOfType(typeof(ParamDecl));
refs -= refs.FindByFieldAttributes(Modifiers.Readonly);
refs -= typerefs.FindByRegex(@"const\s+(\w+::)?\w+\s*&", false, false,false).GetAncOfType(typeof(ParamDecl));
refs -= protoParams;
refs -= refs.FindByShortName("");
//Find modified ref'd parameters.
CxList modRefs = All.FindAllReferences(refs) - refs;
modRefs = refs.FindDefinition(modRefs.FindByAssignmentSide(CxList.AssignmentSide.Left));
modRefs.Add((pointers + refs).FindDefinition(doubleLeft));
modified.Add(modRefs);

//Find methods that are overriden by modified.
CxList modMethods = modified.GetAncOfType(typeof(MethodDecl));
CxList relMethods = All.FindByType(typeof(MethodDecl)).FindByShortName(modMethods) - modMethods - protoMethods;
CxList allParams = All.FindByType(typeof(ParamDecl));
CxList relParams = allParams.FindByShortName(modified) - modified - protoParams;
CxList relClasses = All.GetClass(relMethods);
CxList modClasses = All.GetClass(modMethods);
typerefs = typerefs.GetByAncs(relMethods.GetParameters(relParams.GetAncOfType(typeof(MethodDecl))));
bool isOverride = true;
foreach(CxList curr in relParams) {
	CxList sons = modClasses.InheritsFrom(relClasses.GetClass(curr));
	CxList others = modified.FindByShortName(curr);
	CxList currMethod = curr.GetAncOfType(typeof(MethodDecl));
	if (currMethod.Count == 0) {
		continue;
	}
	CxList otherMethods = relMethods.FindByShortName(currMethod);
	others = others.GetByAncs(otherMethods);
	String currMethodString = ((MethodDecl) currMethod.data.GetByIndex(0)).Name;
	CxList currParams = relMethods.GetParameters(curr.GetAncOfType(typeof(MethodDecl)));
	currParams = typerefs.GetByAncs(currParams);
	foreach(CxList other in others) {
		if (curr.GetAncOfType(typeof(MethodDecl)).Count == 0) {
			continue;
		}
		String otherMethod = ((MethodDecl) other.GetAncOfType(typeof(MethodDecl)).data.GetByIndex(0)).Name;
		CxList otherParams = allParams.GetParameters(other.GetAncOfType(typeof(MethodDecl)));
		otherParams = typerefs.GetByAncs(otherParams);
		//Check if otherMethod overrides currMethod.
		if( sons.FindByName(modClasses.GetClass(other)).Count == 1 && //other's class inherits curr's class.
		otherMethod.Equals(currMethodString) && currParams.Count == otherParams.Count) {
			for(int i = 0; i < currParams.Count; i++) {
				string cName = ((ParamDecl) currParams.data.GetByIndex(i)).Name;
				string oName = ((ParamDecl) otherParams.data.GetByIndex(i)).Name;
				if(!cName.Equals(oName)) {
					isOverride = false;	
					break;
				}
			}//end for
		}//end if
		else {
			isOverride = false;
		}
		
		if(isOverride) {
			modified.Add(curr);
			break;
		}
	}//end foreach on others
}//end foreach on relParams

result = refs + pointers - modified;