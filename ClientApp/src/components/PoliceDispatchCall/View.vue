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
              <li>Reported Date/Time: {{reportedDateTime}} ({{relativeTime}})</li>
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
            <nearby-map :position="position"></nearby-map>
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
      reportedDateTime: null,
      relativeTime: null,
      position: null
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
          this.position = this.$store.getters.getGeometryPosition(this.policeDispatchCall.geometry);

          this.reportedDateTime = moment(this.policeDispatchCall.reportedDateTime).format('llll');
          this.relativeTime = moment(this.policeDispatchCall.reportedDateTime).fromNow();
          
          this.loadPoliceDispatchCallType();
        })
        .catch(error => {
          console.log(error);
        });
    },
    loadPoliceDispatchCallType: function() {
      axios
        .get('/api/policeDispatchCallType/' + encodeURIComponent(this.policeDispatchCall.natureOfCall))
        .then(response => {
          this.policeDispatchCallType = response.data;
        })
        .catch(error => {
          console.log(error);
        });
    },
    onClose(evt) {
      this.$router.push('/policeDispatchCall');
    }
  },
  async mounted () {
    this.load();
  }
};
</script>