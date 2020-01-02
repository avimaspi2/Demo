CxList file = All.FindByFileName("*environment.rb");

CxList str = file.FindByName("RAILS_GEM_VERSION");

CxList assignments = file.FindByAssignmentSide(CxList.AssignmentSide.Left).FindByName("RAILS_GEM_VERSION");
CxList rightSide = file.FindByAssignmentSide(CxList.AssignmentSide.Right).GetByAncs(assignments.GetFathers());

result = rightSide;