import L from "leaflet";

const geolocation = [35.7779598, 51.4818181];

const map = L.map('map').setView(geolocation, 16);
L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
}).addTo(map);

// Removes Ukrain Flag
map.attributionControl.setPrefix('');

// Change Icon
const locationPinIcon = L.icon({
    iconUrl: '../images/contact_us/location-pin.png',
    shadowUrl: '../images/contact_us/location-pin-shadow.png',

    iconSize: [48, 48], // size of the icon
    shadowSize: [40, 40], // size of the shadow
    iconAnchor: [22, 40], // point of the icon which will correspond to marker's location
    shadowAnchor: [6, 30],  // the same for the shadow
    popupAnchor: [0, 30] // point from which the popup should open relative to the iconAnchor
});

L.marker(geolocation, { icon: locationPinIcon }).addTo(map);