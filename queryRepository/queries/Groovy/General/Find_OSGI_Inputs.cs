/* 

This query looks for inputs arriving via dependnecy injection through OSGI mechanism.

For reference: http://blog.knowhowlab.org/2010/10/osgi-tutorial-4-ways-to-activate-code.html 


It is divided into two parts:
Part 1 - Injection through Bundle Activator. 
Find dependency in MANIFEST_MF files, through field Bundle_Activator. 
Example:
  Bundle-Activator: activator_sample.Activator
Then take the class mentioned here and add its "start" and "stop" methods to the list of inputs.


Part 2 - Find dependency in XML files located in OSGI-INF directory (possibly in the future this will
 be extended to other directories mentioned in MANIFEST_MF as OSGI ).

Injection through Declarative Services:

//////////////////////////-
Method a: Refernce binding
//////////////////////////-
   <implementation class="com.obekt.ds.consumer.StandAloneClass"/>

   <reference bind="bindMethodName" cardinality="1..1" 
   interface="com.obekt.ds.common.ServiceDefinition" name="ServiceDefinition" 
   policy="dynamic" unbind="unbindMethodName"/>

For that case, add the "bind" method from the class declared for implementation, as input.


//////////////////
Method b: Service
//////////////////
   <implementation class="com.obekt.ds.impl.ServiceImpl"/>
   <service>
      <provide interface="com.obekt.ds.common.ServiceDefinition"/>
   </service>

Find the class and add all its methods as inputs (future improvements: Only methods declared via the interface).

*/

// General:
CxList tagImplementationClasses = All.NewCxList();
CxList startStopMethods = All.NewCxList();
CxList inputMethods = All.NewCxList();
CxList tagClass = All.NewCxList();
CxList classes = Find_Class_Decl();

CxList methods = All.FindByType(typeof(MethodDecl));

// Part 1: Handle MANIFEST.MF : 


//Contents of the manifest classes 
CxList manifest = All.GetByAncs(classes.FindByName("CxManifestClass*"));


// Find field "Bundle_Activator" and the class it points to:
CxList Manifest_Fields = manifest.GetByAncs(manifest.FindByType(typeof(Declarator)));
CxList Bundle_Activator_Fields = Manifest_Fields.FindByShortName("Bundle_Activator");
CxList Bundle_Activator_Fields_Ancs = manifest.GetByAncs(Bundle_Activator_Fields);
CxList bundleActivators = Bundle_Activator_Fields_Ancs.FindByAssignmentSide(CxList.AssignmentSide.Right);

foreach (CxList bundleActivator in bundleActivators) {
	StringLiteral bundleActivatorSL = (bundleActivator.data.GetByIndex(0) as StringLiteral);
	String bundleActivatorStr = (bundleActivatorSL != null) ? bundleActivatorSL.ShortName.Trim('"') : null;
	if (bundleActivatorStr != null) {
		// Find the class given in Bundle_Activator and its methods.
		CxList activatorClass = classes.FindByName(bundleActivatorStr);
		CxList activatorClassMethods = methods.GetByAncs(activatorClass);
		// Add the class's "start" and "stop" methods.
		startStopMethods.Add(activatorClassMethods.FindByShortName("start") + activatorClassMethods.FindByShortName("stop"));		
	}
}


// Part 2: Handle XML 
CxList OSGI_XML = All.FindByFileName(@"*\OSGI-INF\*").FindByFileName("*.xml");

// Returns the assignment ("=" at the top of parse tree) for XMLTAG = VALUE (On multiple XML files!)
CxList tagImplementationValue = Find_Xml_Value_By_Tag(OSGI_XML, "SCR_COMPONENT.IMPLEMENTATION", "*.CLASS"); 


// Go over all tags SCR_COMPONENT.IMPLEMENTATION->CLASS
//  Find both types of references and add the appropriate methods.
CxList TagImplementationXMLFiles = All.GetByAncs(tagImplementationValue.GetAncOfType(typeof(ClassDecl)));

foreach (CxList tagImplementationValueElement in tagImplementationValue) 
{
	// Find the class represnting the whole XML file where we found this tag.
	CxList tagValueClassDecl = tagImplementationValueElement.GetAncOfType(typeof(ClassDecl));
	CxList tagValueXMLFile = TagImplementationXMLFiles.GetByAncs(tagValueClassDecl);
	
	// Find the class mentioned in tag
	StringLiteral tagImplementationValueSL = tagImplementationValueElement.data.GetByIndex(0) as StringLiteral;
	String tagImplementationValueStr = 
		(tagImplementationValueSL != null) ? tagImplementationValueSL.ShortName.Trim('"') : null;
	if (tagImplementationValueStr != null) {
		tagClass = classes.FindByName(tagImplementationValueStr); // The class mentioned in tag
	}
	CxList tagClassMethods = methods.GetByAncs(tagClass); 		  // Find its methods

	// Find sibling with reference binding
	CxList referenceBind = Find_Xml_Value_By_Tag(tagValueXMLFile, "SCR_COMPONENT.REFERENCE", "*.BIND");
	StringLiteral bindNameStrSL = (referenceBind.data.GetByIndex(0) as StringLiteral);
	String bindNameStr = (bindNameStrSL != null) ? bindNameStrSL.ShortName.Trim('"') : null;
	if (bindNameStr != null) { 		// If this element has a sibling with refernce-bind, add the "bind" method:
		inputMethods.Add(tagClassMethods.FindByShortName(bindNameStr));
	}


	// Find sibling with service binding
	CxList serviceProvide = Find_Xml_Value_By_Tag(tagValueXMLFile, "SCR_COMPONENT.SERVICE", "*.PROVIDE.INTERFACE");
	StringLiteral serviceProvideSL = (serviceProvide.data.GetByIndex(0) as StringLiteral);
	String serviceProvideStr = (serviceProvideSL != null) ? serviceProvideSL.ShortName.Trim('"') : null;
	if (serviceProvideSL != null) { 		// If this element has a sibling with service-bind, add all methods
		inputMethods.Add(tagClassMethods); 
	}
}


result = All.GetParameters(inputMethods + startStopMethods);