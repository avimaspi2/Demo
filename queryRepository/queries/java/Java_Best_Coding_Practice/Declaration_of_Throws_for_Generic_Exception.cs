// Searches for throws of generic exceptions by considering the CxThrows custom attributes as initial searching points.
// Generic exceptions in java are considered the classes Exception and Throwable.

// Finds the exceptions in the next line of the method signature	
CxList CxThrows = All.FindByCustomAttribute("CxThrows");
result = CxThrows.FindByRegex(@"throws\s+Exception");
result.Add(CxThrows.FindByRegex(@"throws\s+Throwable"));