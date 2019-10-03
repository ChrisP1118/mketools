<template>
  <div>
    <div class="map" id="basicMap" />
  </div>
</template>

<script>
import axios from "axios";
import gmapsInit from '../Common/googlemaps';
import moment from 'moment'

export default {
  name: "BasicMap",
  mixins: [],
  props: {
    hours: {
      type: Number,
      default: 4
    },
    mapItemLimit: {
      type: Number,
      default: 200
    },
    filter: {
      type: String,
      default: ''
    },
    locationData: null,
    distance: {
      type: Number,
      default: null
    }
  },
  data() {
    return {
      google: null,
      map: null,
      bounds: null,
      markerCache: [],
      markerCacheBounds: null,
      circle: null
    }
  },
  computed: {
  },
  methods: {
    loadAllMarkers: function () {
      this.loadPoliceDispatchMarkers();
      this.loadFireDispatchMarkers();
    },
    loadPoliceDispatchMarkers: function () {
      let now = moment().subtract(this.hours, 'hours').format('YYYY-MM-DD HH:mm:ss');
      let filter = 'ReportedDateTime%20%3E%3D%20%22' + encodeURIComponent(now) + '%22' + this.getBoundsFilter();

      if (!this.areBoundsCached()) {
        this.showMarkers();
      } else {
        axios
          .get('/api/PoliceDispatchCall?offset=0&limit=' + this.mapItemLimit + '&order=ReportedDateTime%20desc&filter=' + filter)
          .then(response => {
            response.data.forEach(i => {
              if (!i.Geometry || !i.Geometry.coordinates || !i.Geometry.coordinates[0] || !i.Geometry.coordinates[0][0])
                return;

              if (this.markerCache.find(x => x.type == 'PoliceDispatch' && x.id == i.CallNumber))
                return;

              let time = moment(i.ReportedDateTime).format('llll');
              let fromNow = moment(i.ReportedDateTime).fromNow();

              // http://kml4earth.appspot.com/icons.html#paddle
              let icon = 'wht-blank.png';
              switch (i.NatureOfCall) {

                // Red: Violent crime
                case 'BATTERY':
                case 'FIGHT':
                case 'BATTERY DV':
                case 'HOLDUP ALARM':
                  icon = 'red-blank.png'; break;

                case 'SHOTSPOTTER':
                case 'SHOOTING':
                case 'SHOTS FIRED':
                  icon = 'red-circle.png'; break;

                // Orange: Non-violent serious crime
                case 'THEFT': 
                case 'DRUG DEALING':
                case 'OVERDOSE':
                case 'STOLEN VEHICLE':
                case 'ENTRY':
                case 'ENTRY TO AUTO':
                case 'PROPERTY DAMAGE':
                  icon ='orange-blank.png'; break;

                // Blue: Traffic
                case 'TRAFFIC STOP':
                  icon = 'blu-blank.png'; break;
              }

              this.markerCache.push({
                type: 'PoliceDispatch',
                id: i.CallNumber,
                position: {
                  lat: i.Geometry.coordinates[0][0][1],
                  lng: i.Geometry.coordinates[0][0][0]
                },
                status: i.Status,
                content: '<p style="font-size: 150%; font-weight: bold;">' + i.NatureOfCall + '</p>' +
                  i.Location + ' (Police District ' + i.District + ')<hr />' +
                  time + ' (' + fromNow + ')<br />' + 
                  '<b><i>' + i.Status + '</i></b>',
                icon: 'https://maps.google.com/mapfiles/kml/paddle/' + icon,
                marker: null,
                state: 'Hidden'
              })
            });

            this.markerCacheBounds = this.bounds;
            this.showMarkers();
          })
          .catch(error => {
            console.log(error);
          });
      }
    },
    loadFireDispatchMarkers: function () {
      let now = moment().subtract(this.hours, 'hours').format('YYYY-MM-DD HH:mm:ss');
      let filter = 'Disposition%20%3D%20%22ACTIVE%22%20and%20ReportedDateTime%20%3E%3D%20%22' + encodeURIComponent(now) + '%22' + this.getBoundsFilter();

      if (!this.areBoundsCached()) {
        this.showMarkers();
      } else {
        axios
          .get('/api/FireDispatchCall?offset=0&limit=' + this.mapItemLimit + '&order=ReportedDateTime%20desc&filter=' + filter)
          .then(response => {

            response.data.forEach(i => {
              if (!i.Geometry || !i.Geometry.coordinates || !i.Geometry.coordinates[0] || !i.Geometry.coordinates[0][0])
                return;

              if (this.markerCache.find(x => x.type == 'FireDispatch' && x.id == i.CFS))
                return;

              let time = moment(i.ReportedDateTime).format('llll');
              let fromNow = moment(i.ReportedDateTime).fromNow();

              // http://kml4earth.appspot.com/icons.html#paddle
              let icon = 'wht-blank.png';

              if (i.NatureOfCall == 'EMS')
                icon = 'orange-blank.png';
              else if (i.NatureOfCall.includes('Fire'))
                icon = 'red-blank.png';

              this.markerCache.push({
                type: 'FireDispatch',
                id: i.CFS,
                position: {
                  lat: i.Geometry.coordinates[0][0][1],
                  lng: i.Geometry.coordinates[0][0][0]
                },
                disposition: i.Disposition,
                content: '<p style="font-size: 150%; font-weight: bold;">' + i.NatureOfCall + '</p>' +
                  i.Address + (i.Apt ? ' APT. #' + i.Apt : '') + '<hr />' +
                  time + ' (' + fromNow + ')<br />' + 
                  '<b><i>' + i.Disposition + '</i></b>',
                icon: 'https://maps.google.com/mapfiles/kml/paddle/' + icon,
                marker: null,
                state: 'Hidden'
              })
            });

            this.markerCacheBounds = this.bounds;
            this.showMarkers();
          })
          .catch(error => {
            console.log(error);
          });
      }
    },
    areBoundsCached: function () {
      let cacheBounds = this.markerCacheBounds;
      let loadBounds = this.bounds;

      return cacheBounds == null ||
        loadBounds.ne.lng > cacheBounds.ne.lng ||
        loadBounds.sw.lng < cacheBounds.sw.lng ||
        loadBounds.ne.lat > cacheBounds.ne.lat ||
        loadBounds.sw.lat < cacheBounds.sw.lat;
    },
    showMarkers: function (filter) {
      if (!filter)
        filter = this.filter;

      if (!google)
        return;

      this.markerCache.forEach(markerDetail => {
        if (markerDetail.state == 'Visible')
          markerDetail.state = 'Hide';
      });

      let filteredCache;
      if (filter == 'ap') {
        filteredCache = this.markerCache.filter(x => x.type == 'PoliceDispatch' && x.status == 'Service in Progress');
      } else if (filter == 'rp') {
        filteredCache = this.markerCache.filter(x => x.type == 'PoliceDispatch');
      } else if (filter == 'af') {
        filteredCache = this.markerCache.filter(x => x.type == 'FireDispatch' && x.disposition == 'ACTIVE');
      } else if (filter == 'rf') {
        filteredCache = this.markerCache.filter(x => x.type == 'FireDispatch');
      } else {
        filteredCache = this.markerCache;
      }

      if (this.bounds)
        filteredCache = filteredCache.filter(x => 
          x.position.lat <= this.bounds.ne.lat &&
          x.position.lat >= this.bounds.sw.lat &&
          x.position.lng <= this.bounds.ne.lng &&
          x.position.lng >= this.bounds.sw.lng
        );

      filteredCache.forEach(markerDetail => {

        if (markerDetail.marker) {
          if (markerDetail.state == 'Hide') {
            markerDetail.state = 'Visible';
          } else if (markerDetail.state == 'Hidden') {
            markerDetail.state = 'Visible';
            markerDetail.marker.setMap(this.map);
          }

          return;
        }

        markerDetail.state = 'Visible';
        
        markerDetail.marker = new google.maps.Marker({
          position: markerDetail.position,
          icon: {
            url: markerDetail.icon,
            scaledSize: new google.maps.Size(50, 50),
          },
          map: this.map
        });

        markerDetail.marker.addListener('click', e => {
          if (this.openInfoWindow)
            this.openInfoWindow.close();

          this.openInfoWindow = new google.maps.InfoWindow({
            content: markerDetail.content
          });
          this.openInfoWindow.open(this.map, markerDetail.marker);
        });
      });

      // TODO: Set mapFull
      //this.mapFull = totalCount >= this.mapItemLimit;

      this.markerCache.forEach(markerDetail => {
        if (markerDetail.state == 'Hide') {
          markerDetail.state = 'Hidden';
          markerDetail.marker.setMap(null);
        }
      });

    },
    getBoundsFilter: function() {
      if (!this.bounds)
        return '';

      return '&northBound=' + this.bounds.ne.lat + '&southBound=' + this.bounds.sw.lat + '&eastBound=' + this.bounds.ne.lng + '&westBound=' + this.bounds.sw.lng;
    },
    distanceUpdated: function (value) {
      if (this.circle)
        this.circle.setMap(null);

      if (!this.locationData)
        return;

      this.circle = new google.maps.Circle({
        strokeColor: '#bd2130',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: '#0d2240',
        fillOpacity: 0.10,
        map: this.map,
        center: this.locationData,
        radius: (value * 0.3048) // Feet to meters
      });
    }
  },
  watch: {
    filter: function (newValue, oldValue) {
      this.showMarkers(newValue);
    },
    locationData: function (newValue, oldValue) {
      this.map.setCenter(newValue);
      this.map.setZoom(15);

      this.distanceUpdated(this.distance);
    },
    distance: function (newValue, oldValue) {
      this.distanceUpdated(newValue);
    }
  },
  async mounted () {

    this.google = await gmapsInit();
    this.map = new google.maps.Map(document.getElementById('basicMap'), {
      center: { lat: 43.0315528, lng: -87.9730566 },
      zoom: 12,
      gestureHandling: 'greedy'
    });

    let boundsChangedTimeout = null;

    this.map.addListener('bounds_changed', e => {

      if (boundsChangedTimeout != null)
        clearTimeout(boundsChangedTimeout);

      boundsChangedTimeout = setTimeout(() => {
        let bounds = this.map.getBounds();
        let ne = bounds.getNorthEast();
        let sw = bounds.getSouthWest();
        this.bounds = {
          ne: {
            lat: ne.lat(),
            lng: ne.lng()
          },
          sw: {
            lat: sw.lat(),
            lng: sw.lng()
          }
        };

        this.loadAllMarkers();

      }, 1000);
    });

    this.loadAllMarkers();
  }
};
</script>