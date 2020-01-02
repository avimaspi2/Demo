CxList production = All.FindByFileName(@"*\environments\production.rb");

CxList consider = production.FindByShortName("perform_caching");

result = production.DataInfluencingOn(consider).FindByShortName("false");