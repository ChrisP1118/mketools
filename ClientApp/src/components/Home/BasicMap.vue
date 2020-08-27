<template>
  <div>
    <l-map ref="basicMap" style="height: 80vh; width: 100%" :zoom="zoom" :center="center">
      <l-tile-layer :url="tileUrl" :attribution="attribution"></l-tile-layer>
      <l-circle v-if="circleCenter" :lat-lng="circleCenter" :radius="circleRadius" color="#bd2130" />
      <l-marker v-for="marker in markers" v-bind:key="marker.id" :lat-lng="marker.position" :icon="marker.icon">
        <l-popup :content="marker.popup"></l-popup>
      </l-marker>
    </l-map>
  </div>
</template>

<script>
import axios from "axios";
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
      items: [],

      //tileUrl: 'https://{s}.tile.osm.org/{z}/{x}/{y}.png',
      //tileUrl: 'https://stamen-tiles-{s}.a.ssl.fastly.net/toner/{z}/{x}/{y}.png',
      tileUrl: 'https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png',
      zoom: 12,
      center: [43.0315528, -87.9730566],
      attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors',
      markers: [],
      circleCenter: null,
      circleRadius: null
    }
  },
  computed: {
    ...mapGetters(['getPoliceDispatchCallTypeIcon', 'getFireDispatchCallTypeIcon', 'getGeometryPosition']),
  },
  methods: {
    refreshMarkers: function () {
      Promise.all([this.$store.dispatch("loadPoliceDispatchCallTypes"), this.$store.dispatch("loadFireDispatchCallTypes")]).then(() => {
        this.loadMarkers();
      });
    },
    loadMarkers: function () {
      this.itemTypesLoaded = 0;

      let now = moment().subtract(this.hours, 'hours').format('YYYY-MM-DD HH:mm:ss');
      let filter = 'ReportedDateTime%20%3E%3D%20%22' + encodeURIComponent(now) + '%22';

      let promise1 = axios
        .get('/api/policeDispatchCall?limit=1000&filter=' + filter)
        .then(response => {
          let x = response.data.filter(i => i.geometry && this.getGeometryPosition(i.geometry));
          x.forEach(i => {
            if (this.items.some(x => x.id == i.callNumber))
              return;

            let icon = this.getPoliceDispatchCallTypeIcon(i.natureOfCall);

            this.items.push({
              type: 'PoliceDispatchCall',
              id: i.callNumber,
              position: this.getGeometryPosition(i.geometry),
              getContent: () => {
                let time = moment(i.reportedDateTime).format('llll');
                let fromNow = moment(i.reportedDateTime).fromNow();

                return '<p style="font-size: 150%; font-weight: bold;">' + i.natureOfCall + '</p>' +
                  i.location + ' (Police District ' + i.district + ')<hr />' +
                  time + ' (' + fromNow + ')<br />' + 
                  '<b><i>' + i.status + '</i></b>' +
                  '<hr />' +
                  '<p style="font-size: 125%;"><a href="#/policeDispatchCall/' + i.callNumber + '">Details</a></p>';
              },
              icon: icon
            });
          });
        });

      let promise2 = axios
        .get('/api/fireDispatchCall?limit=1000&filter=' + filter)
        .then(response => {
          let x = response.data.filter(i => i.geometry && this.getGeometryPosition(i.geometry));
          x.forEach(i => {
            if (this.items.some(x => x.id == i.cfs))
              return;

            let icon = this.getFireDispatchCallTypeIcon(i.natureOfCall);

            this.items.push({
              type: 'FireDispatchCall',
              id: i.cfs,
              position: this.getGeometryPosition(i.geometry),
              getContent: () => {
                let time = moment(i.reportedDateTime).format('llll');
                let fromNow = moment(i.reportedDateTime).fromNow();

                return '<p style="font-size: 150%; font-weight: bold;">' + i.natureOfCall + '</p>' +
                  i.address + (i.apt ? ' APT. #' + i.apt : '') + '<hr />' +
                  time + ' (' + fromNow + ')<br />' + 
                  '<b><i>' + i.disposition + '</i></b>' +
                  '<hr />' +
                  '<p style="font-size: 125%;"><a href="#/fireDispatchCall/' + i.cfs + '">Details</a></p>';
              },
              icon: icon
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
      this.markers = [];

      this.items.forEach(i => {

        let visible = true;

        if (this.filterType && this.filterType.length > 0)
          visible = i.type == this.filterType;

        if (visible) {
          this.markers.push({
            id: i.id,
            icon: L.icon({
              iconUrl: i.icon,
              iconSize: [40, 40],
              iconAnchor: [20, 0]
            }),
            position: i.position,
            popup: i.getContent()
          });
        }

      });
    },
    distanceUpdated: function (value) {
      if (!this.locationData)
        return;

      this.circleCenter = this.locationData;
      this.circleRadius = (value * 0.3048);
    }
  },
  watch: {
    filterType: function (newValue, oldValue) {
      this.showMarkers();
    },
    locationData: function (newValue, oldValue) {
      //this.center = newValue;
      //this.zoom = 15;

      if (newValue)
        this.$refs.basicMap.mapObject.setView(L.latLng(newValue.lat, newValue.lng), 15);
      else
        this.circleCenter = null;

      this.distanceUpdated(this.distance);
    },
    distance: function (newValue, oldValue) {
      this.distanceUpdated(newValue);
    }
  },
  created() {
  },
  mounted () {
    this.refreshMarkers();
  }
};
</script>