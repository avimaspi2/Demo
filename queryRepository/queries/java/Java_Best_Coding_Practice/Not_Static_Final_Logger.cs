CxList fileds = Find_Field_Decl();
fileds.Add(Find_Constants());

CxList StaticLoggers = fileds.FindByFieldAttributes(Modifiers.Static);

CxList allLoggers = fileds.FindByTypes(new string[] {"Logger", "*.Logger"});

result = allLoggers - StaticLoggers;