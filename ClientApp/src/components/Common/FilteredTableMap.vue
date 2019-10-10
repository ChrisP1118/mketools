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
    'getItemIcon',
    'getItemId',
    'openInfoWindowItem'
  ],
  data() {
    return {
      wrappers: [],
      google: null,
      map: null,
      openInfoWindow: null,
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
        //gestureHandling: 'greedy'
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

      if (this.items)
        this.redraw();

    } catch (error) {
      console.error(error);
    }
  },
  methods: {
    drawMarkers(map) {
      if (!this.google)
        return;

      let newWrappers = [];

      this.items.forEach(i => {

        let geometry = this.getItemMarkerGeometry(i);

        if (!geometry)
          return;

        let existingMarkerWrapper = this.wrappers.find(w => w.id == this.getItemId(i));
        if (existingMarkerWrapper) {
          newWrappers.push(existingMarkerWrapper);
          return;
        } else if (geometry) {

          let icon = 'wht-blank.png';
          if (this.getItemIcon)
            icon = this.getItemIcon(i);

          let marker = new google.maps.Marker({
            position: geometry,
            map: map,
            icon: 'https://maps.google.com/mapfiles/kml/paddle/' + icon
          });

          marker.addListener('click', e => {
            if (this.openInfoWindow)
              this.openInfoWindow.close();

            this.openInfoWindow = new google.maps.InfoWindow({
              content: this.getItemInfoWindowText(i)
            });
            this.openInfoWindow.open(map, marker);
          });

          newWrappers.push({
            id: this.getItemId(i),
            marker: marker
          });
        }
      });

      let oldMarkerWrappers = this.wrappers.filter(m => { return !(newWrappers.find(x => x.id == m.id)); });
      oldMarkerWrappers.forEach(w => { 
        w.marker.setMap(null);
      });
      this.wrappers = newWrappers;
    },
    drawPolygons(map) {
      if (!google)
        return;

      let newWrappers = [];

      this.items.forEach(i => {

        let geometry = this.getItemPolygonGeometry(i);

        if (!geometry)
          return;

        let existingWrapper = this.wrappers.find(w => w.id == this.getItemId(i));
        if (existingWrapper) {
          newWrappers.push(existingWrapper);
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

          newWrappers.push({
            id: this.getItemId(i),
            polygon: polygon
          })
        }
      });

      let oldPolygonWrappers = this.wrappers.filter(m => { return !(newWrappers.find(x => x.id == m.id)); });
      oldPolygonWrappers.forEach(w => { 
        w.polygon.setMap(null);
      });
      this.wrappers = newWrappers;
    },
    redraw: function () {
      if (typeof this.getItemPolygonGeometry === 'function')
        this.drawPolygons(this.map);

      if (typeof this.getItemMarkerGeometry === 'function')
        this.drawMarkers(this.map);
    }
  },
  watch: {
    async items() {
      this.redraw();
    },
    openInfoWindowItem (newValue, oldValue) {
      let item = newValue['_item'];
      let markerWrapper = this.wrappers.find(m => m.id == this.getItemId(item));

      if (this.openInfoWindow)
        this.openInfoWindow.close();
        
      this.openInfoWindow = new google.maps.InfoWindow({
        content: this.getItemInfoWindowText(item)
      });
      this.openInfoWindow.open(this.map, markerWrapper.marker);
    }
  }
};
</script>
