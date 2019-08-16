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
          <p><i>Remember! An increased awareness of crime does not necessarily indicate an increase in crime.</i></p>
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
                  <b-button type="button" @click="onClear">Clear</b-button>
                </b-form-group>
              </b-form-row>
            </b-form>
          </b-card-text>
        </b-card>        
      </b-col>
    </b-row>
    <b-row class="mt-2" v-if="mapFull">
      <b-col>
        <b-alert variant="warning" show>Only the most recent items are displayed. Zoom in on the map to see more.</b-alert>
      </b-col>
    </b-row>
    <b-row class="mt-2">
      <b-col xs="12" md="3">
        <b-list-group>
          <b-list-group-item :active="tabIndex == 0" @click="() => { this.updateTabIndex(0); }">Active Calls</b-list-group-item>
          <b-list-group-item :active="tabIndex == 1" @click="() => { this.updateTabIndex(1); }">Recent Calls</b-list-group-item>
          <b-list-group-item>Recent Crimes</b-list-group-item>
          <b-list-group-item>Notifications</b-list-group-item>
        </b-list-group>
      </b-col>
      <b-col xs="12" md="9">
        <div class="map" id="homeMap" />
      </b-col>
    </b-row>
  </div>
</template>

<script>
import axios from "axios";
import gmapsInit from './Common/googlemaps';
import moment from 'moment'

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
      map: null,
      bounds: null,
      tabIndex: 0,
      markerWrappers: [],
      mapFull: false,
      mapItemLimit: 100
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
          this.map.setZoom(14);
        })
        .catch(error => {
          console.log(error);
        });
    },
    onClear: function () {
      this.showJumbotron = true;
      this.number = null;
      this.streetDirection = '';
      this.streetName = '';
      this.streetType = '';
    },
    updateTabIndex: function (newIndex) {
      this.tabIndex = newIndex;
      this.updateTab();
    },
    updateTab: function () {
      if (this.tabIndex == 0)
        this.loadActiveCalls();
      else if (this.tabIndex == 1)
        this.loadRecentCalls();
    },
    loadActiveCalls: function () {
      let now = moment().subtract(6, 'hours').format('YYYY-MM-DD HH:mm:ss');
      this.loadDispatchCalls('Status%20%3D%20%22Service%20in%20Progress%22%20and%20ReportedDateTime%20%3E%3D%20%22' + encodeURIComponent(now) + '%22' + this.getBoundsFilter())
    },
    loadRecentCalls: function () {
      let now = moment().subtract(2, 'hours').format('YYYY-MM-DD HH:mm:ss');
      this.loadDispatchCalls('ReportedDateTime%20%3E%3D%20%22' + encodeURIComponent(now) + '%22' + this.getBoundsFilter())
    },
    loadDispatchCalls: function (filter) {
      axios
        .get('/api/DispatchCall?offset=0&limit=' + this.mapItemLimit + '&order=ReportedDateTime%20desc&filter=' + filter)
        .then(response => {
          let totalCount = response.headers['x-total-count'];
          this.mapFull = totalCount >= this.mapItemLimit;
          this.drawMarkers(response.data, 
            i => { return i.Geometry; }, 
            i => { return i.CallNumber; }, 
            i => { 
              let time = moment(i.ReportedDateTime).format('llll');
              let fromNow = moment(i.ReportedDateTime).fromNow();
              return '' + 
                '<p style="font-size: 150%; font-weight: bold;">' + i.NatureOfCall + '</p>' +
                i.Location + ' (Police District ' + i.District + ')<hr />' +
                time + ' (' + fromNow + ')<br />' + 
                '<b><i>' + i.Status + '</i></b>';
            },
            i => {
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
              return 'https://maps.google.com/mapfiles/kml/paddle/' + icon;
            });
        })
        .catch(error => {
          console.log(error);
        });
    },
    drawMarkers: function (items, getItemMarkerGeometry, getItemId, getItemInfoWindowText, getMarkerIcon) {
      if (!google)
        return;

      let newMarkerWrappers = [];

      items.forEach(i => {

        let geometry = getItemMarkerGeometry(i);

        if (!geometry)
          return;

        let existingMarkerWrapper = this.markerWrappers.find(w => w.id == getItemId(i));
        if (existingMarkerWrapper) {
          newMarkerWrappers.push(existingMarkerWrapper);
          return;
        } else {

          let point = geometry.coordinates[0][0];

          let marker = new google.maps.Marker({
            position: {
              lat: point[1],
              lng: point[0]
            },
            icon: {
              url: getMarkerIcon(i),
              scaledSize: new google.maps.Size(50, 50),
            },
            map: this.map
          });

          marker.addListener('click', e => {
            if (this.openInfoWindow)
              this.openInfoWindow.close();

            this.openInfoWindow = new google.maps.InfoWindow({
              content: getItemInfoWindowText(i)
            });
            this.openInfoWindow.open(this.map, marker);
          });

          newMarkerWrappers.push({
            id: getItemId(i),
            marker: marker
          });
        }
      });

      let oldMarkerWrappers = this.markerWrappers.filter(m => { return !(newMarkerWrappers.find(x => x.id == m.id)); });
      oldMarkerWrappers.forEach(w => { 
        w.marker.setMap(null);
      });
      this.markerWrappers = newMarkerWrappers;
    },
    getBoundsFilter: function() {
      if (!this.bounds)
        return '';

      return '&northBound=' + this.bounds.ne.lat + '&southBound=' + this.bounds.sw.lat + '&eastBound=' + this.bounds.ne.lng + '&westBound=' + this.bounds.sw.lng;
    }
  },
  async mounted () {
    this.loadStreetReferences();

    this.google = await gmapsInit();
    this.map = new google.maps.Map(document.getElementById('homeMap'), {
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
        this.bounds = {
          ne: {
            lat: bounds.na.j,
            lng: bounds.ga.j
          },
          sw: {
            lat: bounds.na.l,
            lng: bounds.ga.l
          }
        };

        this.updateTab();

      }, 1000);
    });      
  }
};
</script>