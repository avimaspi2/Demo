// Inadequate Pointer Validation 
// -----------------------------
// The functions are obsolete and cannot guarantee that a pointer is valid or referenced memory is safe to use.


CxList methods = Find_Methods();

result =  methods.FindByShortName("IsBadWritePtr") + 
		  methods.FindByShortName("IsBadCodePtr")  + 
		  methods.FindByShortName("IsBadReadPtr")  +
		  methods.FindByShortName("IsBadStringPtr");