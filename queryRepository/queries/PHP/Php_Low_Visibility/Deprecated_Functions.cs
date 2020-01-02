CxList methods = Find_Methods();

List<string> names = new List<string> {"mcrypt_ecb", "PDF_add_annotation",
		"PDF_add_bookmark", "PDF_add_launchlink", "PDF_add_locallink", "PDF_add_note", "PDF_add_outline",
		"PDF_add_pdflink", "PDF_add_weblink", "PDF_attach_file", "PDF_begin_page", "PDF_begin_template",
		"PDF_close", "PDF_close_pdi", "PDF_findfont", "PDF_get_font", "PDF_get_fontname", "PDF_get_fontsize",
		"PDF_get_image_height", "PDF_get_image_width", "PDF_get_majorversion", "PDF_get_minorversion",
		"PDF_get_pdi_parameter", "PDF_get_pdi_value", "PDF_open_ccitt", "PDF_open_file", "PDF_open_gif",
		"PDF_open_image", "PDF_open_image_file", "PDF_open_jpeg", "PDF_open_pdi", "PDF_open_tiff",
		"PDF_place_image", "PDF_place_pdi_page", "PDF_setgray", "PDF_setgray_fill", "PDF_setgray_stroke",
		"PDF_setpolydash", "PDF_setrgbcolor", "PDF_setrgbcolor_fill", "PDF_setrgbcolor_stroke", "PDF_set_border_color",
		"PDF_set_border_dash", "PDF_set_border_style", "PDF_set_char_spacing", "PDF_set_duration",
		"PDF_set_horiz_scaling", "PDF_set_info_author", "PDF_set_info_creator", "PDF_set_info_keywords",
		"PDF_set_info_subject", "PDF_set_info_title", "PDF_set_leading", "PDF_set_text_matrix",
		"PDF_set_text_rendering", "PDF_set_text_rise", "PDF_set_word_spacing", "PDF_show_boxed",
		"px_set_tablename", "px_set_targetencoding" };
		
List<string> memberNames = new List<string> {"GearmanClient.data", "GearmanClient.do", "GearmanClient.echo",
		"GearmanClient.setClientCallback", "GearmanClient.setData", "GearmanJob.complete", "GearmanJob.data",
		"GearmanJob.exception", "GearmanJob.fail", "GearmanJob.status", "GearmanJob.warning", "GearmanTask.create",
		"GearmanTask.function", "GearmanTask.sendData", "GearmanTask.uuid", "MongoClient.dropDB",
		"MongoCursor.slaveOkay", "MongoDB.dropCollection", "MongoDB.execute", "SoapClient.__call" };

foreach (var name in names)
{
	result.Add(methods.FindByName(name));
}

foreach (var memberName in memberNames) 
{
	result.Add(methods.FindByMemberAccess(memberName));		
}