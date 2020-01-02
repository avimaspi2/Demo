// CollectAll is deprecated, use collectNested instead
CxList collectAll = All.FindByName("*.collectAll").GetTargetOfMembers();

// groovy.lang.Immutable is deprecated, use groovy.transform.Immutable instead
CxList imports = All.FindByType(typeof(Import));
CxList importsLangImmutable = imports.FindByName("groovy.lang.Immutable");
CxList importsTransformImmutable = imports.FindByName("groovy.transform.*");

CxList immutableLangAnnotations = All.FindByCustomAttribute("groovy.lang.Immutable");
CxList immutableAnnotations = All.FindByCustomAttribute("Immutable");
CxList finalImmutableAnnotations = immutableLangAnnotations;

// look for imports with NamespaceAlias to look after for annotations with such NamespaceAlias
foreach(CxList item in importsLangImmutable)
{
	Import im = item.data.GetByIndex(0) as Import;
	if (im.NamespaceAlias != null)
	{
		CxList aliasAnnotations = All.FindByCustomAttribute(im.NamespaceAlias);
		immutableAnnotations.Add(aliasAnnotations);
	}
}

// exclude the ones that occur after an "import groovy.transform": those are valid
foreach(CxList ann in immutableAnnotations)
{
	// gets the line pragma in order to compare with the groovy.transform linepragmas
	LinePragma annLinePragma = ann.data.GetByIndex(0).LinePragma;
	bool found = false;
	foreach(CxList it in importsTransformImmutable)
	{
		LinePragma itLinePragma = it.data.GetByIndex(0).LinePragma;
		if (itLinePragma.Line < annLinePragma.Line)
		{
			found = true;
		}
	}
	if (!found)
	{
		finalImmutableAnnotations.Add(ann);
	}
}

result = collectAll + finalImmutableAnnotations;