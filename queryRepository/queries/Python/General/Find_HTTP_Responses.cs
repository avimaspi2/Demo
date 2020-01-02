result = All.FindByMemberAccess("flask.make_response").GetAssignee();
result.Add(All.FindByMemberAccess("flask.Response").GetAssignee());
result.Add(All.FindByMemberAccess("flask.make_default_options_response").GetAssignee());
result = All.FindAllReferences(result);