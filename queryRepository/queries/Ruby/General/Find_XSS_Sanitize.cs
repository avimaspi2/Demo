CxList methods = Find_Methods();
CxList viewMethods = Find_View_Methods();
CxList viewCode = Find_View_Code();
result = Find_XSS_Replace() + 
	Find_Encode() + 
	Find_Regex() + 
	All.GetByAncs(viewMethods.FindByShortName("link_to*")) +
	All.GetByAncs(viewMethods.FindByShortName("_path*")) +
	All.GetByAncs(viewMethods.FindByShortName("_tag*")) +
	All.GetByAncs(viewMethods.FindByShortName("_url*")) +
	viewCode.GetByAncs(viewMethods.FindByShortName("text_field_tag")) +
	viewCode.GetByAncs(viewMethods.FindByShortName("hidden_field_tag")) +
	viewCode.GetByAncs(viewMethods.FindByShortName("image_tag")) +
	viewCode.GetByAncs(viewMethods.FindByShortName("submit_tag")) +
	viewCode.GetByAncs(viewMethods.FindByShortName("hidden_field")) +
	viewCode.GetByAncs(viewMethods.FindByShortName("radio_button")) +
	viewCode.GetByAncs(viewMethods.FindByShortName("will_paginate")) +
	viewCode.GetByAncs(viewMethods.FindByShortName("button_to")) +
	viewCode.GetByAncs(viewMethods.FindByShortName("url_for")) +
	viewCode.GetByAncs(viewMethods.FindByShortName("mail_to")) +
	viewCode.GetByAncs(viewMethods.FindByShortName("fields_for")) +
	viewCode.GetByAncs(viewMethods.FindByShortName("label")) +
	viewCode.GetByAncs(viewMethods.FindByShortName("text_area")) +
	viewCode.GetByAncs(viewMethods.FindByShortName("text_field")) +
	viewCode.GetByAncs(viewMethods.FindByShortName("hidden_field")) +
	viewCode.GetByAncs(viewMethods.FindByShortName("check_box")) +
	viewCode.GetByAncs(viewMethods.FindByShortName("field_field")) +
	All.GetByAncs(viewMethods.FindByShortName("render"));

CxList findBy = 
	methods.FindByShortName("find_by*") +
	methods.FindByShortName("scoped_by*") +
	methods.FindByShortName("find_last_by*") +
	methods.FindByShortName("find_or_initialize_by*") +
	methods.FindByShortName("find_or_create_by*") +
	methods.FindByShortName("find_all_by*");
findBy -= findBy.FindByShortName("find_by_sql*");
result.Add(All.GetByAncs(All.GetParameters(findBy)));

viewMethods -= viewMethods.FindByShortName("raw");
viewMethods -= viewMethods.FindByShortName("responseWrite");
viewMethods -= viewMethods.FindByShortName("will_paginate");
viewMethods -= viewMethods.GetTargetOfMembers().GetMembersOfTarget();

CxList viewMethodsDef = All.FindDefinition(viewMethods);
viewMethods -= All.FindAllReferences(viewMethodsDef);

result.Add(viewMethods);
result.Add(Find_Console_Outputs());

result.Add(Find_General_Sanitize());