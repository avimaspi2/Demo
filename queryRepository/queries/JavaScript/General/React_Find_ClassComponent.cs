CxList allClasses = Find_Classes();
CxList allInheritingReactComponent = allClasses.InheritsFrom("React.Component");

result = allInheritingReactComponent;