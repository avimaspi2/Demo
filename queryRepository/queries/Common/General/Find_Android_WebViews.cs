// Finds all the references for WebViews in Android projects

CxList members = Find_MemberAccesses();
CxList unknownRef = Find_UnknownReference();
CxList urAndMembers = unknownRef + members;
result = unknownRef.FindByType("WebView");
result.Add(urAndMembers.FindByType(Find_ClassDecl().InheritsFrom("WebView")));