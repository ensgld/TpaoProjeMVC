var map;
$(document).ready(function () {
  map = L.map("map").setView([39.9208, 32.8541], 6);
  L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
    maxZoom: 18,
    attribution: "© OpenStreetMap contributors",
  }).addTo(map);
  loadAllSahalar();

  $("#selectLocation").on("change", function () {
    var selectedLocation = $(this).val();
    getSahalarByLocation(selectedLocation);
  });
});
function loadAllSahalar() {
  $.ajax({
    url: "/Home/GetSahalarByLocation?locationName=", // boş gönderince tümünü döner
    method: "GET",
    success: function (sahalar) {
      sahalar.forEach(function (saha) {
        L.marker([saha.latitude, saha.longitude]).addTo(
          map
        ).bindPopup(`<b>${saha.sahaAdı}</b><br>${saha.city}<br>
            <a href="/Saha/Details/${saha.sahaId}" class="btn btn-sm btn-light mt-1">Detay</a>`);
      });
    },
    error: function () {
      console.error("Sahalar alınamadı.");
    },
  });
}

function getSahalarByLocation(locationName) {
  $.ajax({
    url:
      "/Home/GetSahalarByLocation?locationName=" +
      encodeURIComponent(locationName),
    method: "GET",
    success: function (sahalar) {
      map.eachLayer(function (layer) {
        if (layer instanceof L.Marker) {
          map.removeLayer(layer);
        }
      });
      var markers = []; //Zoomiçin flaan
      sahalar.forEach(function (saha) {
        var marker = L.marker([saha.latitude, saha.longitude]).addTo(map)
          .bindPopup(`<b>${saha.sahaAdı}</b><br>${saha.city}<br>
          <a href="/Saha/Details/${saha.sahaId}" class="btn btn-sm btn-light mt-1">Detay</a>
       `);
        markers.push(marker);
      });
      if (markers.length > 0) {
        //Seçilen yere zoom yapma
        var locationGroup = L.featureGroup(markers);
        map.fitBounds(locationGroup.getBounds().pad(0.1));
      } else {
        map.setView([39.9208, 32.8541], 6); //marker yoksa ankaraya konumlan
      }
    },
    error: function () {
      console.error("Saha verileri alınamadı.");
    },
  });
}
let mapInstance = null;

function getSahaByCoordinates(lat, lng, title, subtitle) {
  const mapContainer = document.getElementById("detailMap");

  if (!mapContainer) {
    console.warn("detailMap divi bulunamadı.");
    return;
  }

  if (mapInstance !== null) {
    mapInstance.remove();
    mapInstance = null;
  }

  mapInstance = L.map(mapContainer).setView([lat, lng], 13);

  L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
    attribution: "&copy; OpenStreetMap contributors",
  }).addTo(mapInstance);

  L.marker([lat, lng])
    .addTo(mapInstance)
    .bindPopup(`<b>${title}</b><br>${subtitle}`)
    .openPopup();

  setTimeout(() => {
    mapInstance.invalidateSize();
  }, 300);
}
