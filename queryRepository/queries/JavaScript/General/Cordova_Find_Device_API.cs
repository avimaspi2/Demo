/*This query finds Apache Cordova's Network Device API usages.
Reference: https://cordova.apache.org/docs/en/latest/reference/cordova-plugin-device/index.html#installation */

result = All.FindByName("device.uuid");
result.Add(All.FindByName("device.serial"));