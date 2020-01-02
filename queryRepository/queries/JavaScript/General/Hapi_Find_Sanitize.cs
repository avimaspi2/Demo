//This query searches for sanitzing inputs using the Joi packgage.
CxList joiReferences = Hapi_Find_Joi_References();
CxList hapiRoutes = Hapi_Find_Routes();
CxList fields = Find_FieldDecls();
CxList memberAccess = Find_MemberAccesses();

CxList joiReferencesMembers = joiReferences.GetMembersOfTarget();
CxList sanitizedParameters = joiReferencesMembers.FindByShortNames(new List<string> {"number", "date",
		"boolean"});

CxList sanitizedParametersKeys = sanitizedParameters.GetRightmostMember().GetAssignee();
CxList optionFields = fields.FindByShortName("options");
CxList handlerFields = fields.FindByShortName("handler");
CxList validationFields = fields.FindByShortName("validate").GetByAncs(optionFields);
CxList joiValidatedKeys = sanitizedParametersKeys.GetByAncs(validationFields);
CxList allWithinRouteHandlers = memberAccess.GetByAncs(handlerFields);

/*
//Match joi validated keys with respective references within route handler
server.route({ method: 'GET', path: '/getIdWithValidation',
    handler: function(request, reply) { return names[parseInt(request.query.id)] },
    options: { validate: { query: { id: Joi.number().integer().min(0).max(3) } } }
});
*/
foreach(CxList route in hapiRoutes){
	CxList sanitizedKeysWithinRouteValidation = joiValidatedKeys.GetByAncs(route);
	CxList routeInputs = allWithinRouteHandlers.GetByAncs(route);
	foreach(CxList sanitizedKey in sanitizedKeysWithinRouteValidation){
		string name = "*." + sanitizedKey.GetName();
		result.Add(routeInputs.FindByMemberAccess(name));
	}
}

/*
//Add references sanitized by schema according to the following scenario
server.route({ method: 'GET', path: '/getNameWithValidation',
        handler: function(request, reply) {
            if(Joi.validate(request.query, schema)){ return names[parseInt(request.query.id)] }
            else return "Invalid ID. Please insert an integer ID between 0 and 3";
        }
    });
*/
CxList methods = Find_Methods();
CxList declarators = Find_Declarators();
CxList unknownReferences = Find_UnknownReference();

CxList schemaMethods = methods.FindByShortNames(new List<string> {"keys", "object", "append"});
CxList otherJoiValidatedKeys = sanitizedParametersKeys - joiValidatedKeys;
CxList schemas = otherJoiValidatedKeys.GetAncOfType(typeof(MethodInvokeExpr)) * schemaMethods;
schemas.Add(unknownReferences.FindAllReferences(schemas.GetAssignee()));
CxList joiValidateMehtods = joiReferences.GetMembersOfTarget().FindByShortName("validate");

foreach(CxList schema in schemas){
	CxList schemaValidatedKeys = otherJoiValidatedKeys.GetByAncs(declarators.FindDefinition(schema));
	CxList validations = joiValidateMehtods.DataInfluencedBy(schema)
		.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	validations.Add(methods.FindAllReferences(methods.GetMethod(validations)));
	CxList handlerWithValidation = validations.GetAncOfType(typeof(FieldDecl)) * handlerFields;
	CxList allWithinValidatedHandler = allWithinRouteHandlers.GetByAncs(handlerWithValidation);
	foreach(CxList schemavalidatedKey in schemaValidatedKeys){
		string name = "*." + schemavalidatedKey.GetName();
		result.Add(allWithinValidatedHandler.FindByMemberAccess(name));
	}
}