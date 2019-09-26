<template>
  <div class="map"/>
</template>

<script>
import gmapsInit from '../Common/googlemaps';

export default {
  name: 'FilteredTableMap',
  props: [
    'items',
    'getItemInfoWindowText',
    'getItemPolygonGeometry',
    'getItemMarkerGeometry',
    'getItemId'
  ],
  data() {
    return {
      markerWrappers: [],
      polygonWrappers: [],
      google: null,
      map: null,
      openInfoWindow: null
    }
  },
  async mounted() {
    let boundsChangedTimeout = null;

    try {
      this.google = await gmapsInit();
      //const geocoder = new google.maps.Geocoder();
      this.map = new google.maps.Map(this.$el, {
        center: { lat: 43.0315528, lng: -87.9730566 },
        zoom: 10,
        gestureHandling: 'greedy'
      });

      //this.map.setCenter({lat: 43.0315528, lng: -87.9730566});
      //this.map.fitBounds(new google.maps.LatLngBounds({lat: 43.191766, lng: -88.062779}, {lat: 42.916096, lng: -87.880899}));

      this.map.addListener('bounds_changed', e => {

        if (boundsChangedTimeout != null)
          clearTimeout(boundsChangedTimeout);

        boundsChangedTimeout = setTimeout(() => {
          let bounds = this.map.getBounds();
          let ne = bounds.getNorthEast();
          let sw = bounds.getSouthWest();

          this.$emit('bounds-changed', {
            ne: {
              lat: ne.lat(),
              lng: ne.lng()
            },
            sw: {
              lat: sw.lat(),
              lng: sw.lng()
            }
          });

        }, 1000);
      });      

    } catch (error) {
      console.error(error);
    }
  },
  methods: {
    drawMarkers(map) {
      if (!google)
        return;

      let newMarkerWrappers = [];

      this.items.forEach(i => {

        let geometry = this.getItemMarkerGeometry(i);

        if (!geometry)
          return;

        let existingMarkerWrapper = this.markerWrappers.find(w => w.id == this.getItemId(i));
        if (existingMarkerWrapper) {
          newMarkerWrappers.push(existingMarkerWrapper);
          return;
        } else {

          let point = geometry.coordinates[0][0];

          let marker = new google.maps.Marker({
            position: {
              lat: point[1],
              lng: point[0]
            },
            map: map
          });

          marker.addListener('click', e => {
            if (this.openInfoWindow)
              this.openInfoWindow.close();

            this.openInfoWindow = new google.maps.InfoWindow({
              content: this.getItemInfoWindowText(i)
            });
            this.openInfoWindow.open(map, marker);
          });

          newMarkerWrappers.push({
            id: this.getItemId(i),
            marker: marker
          });
        }
      });

      let oldMarkerWrappers = this.markerWrappers.filter(m => { return !(newMarkerWrappers.find(x => x.id == m.id)); });
      oldMarkerWrappers.forEach(w => { 
        w.marker.setMap(null);
      });
      this.markerWrappers = newMarkerWrappers;
    },
    drawPolygons(map) {
        if (!google)
          return;

      let newPolygonWrappers = [];

      this.items.forEach(i => {

        let geometry = this.getItemPolygonGeometry(i);

        if (!geometry)
          return;

        let existingPolygonWrapper = this.polygonWrappers.find(w => w.id == this.getItemId(i));
        if (existingPolygonWrapper) {
          newPolygonWrappers.push(existingPolygonWrapper);
          return;
        } else {

          let coords = [];

          let x = geometry.coordinates[0].forEach(y => {
            coords.push({
              lat: y[1],
              lng: y[0]
            });
          })
          let polygon = new google.maps.Polygon({
            paths: coords,
            strokeColor: '#FF0000',
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: '#FF0000',
            fillOpacity: 0.35
          });
          polygon.setMap(this.map);

          polygon.addListener('click', e => {
            if (this.openInfoWindow)
              this.openInfoWindow.close();
              
            this.openInfoWindow = new google.maps.InfoWindow({
              content: this.getItemInfoWindowText(i)
            });
            this.openInfoWindow.setPosition(e.latLng);
            this.openInfoWindow.open(map);
          });

          newPolygonWrappers.push({
            id: this.getItemId(i),
            polygon: polygon
          })
        }
      });

      let oldPolygonWrappers = this.polygonWrappers.filter(m => { return !(newPolygonWrappers.find(x => x.id == m.id)); });
      oldPolygonWrappers.forEach(w => { 
        w.polygon.setMap(null);
      });
      this.polygonWrappers = newPolygonWrappers;

    }
  },
  watch: {
    async items() {

      if (typeof this.getItemPolygonGeometry === 'function')
        this.drawPolygons(this.map);

      if (typeof this.getItemMarkerGeometry === 'function')
        this.drawMarkers(this.map);
    }
  }
};
</script>
