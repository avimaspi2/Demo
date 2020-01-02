List<string> mathBigPkgConstMembers = new List<string> {"MinExp", "MaxExp", "MaxPrec"};
result = All.FindByMemberAccess("\"math/big\".*").FindByShortNames(mathBigPkgConstMembers);