List<string> inOutMemberNames = new List<string> {"query", "set", "transaction"};
CxList sequelizeInstances = Find_ObjectCreations().FindByShortName("Sequelize");
sequelizeInstances.Add(All.FindAllReferences(sequelizeInstances.GetAssignee()));
result = sequelizeInstances.GetMembersOfTarget().FindByShortNames(inOutMemberNames);