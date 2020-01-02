// Use of eval and node-serialize, serialize-to-js and notevil libraries should be considered unsafe for deserialization

CxList nodeSerialize = Find_Require("node-serialize");
result.Add(nodeSerialize.GetMembersOfTarget().FindByShortName("unserialize*"));

CxList serializeToJs = Find_Require("serialize-to-js");
result.Add(serializeToJs.GetMembersOfTarget().FindByShortName("deserialize*"));

CxList methods = Find_Methods();
result.Add(methods.FindByShortName("eval"));

CxList notevil = Find_Require("notevil");
result.Add(notevil.FindByType(typeof(MethodInvokeExpr)));