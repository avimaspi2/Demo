CxList refs = Find_UnknownReference();
CxList memberAccesses = Find_MemberAccesses();

result.Add(refs.FindByShortName("$event"));

// ElementRef.nativeElement.value
CxList nativeElem = memberAccesses.FindByMemberAccess("ElementRef.nativeElement");
CxList potentialValueFieldAccess = nativeElem.GetMembersOfTarget();
result.Add(potentialValueFieldAccess.FindByShortName("value"));

CxList formControls = Find_MemberAccesses().FindByShortName("controls");
result.Add(formControls.GetRightmostMember().FindByShortName("value"));