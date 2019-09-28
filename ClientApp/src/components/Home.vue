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
    <b-row class="lg-2" v-if="mapFull">
      <b-col>
        <b-alert variant="warning" show>Only the most recent items are displayed. Zoom in on the map to see more.</b-alert>
      </b-col>
    </b-row>
    <b-row class="lg-2">
      <b-col xs="12" lg="3">
        <b-list-group>
          <b-list-group-item :active="tabKey == 'a'" @click="() => { this.updateTabKey('a'); }">Active Calls</b-list-group-item>
          <b-list-group-item :active="tabKey == 'ap'" @click="() => { this.updateTabKey('ap'); }">Active Police Calls</b-list-group-item>
          <b-list-group-item :active="tabKey == 'rp'" @click="() => { this.updateTabKey('rp'); }">Recent Police Calls</b-list-group-item>
          <b-list-group-item :active="tabKey == 'af'" @click="() => { this.updateTabKey('af'); }">Active Fire Calls</b-list-group-item>
          <b-list-group-item :active="tabKey == 'rf'" @click="() => { this.updateTabKey('rf'); }">Recent Fire Calls</b-list-group-item>
        </b-list-group>
        <hr class="mt-3" />
        <b-form class="mt-3">
          <h2>Get Notifications</h2>
          <p>Sign up to get an email notification whenever there's a call in your area.</p>
          <b-form-group>
            <label class="sr-only" for="EmailAddress">Email Address</label>
            <b-form-input v-model="emailAddress" id="EmailAddress" placeholder="Email Address" type="email" />
          </b-form-group>
          <b-form-group>
            <label class="sr-only" for="Password">Password</label>
            <b-form-input v-model="password" id="Password" placeholder="Password" type="password" />
          </b-form-group>
          <b-form-group>
            <label class="sr-only" for="Confirm">Confirm Password</label>
            <b-form-input v-model="confirmPassword" id="Confirm" placeholder="Confirm Password" type="password" />
          </b-form-group>
          <b-button>
            <g-signin-button :params="googleSignInParams" @success="onGoogleSignInSuccess" @error="onGoogleSignInError">Sign in with Google</g-signin-button>
          </b-button>
          <b-button>
            <fb-signin-button :params="fbSignInParams" @success="onFacebookSignInSuccess" @error="onFacebookSignInError">Sign in with Facebook</fb-signin-button>
          </b-button>
        </b-form>
      </b-col>
      <b-col xs="12" lg="9">
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

      // Address lookup
      number: null,
      streetDirection: '',
      streetName: '',
      streetType: '',
      streetDirections: [],
      streetNames: [],
      streetTypes: [],

      // Mapping
      google: null,
      map: null,
      bounds: null,
      tabKey: 'a',
      visibleMarkerCaches: ['ap', 'af'],
      markerCache: {
        ap: [],
        rp: [],
        af: [],
        rf: []
      },
      markerCacheBounds: {
        ap: null,
        rp: null,
        af: null,
        rf: null
      },
      markerWrappers: [],
      mapFull: false,
      mapItemLimit: 100,

      // Notifications
      emailAddress: '',
      password: '',
      confirmPassword: '',

      googleSignInParams: {
        client_id: '66835382455-403e538rnmmpmcp5tocmndleh30g4i5d.apps.googleusercontent.com'
      },
      fbSignInParams: {
        scope: 'email,user_likes',
        return_scopes: true
      }
    }
  },
  methods: {
    onGoogleSignInSuccess (googleUser) {
      // `googleUser` is the GoogleUser object that represents the just-signed-in user.
      // See https://developers.google.com/identity/sign-in/web/reference#users
      const profile = googleUser.getBasicProfile();
      console.log('ID: ' + profile.getId());
      console.log('Full Name: ' + profile.getName());
      console.log('Given Name: ' + profile.getGivenName());
      console.log('Family Name: ' + profile.getFamilyName());
      console.log('Image URL: ' + profile.getImageUrl());
      console.log('Email: ' + profile.getEmail());

      // The ID token you need to pass to your backend:
      var id_token = googleUser.getAuthResponse().id_token;
      console.log("ID Token: " + id_token);      
    },
    onGoogleSignInError (error) {
      // `error` contains any error occurred.
      console.log('OH NOES', error);
    },
    onFacebookSignInSuccess (response) {
      FB.api('/me?fields=name,email', user => {
        console.log('ID: ' + user.id);
        console.log('Name: ' + user.name);
        console.log('Email: ' + user.email);
        console.log(user);
      });
    },
    onFacebookSignInError (error) {
      console.log('OH NOES', error);
    },    
    getPosition: function () {
      if (!("geolocation" in navigator))
        return;

      navigator.geolocation.getCurrentPosition(this.gotPosition);
    },
    gotPosition: function (position) {
      console.log(position);

      let location = {
        lat: position.coords.latitude,
        lng: position.coords.longitude
      };

      this.map.setCenter(location);
      this.map.setZoom(14);
    },
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
    updateTabKey: function (newKey) {
      this.tabKey = newKey;
      this.updateTab();
    },
    updateTab: function () {
      if (this.tabKey == 'ap') {
        this.visibleMarkerCaches = ['ap']
        this.loadActivePoliceCalls();
      } else if (this.tabKey == 'rp') {
        this.visibleMarkerCaches = ['rp']
        this.loadRecentPoliceCalls();
      } else if (this.tabKey == 'af') {
        this.visibleMarkerCaches = ['af']
        this.loadActiveFireCalls();
      } else if (this.tabKey == 'rf') {
        this.visibleMarkerCaches = ['rf']
        this.loadRecentFireCalls();
      } else if (this.tabKey == 'a') {
        this.visibleMarkerCaches = ['ap', 'af']
        this.loadActivePoliceCalls();
        this.loadActiveFireCalls();
      }
    },
    loadActivePoliceCalls: function () {
      let now = moment().subtract(6, 'hours').format('YYYY-MM-DD HH:mm:ss');
      this.loadPoliceDispatchCalls('ap', 'Status%20%3D%20%22Service%20in%20Progress%22%20and%20ReportedDateTime%20%3E%3D%20%22' + encodeURIComponent(now) + '%22' + this.getBoundsFilter())
    },
    loadRecentPoliceCalls: function () {
      let now = moment().subtract(2, 'hours').format('YYYY-MM-DD HH:mm:ss');
      this.loadPoliceDispatchCalls('rp', 'ReportedDateTime%20%3E%3D%20%22' + encodeURIComponent(now) + '%22' + this.getBoundsFilter())
    },
    loadPoliceDispatchCalls: function (cacheKey, filter) {
      if (!this.areBoundsCached(cacheKey)) {
        this.showMarkers();
      } else {
        axios
          .get('/api/PoliceDispatchCall?offset=0&limit=' + this.mapItemLimit + '&order=ReportedDateTime%20desc&filter=' + filter)
          .then(response => {
            response.data.forEach(i => {
              if (this.markerCache[cacheKey].find(x => x.id == i.CallNumber))
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

              this.markerCache[cacheKey].push({
                id: i.CallNumber,
                geometry: i.Geometry,
                content: '<p style="font-size: 150%; font-weight: bold;">' + i.NatureOfCall + '</p>' +
                  i.Location + ' (Police District ' + i.District + ')<hr />' +
                  time + ' (' + fromNow + ')<br />' + 
                  '<b><i>' + i.Status + '</i></b>',
                icon: 'https://maps.google.com/mapfiles/kml/paddle/' + icon,
                marker: null,
                state: 'Hidden'
              })
            });

            this.markerCacheBounds[cacheKey] = this.bounds;
            this.showMarkers();
          })
          .catch(error => {
            console.log(error);
          });
      }
    },
    loadActiveFireCalls: function () {
      let now = moment().subtract(6, 'hours').format('YYYY-MM-DD HH:mm:ss');
      this.loadFireDispatchCalls('af', 'Disposition%20%3D%20%22ACTIVE%22%20and%20ReportedDateTime%20%3E%3D%20%22' + encodeURIComponent(now) + '%22' + this.getBoundsFilter())
    },
    loadRecentFireCalls: function () {
      let now = moment().subtract(2, 'hours').format('YYYY-MM-DD HH:mm:ss');
      this.loadFireDispatchCalls('rf', 'ReportedDateTime%20%3E%3D%20%22' + encodeURIComponent(now) + '%22' + this.getBoundsFilter())
    },
    loadFireDispatchCalls: function (cacheKey, filter) {
      if (!this.areBoundsCached(cacheKey)) {
        this.showMarkers();
      } else {
        axios
          .get('/api/FireDispatchCall?offset=0&limit=' + this.mapItemLimit + '&order=ReportedDateTime%20desc&filter=' + filter)
          .then(response => {

            response.data.forEach(i => {
              if (this.markerCache[cacheKey].find(x => x.id == i.CFS))
                return;

              let time = moment(i.ReportedDateTime).format('llll');
              let fromNow = moment(i.ReportedDateTime).fromNow();

              // http://kml4earth.appspot.com/icons.html#paddle
              let icon = 'wht-blank.png';

              if (i.NatureOfCall == 'EMS')
                icon = 'orange-blank.png';
              else if (i.NatureOfCall.includes('Fire'))
                icon = 'red-blank.png';

              this.markerCache[cacheKey].push({
                id: i.CFS,
                geometry: i.Geometry,
                content: '<p style="font-size: 150%; font-weight: bold;">' + i.NatureOfCall + '</p>' +
                  i.Address + (i.Apt ? ' APT. #' + i.Apt : '') + '<hr />' +
                  time + ' (' + fromNow + ')<br />' + 
                  '<b><i>' + i.Disposition + '</i></b>',
                icon: 'https://maps.google.com/mapfiles/kml/paddle/' + icon,
                marker: null,
                state: 'Hidden'
              })
            });

            this.markerCacheBounds[cacheKey] = this.bounds;
            this.showMarkers();
          })
          .catch(error => {
            console.log(error);
          });
      }
    },
    areBoundsCached: function (cacheKey) {
      let cacheBounds = this.markerCacheBounds[cacheKey];
      let loadBounds = this.bounds;

      return cacheBounds == null ||
        loadBounds.ne.lng > cacheBounds.ne.lng ||
        loadBounds.sw.lng < cacheBounds.sw.lng ||
        loadBounds.ne.lat > cacheBounds.ne.lat ||
        loadBounds.sw.lat < cacheBounds.sw.lat;
    },
    showMarkers: function () {
      if (!google)
        return;

      for (let cacheKey in this.markerCache) {
        let cache = this.markerCache[cacheKey];
        cache.forEach(markerDetail => {
          if (markerDetail.state == 'Visible')
            markerDetail.state = 'Hide';
        });
      };

      this.visibleMarkerCaches.forEach(cacheKey => {
        this.markerCache[cacheKey].forEach(markerDetail => {

          if (!markerDetail.geometry)
            return;

          if (!markerDetail.geometry || !markerDetail.geometry.coordinates || !markerDetail.geometry.coordinates[0] || !markerDetail.geometry.coordinates[0][0])
            return;

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
          
          let point = markerDetail.geometry.coordinates[0][0];

          markerDetail.marker = new google.maps.Marker({
            position: {
              lat: point[1],
              lng: point[0]
            },
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
      });

      // TODO: Set mapFull
      //this.mapFull = totalCount >= this.mapItemLimit;

      for (let cacheKey in this.markerCache) {
        let cache = this.markerCache[cacheKey];
        cache.forEach(markerDetail => {
          if (markerDetail.state == 'Hide') {
            markerDetail.state = 'Hidden';
            markerDetail.marker.setMap(null);
          }
        });
      };

    },
    // drawMarkers: function (items, getItemMarkerGeometry, getItemId, getItemInfoWindowText, getMarkerIcon) {
    //   if (!google)
    //     return;

    //   let newMarkerWrappers = [];

    //   items.forEach(i => {

    //     let geometry = getItemMarkerGeometry(i);

    //     if (!geometry)
    //       return;

    //     let existingMarkerWrapper = this.markerWrappers.find(w => w.id == getItemId(i));
    //     if (existingMarkerWrapper) {
    //       newMarkerWrappers.push(existingMarkerWrapper);
    //       return;
    //     } else if (geometry && geometry.coordinates && geometry.coordinates[0] && geometry.coordinates[0][0]) {
          
    //       let point = geometry.coordinates[0][0];

    //       let marker = new google.maps.Marker({
    //         position: {
    //           lat: point[1],
    //           lng: point[0]
    //         },
    //         icon: {
    //           url: getMarkerIcon(i),
    //           scaledSize: new google.maps.Size(50, 50),
    //         },
    //         map: this.map
    //       });

    //       marker.addListener('click', e => {
    //         if (this.openInfoWindow)
    //           this.openInfoWindow.close();

    //         this.openInfoWindow = new google.maps.InfoWindow({
    //           content: getItemInfoWindowText(i)
    //         });
    //         this.openInfoWindow.open(this.map, marker);
    //       });

    //       newMarkerWrappers.push({
    //         id: getItemId(i),
    //         marker: marker
    //       });
    //     }
    //   });

    //   let oldMarkerWrappers = this.markerWrappers.filter(m => { return !(newMarkerWrappers.find(x => x.id == m.id)); });
    //   oldMarkerWrappers.forEach(w => { 
    //     w.marker.setMap(null);
    //   });
    //   this.markerWrappers = newMarkerWrappers;
    // },
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

        console.log(this.bounds);

        this.updateTab();

      }, 1000);
    });

    this.getPosition();
  }
};
</script>