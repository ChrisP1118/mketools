<template>
  <div style="height: 80vh; width: 100%;">
    <l-map style="height: 100%; width: 100%" :zoom="zoom" :center="center">
      <l-tile-layer :url="tileUrl" :attribution="attribution"></l-tile-layer>
      <l-circle v-if="circleCenter" :lat-lng="circleCenter" :radius="circleRadius" color="#bd2130" />
      <l-marker v-for="marker in markers" v-bind:key="marker.id" :lat-lng="marker.position" :icon="marker.icon">
        <l-popup :content="marker.popup"></l-popup>
      </l-marker>
      <l-polygon v-for="polygon in polygons" v-bind:key="polygon.id" :lat-lngs="polygon.coordinates" color="#dc3545" weight="1" fill-color="#fd7e14" fill-opacity="0.2">
        <l-popup :content="polygon.popup"></l-popup>
      </l-polygon>
    </l-map>
  </div>
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
    'getItemId'
  ],
  data() {
    return {
      //wrappers: [],
      //google: null,
      //map: null,
      //openInfoWindow: null,

      tileUrl: 'https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png',
      zoom: 12,
      center: [43.0315528, -87.9730566],
      attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors',
      markers: [],
      polygons: [],
      circleCenter: null,
      circleRadius: null

    }
  },
  methods: {
    drawMarkers(map) {

      this.markers = [];
      this.items.forEach(i => {
        let geometry = this.getItemMarkerGeometry(i);

        if (!geometry)
          return;

        let icon = 'wht-blank.png';
        if (this.getItemIcon)
          icon = this.getItemIcon(i);

        this.markers.push({
          id: this.getItemId(i),
          icon: L.icon({
            iconUrl: 'https://maps.google.com/mapfiles/kml/paddle/' + icon,
            iconSize: [40, 40],
            iconAnchor: [20, 0]
          }),
          position: geometry,
          popup: this.getItemInfoWindowText(i)
        });
      });



      // let newWrappers = [];

      // this.items.forEach(i => {


      //   let existingMarkerWrapper = this.wrappers.find(w => w.id == this.getItemId(i));
      //   if (existingMarkerWrapper) {
      //     newWrappers.push(existingMarkerWrapper);
      //     return;
      //   } else if (geometry) {

      //     let icon = 'wht-blank.png';
      //     if (this.getItemIcon)
      //       icon = this.getItemIcon(i);

      //     let marker = new google.maps.Marker({
      //       position: geometry,
      //       map: map,
      //       icon: 'https://maps.google.com/mapfiles/kml/paddle/' + icon
      //     });

      //     marker.addListener('click', e => {
      //       if (this.openInfoWindow)
      //         this.openInfoWindow.close();

      //       this.openInfoWindow = new google.maps.InfoWindow({
      //         content: this.getItemInfoWindowText(i)
      //       });
      //       this.openInfoWindow.open(map, marker);
      //     });

      //     newWrappers.push({
      //       id: this.getItemId(i),
      //       marker: marker
      //     });
      //   }
      // });

      // let oldMarkerWrappers = this.wrappers.filter(m => { return !(newWrappers.find(x => x.id == m.id)); });
      // oldMarkerWrappers.forEach(w => { 
      //   w.marker.setMap(null);
      // });
      // this.wrappers = newWrappers;
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

      // if (!google)
      //   return;

      // let newWrappers = [];

      // this.items.forEach(i => {

      //   let geometry = this.getItemPolygonGeometry(i);

      //   if (!geometry)
      //     return;

      //   let existingWrapper = this.wrappers.find(w => w.id == this.getItemId(i));
      //   if (existingWrapper) {
      //     newWrappers.push(existingWrapper);
      //     return;
      //   } else {

      //     let coords = [];

      //     let x = geometry.coordinates[0].forEach(y => {
      //       coords.push({
      //         lat: y[1],
      //         lng: y[0]
      //       });
      //     })
      //     let polygon = new google.maps.Polygon({
      //       paths: coords,
      //       strokeColor: '#FF0000',
      //       strokeOpacity: 0.8,
      //       strokeWeight: 2,
      //       fillColor: '#FF0000',
      //       fillOpacity: 0.35
      //     });
      //     polygon.setMap(this.map);

      //     polygon.addListener('click', e => {
      //       if (this.openInfoWindow)
      //         this.openInfoWindow.close();
              
      //       this.openInfoWindow = new google.maps.InfoWindow({
      //         content: this.getItemInfoWindowText(i)
      //       });
      //       this.openInfoWindow.setPosition(e.latLng);
      //       this.openInfoWindow.open(map);
      //     });

      //     newWrappers.push({
      //       id: this.getItemId(i),
      //       polygon: polygon
      //     })
      //   }
      // });

      // let oldPolygonWrappers = this.wrappers.filter(m => { return !(newWrappers.find(x => x.id == m.id)); });
      // oldPolygonWrappers.forEach(w => { 
      //   w.polygon.setMap(null);
      // });
      // this.wrappers = newWrappers;
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
    // openInfoWindowItem (newValue, oldValue) {
    //   let item = newValue['_item'];
    //   let markerWrapper = this.wrappers.find(m => m.id == this.getItemId(item));

    //   if (this.openInfoWindow)
    //     this.openInfoWindow.close();
        
    //   this.openInfoWindow = new google.maps.InfoWindow({
    //     content: this.getItemInfoWindowText(item)
    //   });
    //   this.openInfoWindow.open(this.map, markerWrapper.marker);
    // }
  },
  mounted() {
    if (this.items)
      this.redraw();

    /*
    let boundsChangedTimeout = null;

    try {
      this.google = await gmapsInit();
      this.map = new google.maps.Map(this.$el, {
        center: { lat: 43.0315528, lng: -87.9730566 },
        zoom: 10,
      });

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
    */
  }  
};
</script>
