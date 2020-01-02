result = All.FindByType("NSRegularExpression");
// add C & Xcode regex's methods, such as regexec, regwnexec etc.
string[] regexNames = {"regexec", "regnexec", "regwexec", "regwnexec"};
result.Add(Find_Methods().FindByShortNames(new List<string>(regexNames), false));