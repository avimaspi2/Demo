CxList vfPages = Find_VF_Pages();

CxList sObjectsDef = All.FindByFileName("*.object").FindByType(typeof(ClassDecl));
CxList metaDataObjects = All.NewCxList();

string[] objectNames = new string[2 * sObjectsDef.Count];
int count = 0;
foreach (CxList obj in sObjectsDef)
{
	CSharpGraph g = obj.data.GetByIndex(0) as CSharpGraph;
	objectNames[count++] = g.ShortName;
	objectNames[count++] = "*." + g.ShortName;
}
metaDataObjects.Add(All.FindByTypes(objectNames));

metaDataObjects.Add(vfPages.FindByShortName("*__sobject"));
metaDataObjects.Add(All.FindAllReferences(metaDataObjects));
metaDataObjects -= (vfPages * metaDataObjects).FindByName("$*");
metaDataObjects -= (vfPages * metaDataObjects).GetTargetOfMembers().GetMembersOfTarget();

result = metaDataObjects;