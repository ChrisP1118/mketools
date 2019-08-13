<template>
  <div class="map"/>
</template>

<script>
import gmapsInit from '../Common/googlemaps';

export default {
  name: 'FilteredTableMap',
  props: [
    'items',
    'getInfoWindowText'
  ],
  data() {
    return {
      polygons: [],
      google: null,
      map: null
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
  watch: {
    async items() {
      console.log('items changed');

      let polygons = [];

      let map = this.map;
      let getInfoWindowText = this.getInfoWindowText;

      this.polygons.forEach(p => {
        p.setMap(null);
      });
      this.polygons = [];

      this.items.forEach(i => {

        /*
        if (i._raw.Location) {
          let marker = new google.maps.Marker({
            position: {
              lat: i._raw.Location.Centroid.coordinates[1],
              lng: i._raw.Location.Centroid.coordinates[0]
            },
            map: map
          });
        }
        */

        let coords = [];
        if (i._raw.Location) {
          let x = i._raw.Location.Outline.coordinates[0].forEach(y => {
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

          this.google.maps.event.addListener(polygon, 'click', e => {
            let infoWindow = new google.maps.InfoWindow({
              content: getInfoWindowText(i)
            });
            infoWindow.setPosition(e.latLng);
            infoWindow.open(map);
          });

          // polygon.addListener('click', () => {
          //   infowindow.open(map, polygon);
          //   console.log(i._raw.HOUSE_NR_LO + ' ' + i._raw.SDIR + ' ' + i._raw.STREET + ' ' + i._raw.STTYPE);
          // })

          this.polygons.push(polygon);
        }
      });      
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