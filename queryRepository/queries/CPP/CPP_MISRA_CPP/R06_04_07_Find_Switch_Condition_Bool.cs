/*
 MISRA CPP RULE 6-4-7
 ------------------------------
 This query searches for bool types in switch conditions.
 
 The Example below shows code with vulnerability: 

		switch (x == 0) //Non-compliant
		{
			//...
		}

*/

CxList conditions = All.FindByFathers(All.FindByType(typeof(SwitchStmt)));
conditions -= conditions.FindByType(typeof(Case));
CxList boolConds = All.NewCxList();

//Remove +-*/%|&>><< from binary expressions, change into their sons.
CxList binarys = conditions.FindByType(typeof(BinaryExpr));
conditions -= binarys;
boolConds.Add(binarys);
binarys = binarys.FindByName("") + binarys.FindByName("|");
boolConds -= binarys;
binarys = All.GetByAncs(binarys);
binarys -= binarys.FindByType(typeof(BinaryExpr)) + binarys.FindByType(typeof(CastExpr));
conditions.Add(binarys);

//Add ! and ~ operations to boolCond, change other unarys into their sons.
CxList unarys = conditions.FindByType(typeof(UnaryExpr)) + conditions.FindByType(typeof(PostfixExpr));
conditions -= unarys;
boolConds.Add(unarys.FindByShortName("Not") + unarys.FindByShortName("OnesComplement"));
unarys -= unarys.FindByShortName("Not") + unarys.FindByShortName("OnesComplement");
conditions.Add(All.FindByFathers(unarys));

CxList refs = conditions.FindByType(typeof(UnknownReference));
foreach (CxList curr in refs) {
	TypeRef typeRef = ( (UnknownReference) curr.data.GetByIndex(0)).DeclaratorType;
	if(typeRef == null) {
		continue;
	}
	if(typeRef.TypeName.Equals("bool")) {
		boolConds.Add(curr);
	}
}

refs = conditions.FindByType(typeof(MethodInvokeExpr));
CxList defs = All.FindByType(typeof(MethodDecl));
foreach (CxList curr in refs) {
	CxList def = defs.FindDefinition(curr);
	if( def.Count == 0 ) {
		continue;
	}
	string typeName = ( (MethodDecl) def.data.GetByIndex(0)).ReturnType.TypeName;
	if(typeName.Equals("bool")) {
		boolConds.Add(curr);
	}
}

result = boolConds;