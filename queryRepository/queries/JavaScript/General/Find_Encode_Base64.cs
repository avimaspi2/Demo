CxList methods = Find_Methods();

// Base64 module decode
CxList encryption = methods.FindByName("*Base64.decode", false);
encryption.Add(methods.FindByName("*Base64.encode", false));

// modern browsers encode
encryption.Add(methods.FindByShortNames(new List<string>{"btoa", "atob"}));

result = encryption;