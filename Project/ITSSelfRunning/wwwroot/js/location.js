

var map;
var route = [];
var activityId;
var marker2;
var runnerId;
var uriGara;
var activityType;

function getLocation(actId, runId, actType, garaUri) {
    activityId = actId;
    runnerId = runId;
    uriGara = garaUri;
    activityType = actType;
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition, showError);


    }
    else { data.innerHTML = "Geolocation is not supported by this browser."; }
}



function showPosition(position) {
    map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: position.coords.latitude, lng: position.coords.longitude },
        zoom: 15
    });

    var marker = new google.maps.Marker({
        position: { lat: position.coords.latitude, lng: position.coords.longitude },
        map: map
    });
    marker2 = new google.maps.Marker({
        position: { lat: position.coords.latitude, lng: position.coords.longitude },
        map: map
    });

    route.push({ lat: position.coords.latitude, lng: position.coords.longitude });

   // var latlondata = "Latitude: " + position.coords.latitude + ", Longitude: " + position.coords.longitude;

    var msg = JSON.stringify({
        "latitude": position.coords.latitude,
        "longitude": position.coords.longitude,
        "time": new Date().toISOString(),
        "IdActivity": activityId,
        "IdRunner": runnerId,
        "Type": activityType,
        "UriGara" : uriGara
});

    sendPoint(msg);

    navigator.geolocation.watchPosition(movePosition, showError);
}



function movePosition(position) {

    var x = position.coords.latitude;
    var y = position.coords.longitude;

    map.setCenter(position.coord);
    marker2.setPosition(position.coord);
    route.push({ lat: x, lng: y });
    var flightPath = new google.maps.Polyline({
        path: route,
        geodesic: true,
        strokeColor: '#ffc266',
        strokeOpacity: 1.0,
        strokeWeight: 3
    });

    flightPath.setMap(map);


  //  var latlondata = "Latitude: " + position.coords.latitude + ", Longitude: " + position.coords.longitude;
    var msg = JSON.stringify({
        "latitude": position.coords.latitude,
        "longitude": position.coords.longitude,
        "time": new Date().toISOString(),
        "IdActivity": activityId,
        "IdRunner": runnerId,
        "Type": activityType,
        "UriGara": uriGara
});

    sendPoint(msg);
}







function showError(error) {
    if (error.code === 1) {
        data.innerHTML = "User denied the request for Geolocation.";
    }
    else if (err.code === 2) {
        data.innerHTML = "Location information is unavailable.";
    }
    else if (err.code === 3) {
        data.innerHTML = "The request to get user location timed out.";
    }
    else {
        data.innerHTML = "An unknown error occurred.";
    }
}




function sendPoint(msg) {
    $.ajax({
        method : "POST",
        url: "/Run/SendPoint",
        data: msg,
        type: "json",
       contentType: "application/json"
        /*dataType: "json" /*,
        success: (res) => {
             $('#Location').val(res);
        }*/
    });
}


function getPlace(callback) {

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            getPlaceLoc(position, callback);
        });
    }

}

function getPlaceLoc(position, callback) {
    var geocoder = new google.maps.Geocoder;
    geocoder.geocode({ 'location': { lat: position.coords.latitude, lng: position.coords.longitude } }, function (results, status) {
        if (status === 'OK') {
            if (results[0]) {
                callback(results[0].formatted_address);
            } else {
                window.alert('No results found');
            }
        } else {
            window.alert('Geocoder failed due to: ' + status);
        }
    });
}