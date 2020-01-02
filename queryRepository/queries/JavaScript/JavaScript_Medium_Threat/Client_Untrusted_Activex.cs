CxList methods = Find_Methods();
CxList objCreates = Find_ObjectCreations();
CxList activex = objCreates.FindByShortName("ActiveXObject"); // new ActiveXObject(type, parent, ...)
activex.Add(methods.FindByMemberAccess("Silverlight.createObject"));
CxList inputs = Find_Inputs();

result = activex.DataInfluencedBy(inputs);