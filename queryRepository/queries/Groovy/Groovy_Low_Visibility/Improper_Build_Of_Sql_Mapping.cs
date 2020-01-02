CxList xml = All.FindByFileName("*.xml");
result = xml.FindByType(typeof(UnknownReference)).GetParameters(xml.FindByMemberAccess("*.createQuery"), 1);