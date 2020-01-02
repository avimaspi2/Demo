CxList pswInStr = Find_Password_Strings();
CxList psw = Find_Passwords() - pswInStr;
psw -= psw.FindByShortName(@"""*"); // Remove Param objects that are of strings

CxList psw_in_lSide = psw.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList psw_in_lSide_decl = psw_in_lSide.FindByType(typeof(Declarator));

CxList allUnknownRef = Find_UnknownReference();
CxList allParams = Find_Param();
CxList arrayIndexes = Find_IndexerRefs();

CxList strLiterals = Find_Strings() - Find_Empty_Strings();

//when the hardcoded string includes a space or dot we believe 
//it is not a password string

CxList stringToRenmove = All.NewCxList();
stringToRenmove.Add(strLiterals.FindByName("* *"));
stringToRenmove.Add(strLiterals.FindByName("*.*"));
stringToRenmove.Add(strLiterals.FindByName("*/*"));
stringToRenmove.Add(strLiterals.FindByName("*\\*"));

strLiterals -= stringToRenmove;

// remove also strings that are used as indexers of an array
strLiterals -= strLiterals.GetByAncs(arrayIndexes);

CxList lit_in_rSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);

// Password in declaration
CxList stringsOfPass = strLiterals.GetByAncs(psw);
CxList PasswordInDecl = psw_in_lSide_decl.FindByInitialization(lit_in_rSide);	
PasswordInDecl.Add(stringsOfPass.GetAncOfType(typeof(Declarator)));	
PasswordInDecl.Add(stringsOfPass.GetAncOfType(typeof(UnknownReference)));

// remove strings that are indexes in dictionaries
CxList forKey = All.FindByParameters(stringsOfPass.GetByAncs(PasswordInDecl)).FindByShortName("*ForKey:");
PasswordInDecl -= stringsOfPass.GetByAncs(forKey).GetAncOfType(typeof(Declarator));

CxList methods = Find_Methods();

List<string> strcmpMethods = new List<string>{"strcmp","strncmp","bcmp"};
CxList strcmp = methods.FindByShortNames(strcmpMethods);

CxList objCEqual = methods.FindByShortNames(new List<string>{"isEqualToString:","isEqual:"});

CxList allParamsStrLiterals = All.NewCxList();
allParamsStrLiterals.Add(allParams);
allParamsStrLiterals.Add(strLiterals);

CxList strcmpParam1 = allParamsStrLiterals.GetParameters(strcmp, 0);
//For -2- parameter of "isEqualToString:" (the unknown reference)
//get fathers of "isEqualToString:" then find ancs of type unknown reference
CxList strcmpParam2 = All.GetParameters(strcmp, 1);

CxList allParamsStr = All.NewCxList();
allParamsStr.Add(allParams);
allParamsStr.Add(strLiterals);

CxList objCParam1 = allParamsStr.GetParameters(objCEqual, 0);
CxList objCParam2 = allUnknownRef.GetByAncs(objCEqual.GetFathers()) * psw;	//-2-

//results of type [@"hello" isEqualToString:password]
CxList allIfs = Find_Ifs();
CxList inIsEq = strLiterals.GetFathers() * allIfs;
CxList isEq = objCEqual.GetByAncs(inIsEq);
CxList passInIsEq = psw.GetParameters(isEq, 0);
passInIsEq = objCEqual.FindByParameters(passInIsEq);

//strcmp(password, "myPass")
//strcnmp(password, "myPass", length)
//bcmp(password, "myPass", cnt)
CxList hPassInStrcmp = All.FindByParameters(strcmpParam1 * psw).FindByParameters(strcmpParam2 * strLiterals);

//strcmp("myPass", password)
//strcnmp("myPass", password, length)
//bcmp("myPass", password, cnt)


CxList strcmpParam2Psw = All.NewCxList();
strcmpParam2Psw.Add(strcmpParam2);
strcmpParam2Psw.Add(psw);

CxList parametersFathers = All.FindByParameters(strcmpParam2Psw);
parametersFathers.Add(objCEqual.GetByAncs((objCParam2 * psw).GetFathers()));

CxList strcmpParam1ObjCParam = All.NewCxList();
strcmpParam1ObjCParam.Add(strcmpParam1);
strcmpParam1ObjCParam.Add(objCParam1);

CxList parametersLiterals = All.FindByParameters(strcmpParam1ObjCParam * strLiterals);

hPassInStrcmp.Add(parametersFathers * parametersLiterals);

// Find password in an "Equals" operation
CxList eq = All.FindByMemberAccess("String.Equals");
CxList equalsPassword = strLiterals.GetByAncs(eq * psw.GetMembersOfTarget());

eq *= strLiterals.GetMembersOfTarget();
equalsPassword.Add(psw.GetByAncs(eq));

// Password in simple assignment
CxList assignPassword = psw_in_lSide.GetAncOfType(typeof(AssignExpr));
assignPassword = lit_in_rSide.GetByAncs(assignPassword);


//Password in Dictionary/array initialization
CxList passInDictionary = All.NewCxList();
CxList arrays = Find_ArrayInitializer();
CxList pswInArrayInit = pswInStr.FindByFathers(arrays);
CxList arrayInitFatherOfPsw = pswInStr.GetFathers().FindByType(typeof(ArrayInitializer));
//Look for dictionaries that have a literal password as Key
//If the value of that key is a string literal, we are facing a case of hardcoded password 
//Example: let x = [ "Password" : "test" ]
foreach(CxList member in arrayInitFatherOfPsw) {
	try{
		ArrayInitializer arrayInit = member.GetFirstGraph() as ArrayInitializer;
		ExpressionCollection listOfValues = arrayInit.InitialValues;
		
		for(int i = 0; i < listOfValues.Count; i += 2){
			Expression key = listOfValues[i];
			if(i + 1 < listOfValues.Count && pswInArrayInit.FindById(key.NodeId).Count > 0){
				Expression expr = listOfValues[i + 1];
				if(strLiterals.FindById(expr.NodeId).Count > 0){
					passInDictionary.Add(pswInArrayInit.FindById(key.NodeId));
				}
			}
		}
	}catch(Exception exc){cxLog.WriteDebugMessage(exc);}
}

//Password in dictionary/array access
//Example: x["password"] = "test"
//         y["password"]["user"] = "pswd"
CxList strLiteralsInAssign = strLiterals.GetAncOfType(typeof(AssignExpr));
CxList arraysAssignedToLiterals = arrayIndexes.FindByFathers(strLiteralsInAssign);
passInDictionary.Add(pswInStr.GetByAncs(arraysAssignedToLiterals));


result = PasswordInDecl;
result.Add(hPassInStrcmp);
result.Add(equalsPassword); 
result.Add(assignPassword); 
result.Add(passInIsEq);
result.Add(passInDictionary); 
result -= Find_Interactive_Inputs();