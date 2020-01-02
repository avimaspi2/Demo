CxList methods = Find_Methods();

result.Add(methods.FindByMemberAccess("Clipboard.setString"));
result.Add(methods.FindByMemberAccess("Linking.openURL"));
result.Add(methods.FindByMemberAccess("AsyncStorage.setItem"));
result.Add(methods.FindByMemberAccess("AsyncStorage.mergeItem"));
result.Add(methods.FindByMemberAccess("AsyncStorage.multiSet"));
result.Add(methods.FindByMemberAccess("AsyncStorage.multiMerge"));