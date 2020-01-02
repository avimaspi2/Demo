//Map non resolved members
CxList XSALL = XS_Find_All();
CxList decl = XSALL.FindByType(typeof(Declarator));
CxList onlyRelevantDecls = decl.GetByAncs(decl.GetAncOfType(typeof(VariableDeclStmt)));
CxList ur = XSALL.FindByType(typeof(UnknownReference));
CxList onlyLeftAssignedRefs = ur.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList Pdecl = XSALL.FindByType(typeof(ParamDecl));

CxList allSources = All.NewCxList();
allSources.Add(onlyRelevantDecls);
allSources.Add(onlyLeftAssignedRefs);
allSources.Add(Pdecl);

CxList foundToMap = XSALL.FindAllReferences(allSources);
CxList onlyTargets = foundToMap.GetTargetsWithMembers();
CxList notMembers = onlyTargets - onlyTargets.GetMembersWithTargets();

CxList onlyOne = notMembers.GetRightmostMember().GetTargetOfMembers() * notMembers;
CxList moreThanOne = notMembers - onlyOne;
CxList singleMember = onlyOne.GetMembersOfTarget();
singleMember -= singleMember.FindAllReferences(XSALL.FindDefinition(singleMember));
//leave indexers of for stmt
CxList iterationStmt = XSALL.FindByType(typeof(IterationStmt));
CxList indexersToEliminate = All.NewCxList();
CxList leftAssignedAndXSDecls = onlyLeftAssignedRefs;
leftAssignedAndXSDecls.Add(decl);

foreach(CxList iter in iterationStmt){
	IterationStmt istmt = iter.TryGetCSharpGraph<IterationStmt>();
	StatementCollection sc = istmt.Init as StatementCollection;
	if(sc == null){
		continue;
	}
	foreach(Statement s in sc){
		indexersToEliminate.Add((leftAssignedAndXSDecls).GetByAncs(All.FindById(s.NodeId)));
		
	}		
}
singleMember -= singleMember.DataInfluencedBy(allSources - indexersToEliminate);

CxList rightMost = moreThanOne.GetRightmostMember();
rightMost.Add(singleMember);

foreach(CxList rm in rightMost){
	try{
		CSharpGraph grm = rm.GetFirstGraph();
		if(grm == null || grm.LinePragma == null){
			continue;		
		}
		int fileId = grm.LinePragma.GetFileId();
		int lineNo = grm.LinePragma.Line;
		CxList targetRefs = allSources.FindByFileId(fileId).FindAllReferences(rm.GetLeftmostTarget());
		int max = -1;
		int idOfMax = -1;
		foreach(CxList tRef in targetRefs){
			try{
				CSharpGraph gTRef = tRef.GetFirstGraph();
				if(gTRef == null || gTRef.LinePragma == null || gTRef.NodeId == null){
					continue;		
				}
				int refLine = gTRef.LinePragma.Line;
				if(refLine > max && refLine < lineNo ){
					max = refLine;
					idOfMax = gTRef.NodeId;
				}
			}catch(Exception e){
				cxLog.WriteDebugMessage(e);
			}
			CustomFlows.AddFlow(All.FindById(idOfMax), rm);			
		}
	}catch(Exception e){
		cxLog.WriteDebugMessage(e);
	}
}