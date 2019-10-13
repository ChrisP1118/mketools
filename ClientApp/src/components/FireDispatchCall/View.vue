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
              <li>Reported Date/Time: {{fireDispatchCall.reportedDateTime}} ({{relativeTime}})</li>
            </ul>            
          </b-card>
          <b-card class="mt-3">
            <h4 slot="header">Nature Of Call</h4>
            <ul>
              <li>Nature of Call: {{fireDispatchCall.natureOfCall}}</li>
              <li v-if="fireDispatchCall.isMajor">Type: Major</li>
              <li v-if="!fireDispatchCall.isMajor">Type: Minor</li>
            </ul>
            <p class="small">Does this look incorrect? The way calls are categorized is fairly arbitrary and inexact. <b-link to="/contact">Contact us</b-link> if you think you could help improve this process.</p>
          </b-card>
        </b-col>
        <b-col xs="12" md="6">
          <b-card class="mt-3">
            <h4 slot="header">Location</h4>
            <ul>
              <li>Location: {{fireDispatchCall.address}}</li>
            </ul>
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
  name: "FireDispatchCallView",
  props: ['id'],
  data() {
    return {
      fireDispatchCall: null,
      fireDispatchCallType : null,
      properties: null,
      relativeTime: null,
      google: null,
      map: null,
      marker: null,
      position: null,
      propertiesShowing: false
      //streetView: null
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

          this.position = {
            lat: this.fireDispatchCall.geometry.coordinates[0][0][1], 
            lng: this.fireDispatchCall.geometry.coordinates[0][0][0]
          };

          let time = moment(this.fireDispatchCall.reportedDateTime).format('llll');
          this.relativeTime = moment(this.fireDispatchCall.reportedDateTime).fromNow();

          this.loadFireDispatchCallType();
          this.loadMap();
          this.loadProperties();
        })
        .catch(error => {
          console.log(error);
        });
    },
    loadFireDispatchCallType: function() {
      axios
        .get('/api/fireDispatchCallType/' + this.fireDispatchCall.natureOfCall)
        .then(response => {
          this.fireDispatchCallType = response.data;
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

      this.map = new google.maps.Map(
        document.getElementById('map'), 
        {
          center: this.position,
          zoom: 18,
        });

      this.marker = new google.maps.Marker({
        position: this.position,
        map: this.map
      });

      this.showProperties();

      // this.streetView = new google.maps.StreetViewPanorama(
      //   document.getElementById('streetView'),
      //   {
      //     position: p,
      //     pov: {heading: 165, pitch: 0},
      //     zoom: 1
      //   });
    },
    loadProperties: function () {
      let latDiff = 0.0006;
      let lngDiff = 0.0006;

      axios
        .get('/api/property?limit=50&northBound=' + (this.position.lat + latDiff) + '&southBound=' + (this.position.lat - latDiff) + '&eastBound=' + (this.position.lng + lngDiff) + '&westBound=' + (this.position.lng - lngDiff))
        .then(response => {
          this.properties = response.data;
          this.showProperties();
        })
        .catch(error => {
          console.log(error);
        });
    },
    showProperties: function () {
      if (this.propertiesShowing)
        return;

      if (!this.map)
        return;

      if (!this.properties)
        return;

      this.properties.forEach(p => {
        let coords = [];

        let x = p.parcel.outline.coordinates[0].forEach(y => {
          coords.push({
            lat: y[1],
            lng: y[0]
          });
        });

        let polygon = new google.maps.Polygon({
          paths: coords,
          strokeColor: '#FF0000',
          strokeOpacity: 0.8,
          strokeWeight: 2,
          fillColor: '#FF0000',
          fillOpacity: 0.35
        });
        polygon.setMap(this.map);

        polygon.addListener('click', e => {
          if (this.openInfoWindow)
            this.openInfoWindow.close();

          let address = p.house_nr_lo;
          if (p.house_nr_hi != p.house_nr_lo)
            address += '-' + p.house_nr_hi;
          address += ' ' + p.sdir + ' ' + p.street + ' ' + p.sttype;

          let owner = p.owner_name_1;
          if (p.owner_name_2)
            owner += '<br />' + p.owner_name_2;
          if (p.owner_name_3)
            owner += '<br />' + p.owner_name_3;
          owner += '<br />' + p.owner_mail_addr + '<br />' + p.owner_city_state;
            
          this.openInfoWindow = new google.maps.InfoWindow({
            content: '<h4>' + address + '</h4>' +
              owner
          });
          this.openInfoWindow.setPosition(e.latLng);
          this.openInfoWindow.open(this.map);
        });          
      });

      this.propertiesShowing = true;
    },
    onClose(evt) {
      this.$router.push('/fireDispatchCall');
    }
  },
  async mounted () {
    this.google = await gmapsInit();
    this.load();
  }
};
</script>