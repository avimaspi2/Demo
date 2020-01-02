CxList db = Find_DB_In() + Find_DB_Out();

db.Add(db.GetMembersOfTarget());
db.Add(db.GetMembersOfTarget());
db.Add(db.GetMembersOfTarget());
db.Add(db.GetMembersOfTarget());

db -= db.GetTargetOfMembers().GetTargetOfMembers().GetTargetOfMembers().GetTargetOfMembers();
db -= db.GetTargetOfMembers().GetTargetOfMembers().GetTargetOfMembers();
db -= db.GetTargetOfMembers().GetTargetOfMembers();
db -= db.GetTargetOfMembers();

result = db;