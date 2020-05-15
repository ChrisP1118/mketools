<template>
  <div>
    <b-alert :show="!isShowingParcelOverlays" variant="info">
      Zoom in on the map to view parcel overlays.
    </b-alert>
    <b-alert :show="!isShowingAllParcels" variant="info">
      Only 200 parcels are shown on the map below.
    </b-alert>
    <l-map v-if="position" style="height: 80vh; width: 100%" :zoom="zoom" :center="position" @update:zoom="zoomUpdated" @update:bounds="boundsUpdated">
      <l-tile-layer :url="tileUrl" :attribution="attribution"></l-tile-layer>
      <l-marker :lat-lng="position" :icon="icon">
      </l-marker>
      <l-polygon v-for="polygon in polygons" v-bind:key="polygon.map_id" :lat-lngs="polygon.coordinates" :color="polygon.color" :weight="polygon.weight" :fill-color="polygon.fillColor" :fill-opacity="polygon.fillOpacity">
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
      commonParcels: null,

      tileUrl: 'https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png',
      zoom: 18,
      attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors',
      polygons: [],
      icon: L.icon({
        iconUrl: 'https://maps.google.com/mapfiles/kml/paddle/wht-blank.png',
        iconSize: [40, 40],
        iconAnchor: [20, 0]
      }),

      bounds: null,

      parcelLoadLimit: 250,
      minZoomForParcels: 17
    };
  },
  computed: {
    isShowingParcelOverlays () {
      return this.zoom >= this.minZoomForParcels;
    },
    isShowingAllParcels () {
      return !this.commonParcels || this.commonParcels.length < this.parcelLoadLimit;
    }
  },
  methods: {
    zoomUpdated (zoom) {
      this.zoom = zoom;
    },
    boundsUpdated (bounds) {
      this.bounds = bounds;
      this.loadCommonParcels();
    },
    loadCommonParcels: function () {
      let latDiff = 0.0015;
      let lngDiff = 0.0020;

      if (this.zoom >= this.minZoomForParcels) {
        let url = '';
        if (this.bounds)
          url = '/api/commonParcel?limit=' + this.parcelLoadLimit + '&includes=parcels&northBound=' + this.bounds._northEast.lat + '&southBound=' + this.bounds._southWest.lat + '&eastBound=' + this.bounds._northEast.lng + '&westBound=' + this.bounds._southWest.lng;
        else
          url = '/api/commonParcel?limit=' + this.parcelLoadLimit + '&includes=parcels&northBound=' + (this.position.lat + latDiff) + '&southBound=' + (this.position.lat - latDiff) + '&eastBound=' + (this.position.lng + lngDiff) + '&westBound=' + (this.position.lng - lngDiff);

        axios
          .get(url)
          .then(response => {
            this.commonParcels = response.data;
            this.showCommonParcels();
          })
          .catch(error => {
            console.log(error);
          });
      } else {
        this.commonParcels = [];
      }
    },
    showCommonParcels: function () {
      let newPolygons = [];

      this.commonParcels.forEach(i => {
        let coords = [];
        i.outline.coordinates[0].forEach(y => {
          coords.push({
            lat: y[1],
            lng: y[0]
          });
        });

        let polygon = this.polygons.find(x => x.map_id == i.map_id);
        if (polygon) {
          newPolygons.push(polygon);
        } else {
          newPolygons.push({
            map_id: i.map_id,
            coordinates: [
              coords
            ],
            popup: this.$store.getters.getCommonParcelInfoWindow(i),
            color: this.$store.getters.getCommonParcelPolygonColor(i),
            weight: this.$store.getters.getCommonParcelPolygonWeight(i),
            fillColor: this.$store.getters.getCommonParcelPolygonFillColor(i),
            fillOpacity: this.$store.getters.getCommonParcelPolygonFillOpacity(i),
          });
        }
      });

      this.polygons = newPolygons;
    },
  },
  watch: {
    position: function () {
      if (this.position)
        this.loadCommonParcels();
    }
  },
  async mounted () {
    if (this.position)
      this.loadCommonParcels();
  }
};
</script>