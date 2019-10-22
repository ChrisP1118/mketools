<template>
  <div style="height: 80vh; width: 100%;">
    <l-map style="height: 100%; width: 100%" :zoom="zoom" :center="center" @update:zoom="zoomUpdated" @update:center="centerUpdated" @update:bounds="boundsUpdated">
      <l-tile-layer :url="tileUrl" :attribution="attribution"></l-tile-layer>
      <l-circle v-if="circleCenter" :lat-lng="circleCenter" :radius="circleRadius" color="#bd2130" />
      <l-marker v-for="marker in markers" v-bind:key="marker.id" :lat-lng="marker.position" :icon="marker.icon">
        <l-popup :content="marker.popup"></l-popup>
      </l-marker>
      <l-polygon v-for="polygon in polygons" v-bind:key="polygon.id" :lat-lngs="polygon.coordinates" color="#dc3545" :weight="1" fill-color="#fd7e14" :fill-opacity="0.2">
        <l-popup :content="polygon.popup"></l-popup>
      </l-polygon>
    </l-map>
  </div>
</template>

<script>
export default {
  name: 'FilteredTableMap',
  props: [
    'items',
    'getItemInfoWindowText',
    'getItemPolygonGeometry',
    'getItemMarkerPosition',
    'getItemIcon',
    'getItemId',
    'locationData'
  ],
  data() {
    return {

      tileUrl: 'https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png',
      zoom: 11,
      center: [43.0315528, -87.9730566],
      bounds: null,
      attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors',
      markers: [],
      polygons: [],
      circleCenter: null,
      circleRadius: null

    }
  },
  methods: {
    zoomUpdated (zoom) {
      this.zoom = zoom;
    },
    centerUpdated (center) {
      this.center = center;
    },
    boundsUpdated (bounds) {
      this.bounds = bounds;

      this.$emit('bounds-changed', {
        ne: {
          lat: this.bounds._northEast.lat,
          lng: this.bounds._northEast.lng
        },
        sw: {
          lat: this.bounds._southWest.lat,
          lng: this.bounds._southWest.lng
        }
      });

    },
    drawMarkers(map) {

      this.markers = [];
      this.items.forEach(i => {
        let geometry = this.getItemMarkerPosition(i);

        if (!geometry)
          return;

        let icon = 'https://maps.google.com/mapfiles/kml/paddle/wht-blank.png';
        if (this.getItemIcon)
          icon = this.getItemIcon(i);

        this.markers.push({
          id: this.getItemId(i),
          icon: L.icon({
            iconUrl: icon,
            iconSize: [40, 40],
            iconAnchor: [20, 0]
          }),
          position: geometry,
          popup: this.getItemInfoWindowText(i)
        });
      });
    },
    drawPolygons(map) {

      this.polygons = [];
      this.items.forEach(i => {
        let geometry = this.getItemPolygonGeometry(i);

        if (!geometry)
          return;

        let coords = [];
        geometry.coordinates[0].forEach(y => {
          coords.push({
            lat: y[1],
            lng: y[0]
          });
        });

        this.polygons.push({
          id: this.getItemId(i),
          coordinates: [
            coords
          ],
          popup: this.getItemInfoWindowText(i)
        });
      });
    },
    redraw: function () {
      if (typeof this.getItemPolygonGeometry === 'function')
        this.drawPolygons(this.map);

      if (typeof this.getItemMarkerPosition === 'function')
        this.drawMarkers(this.map);
    }
  },
  watch: {
    async items() {
      this.redraw();
    },
    locationData: function (newValue, oldValue) {
      //console.log('filtered data map - location: ' + (newValue ? newValue.lat + ',' + newValue.lng : '(null)'));

      if (newValue) {
        this.center = [newValue.lat, newValue.lng];
        this.zoom = 16;
      } else {
        this.center = [43.0315528, -87.9730566];
        this.zoom = 11;
      }
    }
  },
  mounted() {
    if (this.items)
      this.redraw();
  }  
};
</script>
