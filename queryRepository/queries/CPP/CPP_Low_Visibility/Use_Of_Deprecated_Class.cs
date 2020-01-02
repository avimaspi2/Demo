// Do not use std::ostrstream and use std::ostringstream instead
// Do not use std::strstream and use std::sstream instead
// Do not use std::strstreambuf and use std::stringbuf or std::wstringbuf 
// Do not use std::istrstream and use std::istringstream or std::wistringstream instead.
CxList types = Find_TypeRef();
CxList objectCreations = Find_ObjectCreations();
types -= types.FindByFathers(objectCreations);
CxList deprecatedClasses = types.FindByShortName("strstream");
deprecatedClasses.Add(types.FindByShortName("ostrstream"));
deprecatedClasses.Add(types.FindByShortName("strstreambuf"));
deprecatedClasses.Add(types.FindByShortName("istrstream"));

result = deprecatedClasses;