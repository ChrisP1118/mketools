<template>
  <div>
    <l-map v-if="position" style="height: 80vh; width: 100%" :zoom="zoom" :center="position">
      <l-tile-layer :url="tileUrl" :attribution="attribution"></l-tile-layer>
      <l-marker :lat-lng="position" :icon="icon">
      </l-marker>
      <l-polygon v-for="polygon in polygons" v-bind:key="polygon.id" :lat-lngs="polygon.coordinates" color="#dc3545" :weight="1" fill-color="#fd7e14" :fill-opacity="0.2">
        <l-popup :content="polygon.popup"></l-popup>
      </l-polygon>
    </l-map>
  </div>
</template>

<script>
import axios from "axios";
import moment from 'moment'

export default {
  name: "NearbyMap",
  props: [
    'position'
  ],
  data() {
    return {
      properties: null,

      tileUrl: 'https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png',
      zoom: 18,
      attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors',
      polygons: [],
      icon: L.icon({
        iconUrl: 'https://maps.google.com/mapfiles/kml/paddle/wht-blank.png',
        iconSize: [40, 40],
        iconAnchor: [20, 0]
      })
    };
  },
  computed: {
  },
  methods: {
    load: function () {
      let latDiff = 0.0005;
      let lngDiff = 0.0010;

      axios
        .get('/api/property?limit=100&northBound=' + (this.position.lat + latDiff) + '&southBound=' + (this.position.lat - latDiff) + '&eastBound=' + (this.position.lng + lngDiff) + '&westBound=' + (this.position.lng - lngDiff))
        .then(response => {
          this.properties = response.data;
          this.showProperties();
        })
        .catch(error => {
          console.log(error);
        });
    },
    showProperties: function () {
      this.polygons = [];
      this.properties.forEach(i => {
        let coords = [];
        i.parcel.outline.coordinates[0].forEach(y => {
          coords.push({
            lat: y[1],
            lng: y[0]
          });
        });

        let address = i.house_nr_lo;
        if (i.house_nr_hi != i.house_nr_lo)
          address += '-' + i.house_nr_hi;
        address += ' ' + i.sdir + ' ' + i.street + ' ' + i.sttype;

        let owner = i.owner_name_1;
        if (i.owner_name_2)
          owner += '<br />' + i.owner_name_2;
        if (i.owner_name_3)
          owner += '<br />' + i.owner_name_3;
        owner += '<br />' + i.owner_mail_addr + '<br />' + i.owner_city_state;
        
        this.polygons.push({
          id: i.taxkey,
          coordinates: [
            coords
          ],
          popup: '<h4>' + address + '</h4><div>' + owner + '</div>'
        });
      });

    },
  },
  watch: {
    position: function () {
      if (this.position)
        this.load();
    }
  },
  async mounted () {
    if (this.position)
      this.load();
  }
};
</script>