<template>
  <div style="height: 80vh; width: 100%;">
    <l-map ref="filteredTableMap" style="height: 100%; width: 100%" :zoom="zoom" :center="center" @update:zoom="zoomUpdated" @update:center="centerUpdated" @update:bounds="boundsUpdated">
      <l-tile-layer :url="tileUrl" :attribution="attribution"></l-tile-layer>
      <l-circle v-if="circleCenter" :lat-lng="circleCenter" :radius="circleRadius" color="#bd2130" />
      <l-marker v-for="marker in markers" v-bind:key="marker.id" :lat-lng="marker.position" :icon="marker.icon">
        <l-popup :content="marker.popup"></l-popup>
      </l-marker>
      <l-polygon v-for="polygon in polygons" v-bind:key="polygon.id" :lat-lngs="polygon.coordinates" :color="polygon.color" :weight="polygon.weight" :fill-color="polygon.fillColor" :fill-opacity="polygon.fillOpacity">
        <l-popup :content="polygon.popup"></l-popup>
      </l-polygon>
    </l-map>
    <b-alert :show="infoMessage" variant="info">
      {{infoMessage}}
    </b-alert>    
  </div>
</template>

<script>
export default {
  name: 'FilteredTableMap',
  props: [
    'defaultZoomWithLocationData',
    'defaultZoomWithoutLocationData',
    'items',
    'getItemInfoWindowText',
    'getItemPolygonGeometry',
    'getItemMarkerPosition',
    'getItemIcon',
    'getItemId',
    'getItemPolygonColor',
    'getItemPolygonWeight',
    'getItemPolygonFillColor',
    'getItemPolygonFillOpacity',
    'locationData',
    'infoMessage'
  ],
  data() {
    return {
      tileUrl: 'https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png',
      zoom: this.defaultZoomWithoutLocationData,
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

      this.$emit('zoom-changed', zoom);
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
        if (geometry.type == 'Polygon') {
          geometry.coordinates[0].forEach(y => {
            coords.push({
              lat: y[1],
              lng: y[0]
            });
          });
        } else if (geometry.type == 'MultiPolygon') {
          geometry.coordinates[0][0].forEach(y => {
            coords.push({
              lat: y[1],
              lng: y[0]
            });
          });
        }

        this.polygons.push({
          id: this.getItemId(i),
          coordinates: [
            coords
          ],
          popup: this.getItemInfoWindowText(i),
          color: this.getItemPolygonColor ? this.getItemPolygonColor(i) : '#dc3545',
          weight: this.getItemPolygonWeight ? this.getItemPolygonWeight(i) : 1,
          fillColor: this.getItemPolygonFillColor ? this.getItemPolygonFillColor(i) : '#fd7e14',
          fillOpacity: this.getItemPolygonFillOpacity ? this.getItemPolygonFillOpacity(i) : 0.2
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
      if (newValue) {
        //this.zoom = this.defaultZoomWithLocationData;
        //this.center = [newValue.lat, newValue.lng];
        // https://github.com/vue-leaflet/Vue2Leaflet/issues/170
        this.$refs.filteredTableMap.mapObject.setView(L.latLng(newValue.lat, newValue.lng), this.defaultZoomWithLocationData);
      } else {
        //this.zoom = this.defaultZoomWithoutLocationData;
        //this.center = [43.0315528, -87.9730566];
        this.$refs.filteredTableMap.mapObject.setView(L.latLng(43.0315528, -87.9730566), this.defaultZoomWithoutLocationData);
      }
    }
  },
  mounted() {
    if (this.items)
      this.redraw();
  }  
};
</script>
