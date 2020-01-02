CxList methods = Find_Methods();

List < String > WPsanitizeStrings = new List<String>{
	"wp_handle_upload", 
	"wp_remote_post",  
	"wp_hash",  
	"wp_cache_get",  
	"wp_nonce_url",  
	"wp_kses", 
	"is_email", 
	"sanitize_email",  
	"sanitize_file_name", 
	"sanitize_html_class", 
	"sanitize_key", 
	"sanitize_mime_type", 
	"sanitize_option", 
	"sanitize_sql_orderby", 
	"sanitize_text_field", 
	"sanitize_title_for_query", 
	"sanitize_user", 
	"sanitize_meta", 
	"sanitize_term", 
	"sanitize_term_field", 
	"sanitize_title", 
	"esc_html", 
	"esc_html__", 
	"esc_html_e", 
	"esc_attr", 
	"esc_textarea", 
	"esc_js", 
	"esc_url", 
	"esc_sql",  
	"get_term_by",  
	"get_post", 
	"get_error_message", 
	"wp_kses_post",  
	"w3_url_format", 
	"selected"};

result = methods.FindByShortNames(WPsanitizeStrings) + 
//	methods.FindByShortName("wp_handle_upload") +
//	methods.FindByShortName("wp_remote_post") + 
	
//	methods.FindByShortName("wp_hash") + 
//	methods.FindByShortName("wp_cache_get") + 
//	methods.FindByShortName("wp_nonce_url") + 
//	methods.FindByShortName("wp_kses") +
//	methods.FindByShortName("is_email") +
//	methods.FindByShortName("sanitize_email") + 
//	methods.FindByShortName("sanitize_file_name") +
//	methods.FindByShortName("sanitize_html_class") +
//	methods.FindByShortName("sanitize_key") +
//	methods.FindByShortName("sanitize_mime_type") +
//	methods.FindByShortName("sanitize_option") +
//	methods.FindByShortName("sanitize_sql_orderby") +
//	methods.FindByShortName("sanitize_text_field") +
//	methods.FindByShortName("sanitize_title_for_query") +
//	methods.FindByShortName("sanitize_user") +
//	methods.FindByShortName("sanitize_meta") +
//	methods.FindByShortName("sanitize_term") +
//	methods.FindByShortName("sanitize_term_field") +
//	methods.FindByShortName("sanitize_title") +
	
//	methods.FindByShortName("esc_html") +
//	methods.FindByShortName("esc_html__") +
//	methods.FindByShortName("esc_html_e") +
//	methods.FindByShortName("esc_attr") +
//	methods.FindByShortName("esc_textarea") +
//	methods.FindByShortName("esc_js") +
//	methods.FindByShortName("esc_url") +
//	methods.FindByShortName("esc_sql") + 
	
//	methods.FindByShortName("get_term_by") + 
//	methods.FindByShortName("get_post") +
//	methods.FindByShortName("get_error_message") +
//	methods.FindByShortName("wp_kses_post") + 
//	methods.FindByShortName("w3_url_format") +
//	methods.FindByShortName("selected") +

	methods.FindByShortName("apply_filters", false)
		
	;