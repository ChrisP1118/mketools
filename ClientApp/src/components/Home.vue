<template>
  <div>
    <b-row v-if="showJumbotron">
      <b-col>
        <b-jumbotron class="text-center" header="MKE Alerts" lead="See and get notified of police calls and crimes in Milwaukee">
          <p>Enter an address below to get started.</p>
          <b-row>
            <b-col>
              <b-form @submit="onSubmit" @submit.stop.prevent>
                <b-form-row class="justify-content-center">
                  <b-form-group>
                    <label class="sr-only" for="Number">Number</label>
                    <b-form-input v-model="number" id="Number" placeholder="Number" type="number" />
                  </b-form-group>
                  <b-form-group>
                    <label class="sr-only" for="Direction">Direction</label>
                    <b-form-select v-model="streetDirection" id="Direction" :options="streetDirections" />
                  </b-form-group>
                  <b-form-group>
                    <label class="sr-only" for="Street">Street</label>
                    <b-form-select v-model="streetName" id="Street" :options="streetNames" />
                  </b-form-group>
                  <b-form-group>
                    <label class="sr-only" for="StreetType">Street Type</label>
                    <b-form-select v-model="streetType" id="StreetType" :options="streetTypes" />
                  </b-form-group>
                  <b-form-group>
                    <b-button type="submit" variant="primary">Go</b-button>
                  </b-form-group>
                </b-form-row>
              </b-form>
            </b-col>
          </b-row>
          <p><i>Remember! An increased awareness of crime does not mean crime is actually increasing.</i></p>
        </b-jumbotron>
      </b-col>
    </b-row>
    <b-row v-if="!showJumbotron">
      <b-col>
        <b-card bg-variant="light">
          <b-card-text>
            <b-form @submit="onSubmit" @submit.stop.prevent>
              <b-form-row class="justify-content-center">
                <b-form-group>
                  <label class="sr-only" for="Number">Number</label>
                  <b-form-input v-model="number" id="Number" placeholder="Number" type="number" />
                </b-form-group>
                <b-form-group>
                  <label class="sr-only" for="Direction">Direction</label>
                  <b-form-select v-model="streetDirection" id="Direction" :options="streetDirections" />
                </b-form-group>
                <b-form-group>
                  <label class="sr-only" for="Street">Street</label>
                  <b-form-select v-model="streetName" id="Street" :options="streetNames" />
                </b-form-group>
                <b-form-group>
                  <label class="sr-only" for="StreetType">Street Type</label>
                  <b-form-select v-model="streetType" id="StreetType" :options="streetTypes" />
                </b-form-group>
                <b-form-group>
                  <b-button type="submit" variant="primary">Go</b-button>
                </b-form-group>
              </b-form-row>
            </b-form>
          </b-card-text>
        </b-card>        
      </b-col>
    </b-row>
    <b-row class="mt-2">
      <b-col xs="12" md="6">
        <div class="map" id="homeMap" />
      </b-col>
      <b-col xs="12" md="6">
        <b-card no-body>
          <b-tabs pills card @input="onTabChanged">
            <b-tab title="Active Calls" active>
              <b-card-text>Active Calls</b-card-text>
            </b-tab>
            <b-tab title="Recent Calls">
              <b-card-text>Recent Calls</b-card-text>
            </b-tab>
            <b-tab title="Recent Crimes">
              <b-card-text>Recent Calls</b-card-text>
            </b-tab>
          </b-tabs>
        </b-card>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import axios from "axios";
import gmapsInit from './Common/googlemaps';

export default {
  name: "Home",
  props: {},
  data() {
    return {
      showJumbotron: true,
      number: null,
      streetDirection: '',
      streetName: '',
      streetType: '',
      streetDirections: [],
      streetNames: [],
      streetTypes: [],
      google: null,
      map: null
    }
  },
  methods: {
    loadStreetReferences: function () {
      axios
        .get('/api/StreetReference')
        .then(response => {
          this.streetDirections = response.data.streetDirections.map(x => { return x == null ? "" : x; });
          this.streetNames = response.data.streetNames;
          this.streetTypes = response.data.streetTypes.map(x => { return x == null ? "" : x; });;
        })
        .catch(error => {
          console.log(error);
        });
    },
    onSubmit: function () {
      this.showJumbotron = false;

      axios
        .get('/api/Geocoding?value=' + this.number + ' ' + this.streetDirection + ' ' + this.streetName + ' ' + this.streetType)
        .then(response => {
          let location = {lat: response.data.Geometry.Centroid.Coordinate[1], lng: response.data.Geometry.Centroid.Coordinate[0]};

          this.map.setCenter(location);
          this.map.setZoom(15);
        })
        .catch(error => {
          console.log(error);
        });
    },
    onTabChanged: function(tabIndex) {
      console.log('Tab changed: ' + tabIndex);
    }
  },
  async mounted () {
    this.loadStreetReferences();
      this.google = await gmapsInit();
      this.map = new google.maps.Map(document.getElementById('homeMap'), {
        center: { lat: 43.0315528, lng: -87.9730566 },
        zoom: 10,
        gestureHandling: 'greedy'
      });
  }
};
</script>