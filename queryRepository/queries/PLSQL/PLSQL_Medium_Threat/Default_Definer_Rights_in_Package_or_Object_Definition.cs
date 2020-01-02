//Packages & objects which have definer rights privileges either by AUTHID DEFINER clause or by default
//if there is no AUTHID clause in their declaration.
//This issue can be solved by adding AUTHID CURRENT_USER clause to the pacakge/object declaration. 

result = Find_Missing_Current_User_Rights_In_Class();