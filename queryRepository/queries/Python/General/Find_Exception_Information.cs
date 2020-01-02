/// <summary>
/// This method finds all python methods and attributes returning exception information by default
/// Exceptions information is called using "sys" attributes or "traceback" methods.
/// </summary>
string[] tb_methods_names = new string[]{"print_stack", "extract_stack", "format_stack", "walk_stack"};
string[] tb_exc_methods_names = new string[]{"format_exc", "print_exc", "print_last", };  
string[] sys_exc_methods_names = new string[]{"_current_frames", "exc_info", "_getframe"};
string[] sys_exc_attrs_names = new string[]{"__excepthook__", "exc_type", "exc_value", "exc_traceback", "last_type", "last_value", "last_traceback"};
result.Add(Find_Methods_By_Import("traceback", tb_methods_names));
result.Add(Find_Methods_By_Import("traceback", tb_exc_methods_names));
result.Add(Find_Methods_By_Import("sys", sys_exc_methods_names));
result.Add(Find_Methods_By_Import("sys", sys_exc_attrs_names));