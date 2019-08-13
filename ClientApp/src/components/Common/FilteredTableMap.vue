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
    'getItemMarkerGeometry'
  ],
  data() {
    return {
      markers: [],
      polygons: [],
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
      this.map = new google.maps.Map(this.$el);

      this.map.setCenter({lat: 43.0315528, lng: -87.9730566});
      this.map.fitBounds(new google.maps.LatLngBounds({lat: 43.191766, lng: -88.062779}, {lat: 42.916096, lng: -87.880899}));

      this.map.addListener('bounds_changed', e => {

        if (boundsChangedTimeout != null)
          clearTimeout(boundsChangedTimeout);

        boundsChangedTimeout = setTimeout(() => {
          let bounds = this.map.getBounds();

          this.$emit('bounds-changed', {
            ne: {
              lat: bounds.na.j,
              lng: bounds.ga.j
            },
            sw: {
              lat: bounds.na.l,
              lng: bounds.ga.l
            }
          });

        }, 1000);
      });      

    } catch (error) {
      console.error(error);
    }
  },
  methods: {
    drawMarkers() {
      let markers = [];
      let map = this.map;

      this.markers.forEach(m => {
        m.setMap(null);
      });
      this.markers = [];

      this.items.forEach(i => {

        let geometry = this.getItemMarkerGeometry(i);

        if (!geometry)
          return;

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

        this.markers.push(marker);
      });
    },
    drawPolygons() {
      let polygons = [];
      let map = this.map;

      this.polygons.forEach(p => {
        p.setMap(null);
      });
      this.polygons = [];

      this.items.forEach(i => {

        let geometry = this.getItemPolygonGeometry(i);

        if (!geometry)
          return;

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

        this.polygons.push(polygon);
      });
    }
  },
  watch: {
    async items() {

      if (typeof this.getItemPolygonGeometry === 'function')
        this.drawPolygons();

      if (typeof this.getItemMarkerGeometry === 'function')
        this.drawMarkers();
    }
  }
};
</script>

<style>
.map {
  width: 100%;
  height: 400px;
}
</style>