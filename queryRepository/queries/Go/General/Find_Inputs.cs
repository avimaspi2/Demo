// From user interaction
result.Add(Find_Interactive_Inputs());

// From console, like stdin, args or environment
result.Add(Find_Console_Inputs());

// From remote requests
result.Add(Find_Remote_Requests());

// From readers (ie files)
result.Add(Find_Read());

// From web requests
result.Add(Find_Web_Inputs());