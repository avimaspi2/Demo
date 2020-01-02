/* This query finds PhoneGap's Contacts API usages.
   Reference: http://docs.phonegap.com/en/edge/cordova_contacts_contacts.md.html */

CxList methods = Find_Methods();

CxList navigatorContacts = All.FindByName("navigator.contacts");

CxList contactsCreate = navigatorContacts.GetMembersOfTarget().FindByShortName("create");
contactsCreate.Add(methods.FindByShortName("create").DataInfluencedBy(navigatorContacts)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

CxList contactsFind = navigatorContacts.GetMembersOfTarget().FindByShortName("find");
contactsFind.Add(methods.FindByShortName("find").DataInfluencedBy(navigatorContacts)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

CxList contactsSaveClone = methods
	.FindByShortNames(new List<string> {"save", "clone"})
	.DataInfluencedBy(contactsCreate)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

result.Add(contactsCreate);
result.Add(contactsFind);
result.Add(contactsSaveClone);