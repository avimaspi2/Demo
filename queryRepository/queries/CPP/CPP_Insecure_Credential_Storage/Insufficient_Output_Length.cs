/// <summary>
/// The following query will find insufficient output length.
/// </summary>

// Limit from where we consider it safe
int outputLength = 32;

if(param.Length == 1 && param[0] is int)
{
	// override output length limit passed as argument
	outputLength = (int) param[0];
}

CxList insuficientOutputLength = All.NewCxList();

// Insufficient_Output_Length in Libsodium (https://libsodium.org)
insuficientOutputLength.Add(Find_Libsodium_Insufficient_Output_Length(outputLength));

// Insufficient_Output_Length in Crypto++ (https://www.cryptopp.com)
insuficientOutputLength.Add(Find_Cryptopp_Insufficient_Output_Length(outputLength));

// Insufficient_Output_Length in OpenSSL (https://www.openssl.org)
insuficientOutputLength.Add(Find_OpenSSL_Insufficient_Output_Length(outputLength));

// Insufficient_Output_Length in Botan (https://botan.randombit.net)
insuficientOutputLength.Add(Find_Botan_Insufficient_Output_Length(outputLength));

// Results (reduce flow)
result = insuficientOutputLength.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);