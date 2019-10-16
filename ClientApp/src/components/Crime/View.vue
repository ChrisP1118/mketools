<template>
  <div>
    <page-loading v-if="!crime" />
    <page-title v-if="crime" :title="pageTitle" />
    <b-link to="/crime">All Crimes</b-link>
    <div v-if="crime">
      <b-row>
        <b-col xs="12" md="6">
          <b-card class="mt-3">
            <h4 slot="header">Basic</h4>
            <ul>
              <li>Incident Number: {{crime.incidentNum}}</li>
              <li>Reported Date/Time: {{reportedDateTime}} ({{relativeTime}})</li>
            </ul>            
          </b-card>
          <b-card class="mt-3">
            <h4 slot="header">Nature Of Crime</h4>
            <ul>
              <li v-if="crime.arson">Arson</li>
              <li v-if="crime.assaultOffense">Assault Offense</li>
              <li v-if="crime.burglary">Burglary</li>
              <li v-if="crime.criminalDamage">Criminal Damage</li>
              <li v-if="crime.homicide">Homicide</li>
              <li v-if="crime.lockedVehicle">Locked Vehicle</li>
              <li v-if="crime.robbery">Robbery</li>
              <li v-if="crime.sexOffense">Sex Offense</li>
              <li v-if="crime.theft">Theft</li>
              <li v-if="crime.vehicleTheft">Vehicle Theft</li>
              <li v-if="crime.weaponUsed">{{crime.weaponUsed}}</li>
            </ul>
          </b-card>
        </b-col>
        <b-col xs="12" md="6">
          <b-card class="mt-3">
            <h4 slot="header">Location</h4>
            <ul>
              <li>Location: {{crime.location}}</li>
              <li>Police District: {{crime.police}}</li>
              <li>Ward: {{crime.ward}}</li>
              <li>Aldermanic District: {{crime.ald}}</li>
              <li>ZIP Code: {{crime.zip}}</li>
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
  name: "CrimeView",
  props: ['id'],
  data() {
    return {
      crime: null,
      relativeTime: null,
      position: null,
      reportedDateTime: null,
      relativeTime: null
    };
  },
  computed: {
    pageTitle: function () {
      return 'Crime: ' + this.id;
    }
  },
  methods: {
    load: function () {
      axios
        .get('/api/crime/' + this.id)
        .then(response => {
          console.log(response);

          this.crime = response.data;

          this.position = {
            lat: this.crime.point.coordinates[1], 
            lng: this.crime.point.coordinates[0]
          };

          this.reportedDateTime = moment(this.crime.reportedDateTime).format('llll');
          this.relativeTime = moment(this.crime.reportedDateTime).fromNow();
        })
        .catch(error => {
          console.log(error);
        });
    },
    onClose(evt) {
      this.$router.push('/crime');
    }
  },
  async mounted () {
    this.google = await gmapsInit();
    this.load();
  }
};
</script>