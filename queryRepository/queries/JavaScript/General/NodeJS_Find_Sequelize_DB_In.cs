/**
Find all DB_In's related to the Sequelize ORM
**/

CxList unknownRefs = Find_UnknownReference();
CxList declarators = Find_Declarators();
CxList objectCreations = Find_ObjectCreations();
CxList methods = Find_Methods();
CxList assignLefts = Find_Assign_Lefts();
CxList models = All.NewCxList();
CxList declUnk = All.NewCxList();
declUnk.Add(unknownRefs);
declUnk.Add(declarators);
	    
// All instance Model methods of Sequelize 
List<string> sequelizeDbInModelMethods = new List<string> {
		"bulkCreate",
		"create",
		"count",
		"destroy",
		"drop",			
		"findAll",		
		"findByPk",		
		"findAndCountAll",	
		"findOne",
		"findOrBuild",	
		"findOrCreate",
		"findCreateFind",	
		"update",		
		"upsert",
		"save",
		"sync",
		"truncate",
		"removeAttribute",		
		// Will be removed in V5 - Official Alternative
		"insertOrUpdate", // upsert	
		"find", // findOne 			
		"findAndCount",	// findAndCountAll
		"findOrInitialize", // findOrBuild
		"updateAttributes", // update
		"findById", // findByPk
		"findByPrimary", // findByPk
		"all" // findAll
		};
		
// Sequelize methods that return a model instance
List<string> returnModelMethods = new List<string> {"define", "model", "import", "build", "create"};

CxList sequelizeObjCreations = objectCreations.FindByShortName("Sequelize");

// Find methods from Sequalize API
CxList sequelizeMethods = methods.FindByMemberAccess("Sequelize.*");
sequelizeMethods.Add(sequelizeObjCreations.GetMembersOfTarget());  
sequelizeMethods.Add(unknownRefs.FindAllReferences(sequelizeObjCreations.GetAssignee()).GetMembersOfTarget());

CxList sequelizeInstances = assignLefts.GetByAncs(sequelizeMethods.GetFathers());
sequelizeMethods.Add(unknownRefs.FindAllReferences(sequelizeInstances).GetMembersOfTarget());


CxList modelProviders = sequelizeMethods.FindByShortNames(returnModelMethods);


// Get all the left sides of the model providers 
CxList leftSides = declUnk.FindAllReferences(assignLefts.GetByAncs(modelProviders.GetFathers()));


// For each left side fetch the model reference 
foreach(CxList leftAssign in leftSides)
{
	try
	{
		CSharpGraph g = leftAssign.GetFirstGraph();
		if(g != null && g.LinePragma != null && g.ShortName != null)
		{
			// Filter the left assigns 
			models.Add(unknownRefs.FindByFileId(g.LinePragma.GetFileId()).FindByShortName(g.ShortName));
		}
	}
	catch(Exception e)
	{
		cxLog.WriteDebugMessage(e);
	}
}

models = declUnk.FindAllReferences(models);
models.Add(modelProviders);

//Get the member targets of the models and the providers and find the db in methods
result = models.GetMembersOfTarget().FindByShortNames(sequelizeDbInModelMethods);

// Sequelize instance dbIn methods
result.Add(methods.FindByMemberAccess("Sequelize.*"));

result.Add(NodeJS_Find_Sequelize_DB_InOut());