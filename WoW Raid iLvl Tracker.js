/* TODO
   Configure script to pull data from all cells in Player and Server columns until it finds 2 consecutive blanks
     This information will be added to the respective arrays and the ranges to affect will automatically be updated
     The row number for each cell found to have values will be added to the Rows array to update that automatically as new players are added
       
   May create functionality to automatically add new row by filling out character name, dropdown selection of server, and dropdown selection of Role
     This will add a row to the appropriate Role(Tank, Healer, Melee, Ranged, Sub) and fill out the Name and Server fields, which will allow UpdateArmory to affect it.  
*/

//Create a new menu option to run the UpdateArmory method directly from the spreadsheet
function onOpen() {
  var menuEntries = [ {name: "Update", functionName: "UpdateArmory"} ];
  ss.addMenu("Commands", menuEntries);
}

//Filters out blank elements for the given array
  function stripArray(values) {
  return values.filter(function(d) {
    return d.length && d[0] !== '';
  });
}

//Connect to Battle.Net API to collect character data and fill out spreadsheet 
function UpdateArmory(){
  var ss = SpreadsheetApp.getActiveSpreadsheet();
  var servers = stripArray(ss.getRange("S4:S29").getValues());
  var characters = stripArray(ss.getRange("B4:B29").getValues());
  var rows = [4,5,7,8,9,10,12,13,14,15,16,17,19,20,21,22,23,24,26,27,28,29];
  var equip = ["head","neck","shoulder","back","chest","tabard","wrist","hands","waist","legs","feet","finger1","finger2","trinket1","trinket2","mainHand"] 

  for (var i = 0; i < characters.length; i++){
    var url = "https://us.api.battle.net/wow/character/"+ servers[i] + "/" + characters[i] +"?fields=items&locale=en_US&apikey=x2qapsnqmsweaqftpcwzacs98vm6p2zx";
    var playerJSON = UrlFetchApp.fetch(url);
    var player = JSON.parse(playerJSON.getContentText());
    var range = SpreadsheetApp.getActiveSpreadsheet().getRange("C"+rows[i]+":R"+rows[i]);
    var ilvls = [[]];
  
    for(var j = 0; j < equip.length; j++){
      var slot = equip[j];     
      if(player.items[slot] == null){
        ilvls[0].push(1);
      }else{
      ilvls[0].push(player.items[slot].itemLevel);
      }
    }   
     range.setValues(ilvls);

  }
}
