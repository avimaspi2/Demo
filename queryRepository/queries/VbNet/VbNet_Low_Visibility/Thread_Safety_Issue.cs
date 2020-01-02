CxList logs = All.FindByName("*log*", false);
logs.Add(All.FindByType("logger", false));

CxList no_logs = All - logs;

CxList statics = no_logs.FindAllReferences(no_logs.FindByFieldAttributes(Modifiers.Static) - 
										no_logs.FindByFieldAttributes(Modifiers.Readonly));

statics = statics - no_logs.FindByType(typeof(MethodInvokeExpr));

CxList EventArgs =  All.FindByType("*CommandEventArgs", false);

result = (EventArgs + Find_Interactive_Inputs()).DataInfluencingOn(statics);