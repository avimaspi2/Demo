/*
	This query looks for methods in which server socket bindings to variable interfaces 
	with the same port number happen.

	EX: public void bindSocket(Socket socket,Interface interface){
			socket.bind(new InetSocketAddress(interface, 2000));
		}
*/
CxList integers = Find_Integers();
CxList integerLiterals = Find_IntegerLiterals();
CxList zeros = integerLiterals.FindByShortName("0");
integers.Add(integerLiterals);

CxList allSocketAddress = All.FindByType("InetSocketAddress");
CxList portNumberAsSecondParameter = integers.GetParameters(allSocketAddress, 1);

/*
	Passing port 0 to the InetSocketAddress constructor instructs it to bind 
	to a random available temporary port, which is safe and therefore not a result
*/

zeros.Add(portNumberAsSecondParameter.DataInfluencedBy(zeros));

portNumberAsSecondParameter -= zeros;

/*
	Only constructors of InetSocketAddress with two parameters matter since initializing 
	it with a port number only (one parameter), binds it with the wildcard interface 
	(0.0.0.0) which can't be binded twice.
*/
CxList socketAddressWithTwoParameters = allSocketAddress.FindByParameters(portNumberAsSecondParameter);
CxList interfaceValue = All.GetParameters(socketAddressWithTwoParameters, 0);
interfaceValue -= interfaceValue.FindByType(typeof(Param));

/*
	Remove results binding to the same interface. i.e. using a string literal as interface.
*/
CxList strings = Find_Strings();
CxList safeInterfaceValue = (interfaceValue * strings);
safeInterfaceValue.Add(interfaceValue.DataInfluencedBy(strings));

/*
	Remove references to localhost name since they are constant although possibly not strings.
*/
safeInterfaceValue.Add(interfaceValue.FindByShortName("*localhost*", false));

/*
	Return parent method encapsulating unsafe bindings since calling them multiple times can lead
	to the vulnerability.
*/
CxList methods = Find_Methods();
CxList bindings = methods.FindByShortName("bind", false);
CxList unsafeSocketAddresses = socketAddressWithTwoParameters.FindByParameters(interfaceValue - safeInterfaceValue);
result = methods.GetMethod(bindings.DataInfluencedBy(unsafeSocketAddresses));