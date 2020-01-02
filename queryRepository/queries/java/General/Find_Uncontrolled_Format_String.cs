// Find all format strings that are affected by any type of input and not sanitized by length or size
result = All.FindByMemberAccess("out.format");
result.Add(All.FindByMemberAccess("out.printf"));