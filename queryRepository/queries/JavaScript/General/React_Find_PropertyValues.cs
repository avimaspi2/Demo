CxList reactAll = React_Find_All();
CxList reactPropertyKeys = React_Find_PropertyKeys();

CxList reactPropertyValues = reactAll.FindByFathers(reactPropertyKeys);

result = reactPropertyValues;