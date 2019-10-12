<template>
  <div>
    <page-loading v-if="!policeDispatchCall" />
    <page-title v-if="policeDispatchCall" :title="pageTitle" />
    <b-link to="/policeDispatchCall">All Police Dispatch Calls</b-link>
    <div v-if="policeDispatchCall">
      <b-row>
        <b-col xs="12" md="6">
          <b-card class="mt-3">
            <h4 slot="header">Basic</h4>
            <ul>
              <li>Call Number: {{policeDispatchCall.callNumber}}</li>
              <li>Reported Date/Time: {{policeDispatchCall.reportedDateTime}} ({{relativeTime}})</li>
            </ul>            
          </b-card>
          <b-card class="mt-3">
            <h4 slot="header">Nature Of Call</h4>
            <ul>
              <li>Nature of Call: {{policeDispatchCall.natureOfCall}}</li>
              <li v-if="policeDispatchCall.isMajor">Type: Major Crime</li>
              <li v-if="policeDispatchCall.isMinor">Type: Minor Crime</li>
              <li v-if="!policeDispatchCall.isMajor && !policeDispatchCall.isMajor">Type: Non-Crime</li>
              <li v-if="policeDispatchCall.isCritical">Category: Critical</li>
              <li v-if="policeDispatchCall.isViolent">Category: Violent</li>
              <li v-if="policeDispatchCall.isProperty">Category: Property</li>
              <li v-if="policeDispatchCall.isDrug">Category: Drug</li>
              <li v-if="policeDispatchCall.isTraffic">Category: Traffic</li>
              <li v-if="policeDispatchCall.isOtherCrime">Category: Other</li>
            </ul>
            <p class="small">Does this look incorrect? The way calls are categorized is fairly arbitrary and inexact. <b-link to="/contact">Contact us</b-link> if you think you could help improve this process.</p>
          </b-card>
        </b-col>
        <b-col xs="12" md="6">
          <b-card class="mt-3">
            <h4 slot="header">Location</h4>
            <ul>
              <li>Location: {{policeDispatchCall.location}}</li>
              <li>Police District: {{policeDispatchCall.district}}</li>
            </ul>
            <p>... area map ...</p>
            <div id="map" class="map">
            </div>
            <!-- <div id="streetView" class="map">
            </div> -->
          </b-card>
        </b-col>
      </b-row>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import moment from 'moment'
import gmapsInit from '../Common/googlemaps';

export default {
  name: "PoliceDispatchCallView",
  props: ['id'],
  data() {
    return {
      policeDispatchCall: null,
      policeDispatchCallType : null,
      relativeTime: null,
      google: null,
      map: null,
      marker: null,
      //streetView: null
    };
  },
  computed: {
    pageTitle: function () {
      return 'Police Dispatch Call: ' + this.id;
    }
  },
  methods: {
    load: function () {
      axios
        .get('/api/policeDispatchCall/' + this.id)
        .then(response => {
          console.log(response);

          this.policeDispatchCall = response.data;

          let time = moment(this.policeDispatchCall.reportedDateTime).format('llll');
          this.relativeTime = moment(this.policeDispatchCall.reportedDateTime).fromNow();

          this.loadPoliceDispatchCallType();
          this.loadMap();
        })
        .catch(error => {
          console.log(error);
        });
    },
    loadPoliceDispatchCallType: function() {
      axios
        .get('/api/policeDispatchCallType/' + this.policeDispatchCall.natureOfCall)
        .then(response => {
          this.policeDispatchCallType = response.data;
        })
        .catch(error => {
          console.log(error);
        });
    },
    loadMap: function () {
      // This sure seems like a hack -- but the HTML element doesn't seem to exist initially
      if (!document.getElementById('map')) {
        setTimeout(this.loadMap, 100);
        return;
      }

      let p = {
        lat: this.policeDispatchCall.geometry.coordinates[0][0][1], 
        lng: this.policeDispatchCall.geometry.coordinates[0][0][0]
      };

      this.map = new google.maps.Map(
        document.getElementById('map'), 
        {
          center: p,
          zoom: 18,
        });

      this.marker = new google.maps.Marker({
        position: p,
        map: this.map
      });

      // this.streetView = new google.maps.StreetViewPanorama(
      //   document.getElementById('streetView'),
      //   {
      //     position: p,
      //     pov: {heading: 165, pitch: 0},
      //     zoom: 1
      //   });
    },
    onClose(evt) {
      this.$router.push('/policeDispatchCall');
    }
  },
  async mounted () {
    this.google = await gmapsInit();
    this.load();
  }
};
</script>