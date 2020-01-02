CxList creditList = Credit_privacy_violation_list();

CxList excludeCreditList = creditList.FindByShortName("*credits*", false);
excludeCreditList.Add(Find_Dead_Code_Contents());
result = excludeCreditList;