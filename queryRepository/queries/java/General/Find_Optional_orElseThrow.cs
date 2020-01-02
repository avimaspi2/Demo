// Finds orElseThrow() method of the java.util.Optional class.
CxList references = Find_UnknownReference();
CxList optionalReferences = references.FindByType("Optional");

result = optionalReferences.GetRightmostMember().FindByShortName("orElseThrow");