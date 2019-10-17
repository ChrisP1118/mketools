<template>
  <div>
    <page-loading v-if="!fireDispatchCall" />
    <page-title v-if="fireDispatchCall" :title="pageTitle" />
    <b-link to="/fireDispatchCall">All Fire Dispatch Calls</b-link>
    <div v-if="fireDispatchCall">
      <b-row>
        <b-col xs="12" md="6">
          <b-card class="mt-3">
            <h4 slot="header">Basic</h4>
            <ul>
              <li>Call Number (CFS): {{fireDispatchCall.cfs}}</li>
              <li>Reported Date/Time: {{reportedDateTime}} ({{relativeTime}})</li>
            </ul>            
          </b-card>
          <b-card class="mt-3">
            <h4 slot="header">Nature Of Call</h4>
            <ul>
              <li>Nature of Call: {{fireDispatchCall.natureOfCall}}</li>
              <li v-if="fireDispatchCall.isMajor">Type: Major</li>
              <li v-if="!fireDispatchCall.isMajor">Type: Minor</li>
            </ul>
            <p class="small">Does this look incorrect? The way calls are categorized is fairly arbitrary and inexact. <b-link to="/support">Contact us if you think you could help improve this process.</b-link></p>
          </b-card>
        </b-col>
        <b-col xs="12" md="6">
          <b-card class="mt-3">
            <h4 slot="header">Location</h4>
            <ul>
              <li>Location: {{fireDispatchCall.address}}</li>
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

export default {
  name: "FireDispatchCallView",
  props: ['id'],
  data() {
    return {
      fireDispatchCall: null,
      fireDispatchCallType : null,
      reportedDateTime: null,
      relativeTime: null,
      position: null
    };
  },
  computed: {
    pageTitle: function () {
      return 'Fire Dispatch Call: ' + this.id;
    }
  },
  methods: {
    load: function () {
      axios
        .get('/api/fireDispatchCall/' + this.id)
        .then(response => {
          console.log(response);

          this.fireDispatchCall = response.data;
          this.position = this.$store.getters.getGeometryPosition(this.fireDispatchCall.geometry);

          this.reportedDateTime = moment(this.fireDispatchCall.reportedDateTime).format('llll');
          this.relativeTime = moment(this.fireDispatchCall.reportedDateTime).fromNow();

          this.loadFireDispatchCallType();
        })
        .catch(error => {
          console.log(error);
        });
    },
    loadFireDispatchCallType: function() {
      axios
        .get('/api/fireDispatchCallType/' + encodeURIComponent(this.fireDispatchCall.natureOfCall))
        .then(response => {
          this.fireDispatchCallType = response.data;
        })
        .catch(error => {
          console.log(error);
        });
    },
    onClose(evt) {
      this.$router.push('/fireDispatchCall');
    }
  },
  async mounted () {
    this.load();
  }
};
</script>