CxList leftIndexer = All.FindByType(typeof(IndexerRef)).FindByAssignmentSide(CxList.AssignmentSide.Left);
result = All.FindByFathers(leftIndexer).FindByShortName("Session", false);