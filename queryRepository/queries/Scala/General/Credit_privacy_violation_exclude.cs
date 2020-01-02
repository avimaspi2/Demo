CxList creditList = Credit_privacy_violation_list();

CxList excludeCreditList = creditList.FindByShortName("*credits*", false);

result = excludeCreditList;