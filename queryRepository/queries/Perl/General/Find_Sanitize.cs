result = Find_General_Sanitize();
result.Add(Find_DB().GetByAncs(Find_Methods().FindByShortName("execute*")));  // prepared statements