// Only returns permit methods inside Ruby on Rails Controllers
// supports: params.permit(<:prop>)
//           params.require(:symbol).permit(<:prop>)
//           symb=params.require(:symbol);symb.permit(<:prop>)
//           somePrivateMethodThatReturnsParameters().permit(<:prop>)
//           and similar permit! variations

CxList methods = Find_Methods();
CxList controllers = Find_Controllers();
CxList unknowns = All.FindByType(typeof(UnknownReference));

CxList parameters = unknowns.FindByShortName("params");
parameters = parameters.GetByAncs(controllers);

CxList permits = methods.FindByShortName("permit");
permits.Add(methods.FindByShortName("permit!"));

// InfluencedBy required when variables/methods are used
result = permits.DataInfluencedBy(parameters).GetPathsOrigins();