<template>
  <div>
    <div class="map" id="basicMap" />
  </div>
</template>

<script>
import axios from "axios";
import gmapsInit from '../Common/googlemaps';
import moment from 'moment'
import { mapState, mapGetters } from 'vuex'

export default {
  name: "BasicMap",
  mixins: [],
  props: {
    hours: {
      type: Number,
      default: 6
    },
    refreshSeconds: {
      type: Number,
      default: 300
    },
    filterType: {
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
      items: [],
      circle: null
    }
  },
  computed: {
    ...mapGetters(['getPoliceDispatchCallTypeIcon', 'getFireDispatchCallTypeIcon']),
  },
  methods: {
    loadMarkers: function () {
      this.itemTypesLoaded = 0;

      let now = moment().subtract(this.hours, 'hours').format('YYYY-MM-DD HH:mm:ss');
      let filter = 'ReportedDateTime%20%3E%3D%20%22' + encodeURIComponent(now) + '%22';

      let promise1 = axios
        .get('/api/policeDispatchCall?limit=1000&filter=' + filter)
        .then(response => {
          let x = response.data.filter(i => i.geometry && i.geometry.coordinates && i.geometry.coordinates[0] && i.geometry.coordinates[0][0]);
          x.forEach(i => {
            if (this.items.some(x => x.id == i.callNumber))
              return;

            let time = moment(i.reportedDateTime).format('llll');
            let fromNow = moment(i.reportedDateTime).fromNow();

            let icon = this.getPoliceDispatchCallTypeIcon(i.natureOfCall);

            this.items.push({
              type: 'PoliceDispatchCall',
              id: i.callNumber,
              position: {
                lat: i.geometry.coordinates[0][0][1],
                lng: i.geometry.coordinates[0][0][0]
              },
              status: i.status,
              content: '<p style="font-size: 150%; font-weight: bold;">' + i.natureOfCall + '</p>' +
                i.location + ' (Police District ' + i.district + ')<hr />' +
                time + ' (' + fromNow + ')<br />' + 
                '<b><i>' + i.status + '</i></b>',
              icon: 'https://maps.google.com/mapfiles/kml/paddle/' + icon,
              marker: null
            });
          });
        });

      let promise2 = axios
        .get('/api/fireDispatchCall?limit=1000&filter=' + filter)
        .then(response => {
          let x = response.data.filter(i => i.geometry && i.geometry.coordinates && i.geometry.coordinates[0] && i.geometry.coordinates[0][0]);
          x.forEach(i => {
            if (this.items.some(x => x.id == i.cfs))
              return;

            let time = moment(i.reportedDateTime).format('llll');
            let fromNow = moment(i.reportedDateTime).fromNow();

            let icon = this.getFireDispatchCallTypeIcon(i.natureOfCall);

            this.items.push({
              type: 'FireDispatchCall',
              id: i.cfs,
              position: {
                lat: i.geometry.coordinates[0][0][1],
                lng: i.geometry.coordinates[0][0][0]
              },
              disposition: i.disposition,
              content: '<p style="font-size: 150%; font-weight: bold;">' + i.natureOfCall + '</p>' +
                i.address + (i.apt ? ' APT. #' + i.apt : '') + '<hr />' +
                time + ' (' + fromNow + ')<br />' + 
                '<b><i>' + i.disposition + '</i></b>',
              icon: 'https://maps.google.com/mapfiles/kml/paddle/' + icon,
              marker: null
            });
          });
        });

        Promise.all([promise1, promise2]).then(() => {
          this.showMarkers();
          this.$emit('updated', moment().format('LT'));
          setTimeout(this.loadMarkers, this.refreshSeconds * 1000);
        });
    },
    showMarkers: function () {
      if (!google)
        return;

      if (!this.map)
        return;

      this.items.forEach(i => {

        let visible = true;

        if (this.filterType && this.filterType.length > 0)
          visible = i.type == this.filterType;

        if (visible) {
          if (!i.marker) {
            i.marker = new google.maps.Marker({
              position: i.position,
              icon: {
                url: i.icon,
                scaledSize: new google.maps.Size(50, 50),
              },
              map: this.map
            });

            i.marker.addListener('click', e => {
              if (this.openInfoWindow)
                this.openInfoWindow.close();

              this.openInfoWindow = new google.maps.InfoWindow({
                content: i.content
              });
              this.openInfoWindow.open(this.map, i.marker);
            });
          } else {
            if (i.marker.map == null)
              i.marker.setMap(this.map);
          }
        } else {
          if (i.marker && i.marker.map)
            i.marker.setMap(null)
        }
      });
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
    filterType: function (newValue, oldValue) {
      this.showMarkers();
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
  created() {
  },
  async mounted () {

    this.google = await gmapsInit();
    this.map = new google.maps.Map(document.getElementById('basicMap'), {
      center: { lat: 43.0315528, lng: -87.9730566 },
      zoom: 12,
      //gestureHandling: 'greedy'
    });

    google.maps.event.addListenerOnce(this.map, 'idle', () => {
      this.showMarkers();
    });

    Promise.all([this.$store.dispatch("loadPoliceDispatchCallTypes"), this.$store.dispatch("loadFireDispatchCallTypes")]).then(() => {
      this.loadMarkers();
    });

  }
};
</script>