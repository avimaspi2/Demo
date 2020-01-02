CxList stream = All.FindByType("FileStream");
stream.Add(All.FindByType("NetworkStream"));

result = stream.FindByType(typeof (Declarator));