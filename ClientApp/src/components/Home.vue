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
    <b-row v-if="!showJumbotron" class="mb-3">
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
              <b-alert variant="danger" show v-if="addressLookupError" class="text-center">{{addressLookupError}}</b-alert>
            </b-form>
            <hr />
            <div v-for="subscription in subscriptions" v-bind:key="subscription.Id" class="text-center">
              <a href="#" @click.prevent="setLocationFromSubscription(subscription)">An email will be sent to {{authUser}} whenever there's {{getCallTypeLabel(subscription.DispatchCallType)}} within {{getDistanceLabel(subscription.Distance)}} 
              of {{subscription.HOUSE_NR}} {{subscription.SDIR}} {{subscription.STREET}} {{subscription.STTYPE}}.</a> <a href="#" class="small" @click.prevent="deleteSubscription(subscription.Id)">Delete</a>
            </div>
            <hr v-if="subscriptions.length > 0" />
            <b-form inline class="justify-content-center" @submit.stop.prevent>
              Email me whenever there's
              <b-form-select v-model="callType" :options="callTypes" @change="updateDistance" />
              within 
              <b-form-select v-model="distance" :options="distances" @change="updateDistance" />
              feet of {{userPositionLabel}}.
              <div>
                <b-form-group>
                  <b-button type="submit" variant="primary" v-b-modal.subscription-modal>Sign Up for Notifications</b-button>
                </b-form-group>
              </div>
            </b-form>
          </b-card-text>
        </b-card>        
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <b-form-group>
          <b-form-radio-group v-model="tabKey" :options="mapCacheViews" buttons button-variant="outline-primary" size="sm" />
        </b-form-group>        
      </b-col>
    </b-row>
    <b-row v-if="mapFull">
      <b-col>
        <b-alert variant="warning" show>Only the most recent items are displayed. Zoom in on the map to see more.</b-alert>
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <div class="map" id="homeMap" />
      </b-col>
    </b-row>
    <b-modal id="subscription-modal" size="lg" title="Sign Up for Email Notifications" 
      header-bg-variant="primary" header-text-variant="light" hide-footer footer-bg-variant="info" footer-text-variant="dark">
      <div v-if="!authUser">
        <b-row>
          <b-col>
            <div class="text-center">
              {{authUser}}
              Log in with: 
              <b-button>
                <g-signin-button :params="googleSignInParams" @success="onGoogleSignInSuccess" @error="onGoogleSignInError">
                  <font-awesome-icon :icon="{ prefix: 'fab', iconName: 'google' }"/>
                  Google
                </g-signin-button>
              </b-button>
              <b-button>
                <fb-signin-button :params="fbSignInParams" @success="onFacebookSignInSuccess" @error="onFacebookSignInError">
                  <font-awesome-icon :icon="{ prefix: 'fab', iconName: 'facebook' }"/>
                  Facebook
                </fb-signin-button>
              </b-button>
            </div>
          </b-col>
        </b-row>
        <b-row>
          <b-col>
            <div class="strike">
               <span>or</span>
            </div>
          </b-col>
        </b-row>
        <b-row>
          <b-col md="3"></b-col>
          <b-col md="6" class="text-center">
            <div v-if="notificationPage == 'create'">
              <h2>Sign Up</h2>
              <b-form @submit.prevent="onSignUp">
                <b-form-group>
                  <label class="sr-only" for="signUpEmail">Email Address</label>
                  <b-form-input v-model="signUpEmail" id="signUpEmail" type="email" placeholder="Email Address" required />
                </b-form-group>
                <b-form-group>
                  <label class="sr-only" for="signUpPassword">Password</label>
                  <b-form-input v-model="signUpPassword" id="signUpPassword" type="password" placeholder="Password" required />
                </b-form-group>
                <b-form-group>
                  <label class="sr-only" for="signUpConfirm">Confirm Password</label>
                  <b-form-input v-model="signUpConfirm" id="signUpConfirm" type="password" placeholder="Confirm Password" required />
                </b-form-group>
                <b-button type="submit" variant="primary">Sign Up</b-button>
              </b-form>
              <div>
                <a href="#" @click.prevent="notificationPage = 'login'">Already have an account? Log in.</a>
              </div>
              <div>
                <a href="#" @click.prevent="notificationPage = 'reset'">Forgot your password? Reset it.</a>
              </div>          
            </div>
            <div v-if="notificationPage == 'login'">
              <h2>Log In</h2>
              <b-form @submit.prevent="onLogIn">
                <b-form-group>
                  <label class="sr-only" for="logInEmail">Email Address</label>
                  <b-form-input v-model="logInEmail" id="logInEmail" type="email" placeholder="Email Address" required />
                </b-form-group>
                <b-form-group>
                  <label class="sr-only" for="logInPassword">Password</label>
                  <b-form-input v-model="logInPassword" id="logInPassword" type="password" placeholder="Password" required />
                </b-form-group>
                <b-button type="submit" variant="primary">Log In</b-button>
              </b-form>
              <div>
                <a href="#" @click.prevent="notificationPage = 'create'">Don't have an account? Sign up.</a>
              </div>
              <div>
                <a href="#" @click.prevent="notificationPage = 'reset'">Forgot your password? Reset it.</a>
              </div>          
            </div>
            <div v-if="notificationPage == 'reset'">
              <h2>Reset Password</h2>
              <b-form>
                <b-form-group>
                  <label class="sr-only" for="EmailAddress">Email Address</label>
                  <b-form-input v-model="emailAddress" id="EmailAddress" placeholder="Email Address" type="email" />
                </b-form-group>
                <b-button>Reset Password</b-button>
              </b-form>
              <div>
                <a href="#" @click.prevent="notificationPage = 'login'">Already have an account? Log in.</a>
              </div>
              <div>
                <a href="#" @click.prevent="notificationPage = 'create'">Don't have an account? Sign up.</a>
              </div>
            </div>
          </b-col>
          <b-col md="3"></b-col>
        </b-row>
      </div>
      <div v-if="authUser">
        <b-form inline class="justify-content-center" @submit.stop.prevent="addSubscription">
          Email {{authUser}} whenever there's
          <b-form-select v-model="callType" :options="callTypes" @change="updateDistance" />
          within 
          <b-form-select v-model="distance" :options="distances" @change="updateDistance" />
          feet of {{userPositionLabel}}.
          <div>
            <b-form-group>
              <b-button type="submit" variant="primary" v-b-modal.subscription-modal>Create Notification</b-button>
            </b-form-group>
          </div>
        </b-form>
      </div>
    </b-modal>
  </div>
</template>

<script>
import axios from "axios";
import gmapsInit from './Common/googlemaps';
import moment from 'moment'
import AuthMixin from './Mixins/AuthMixin.vue';

export default {
  name: "Home",
  mixins: [AuthMixin],
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
      addressLookupError: null,

      // Mapping
      mapCacheViews: [
        { text: 'All', value: 'a' },
        { text: 'Active Police Calls', value: 'ap' },
        { text: 'Recent Police Calls', value: 'rp' },
        { text: 'Active Fire Calls', value: 'af' },
        { text: 'Recent Fire Calls', value: 'rf' },
      ],
      google: null,
      map: null,
      bounds: null,
      tabKey: 'a',
      visibleMarkerCaches: ['rp', 'rf'],
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
      userPosition: null,
      userPositionLabel: null,
      distance: 660,
      distances: [
        { text: '1/16 mile', value: 330 },
        { text: '1/8 mile', value: 660 },
        { text: '1/4 mile', value: 1320 },
        { text: '1/2 mile', value: 2640 },
        { text: '1 mile', value: 5280 }
      ],
      callType: 'MajorCall',
      callTypes: [
        { text: 'any police dispatch call', value: 'PoliceDispatchCall' },
        { text: 'any fire dispatch call', value: 'FireDispatchCall' },
        { text: 'any police or fire dispatch call', value: 'AllDispatchCall' },
        { text: 'any major crime or fire call', value: 'MajorCall' }
      ],
      circle: null,
      notificationPage: 'create',
      emailAddress: '',
      password: '',
      confirmPassword: '',

      subscriptions: [],

      // These are in the Mixin now, but I haven't tested removing them here
      // googleSignInParams: {
      //   client_id: '66835382455-403e538rnmmpmcp5tocmndleh30g4i5d.apps.googleusercontent.com'
      // },
      // fbSignInParams: {
      //   scope: 'email,user_likes',
      //   return_scopes: true
      // }
    }
  },
  computed: {
  },
  methods: {
    addSubscription: function () {
      axios
        .post('/api/DispatchCallSubscription', {
          ApplicationUserId: this.$root.$data.authenticatedUser.id,
          DispatchCallType: this.callType,
          Distance: this.distance,
          Point: {
            type: "Point",
            coordinates: [
              this.userPosition.lng,
              this.userPosition.lat
            ]
          },
          SDIR: this.streetDirection,
          STREET: this.streetName,
          STTYPE: this.streetType,
          HOUSE_NR: this.number
        })
        .then(response => {
          console.log(response);

          this.$bvToast.toast('An email will be sent to ' + this.$root.$data.authenticatedUser.username + ' whenever there\'s ' + this.getCallTypeLabel(this.callType) + ' within ' + this.getDistanceLabel(this.distance) + ' of ' + this.userPositionLabel, {
            title: 'Notification Created',
            autoHideDelay: 5000,
            variant: 'success'
          });

          this.$bvModal.hide('subscription-modal');

          this.updateSubscriptions();
        })
        .catch(error => {
          console.log(error);
        });      
    },
    deleteSubscription: function (id) {
      this.$bvModal.msgBoxConfirm('Are you sure you want to remove this notification?', {
        title: 'Remove Notification',
        size: 'lg',
        headerBgVariant: 'primary',
        headerTextVariant: 'light',
        okVariant: 'danger',
        okTitle: 'Yes',
        cancelTitle: 'No',
        footerClass: 'p-2',
        hideHeaderClose: false,
        centered: true
      })
      .then(value => {
        if (!value)
          return;

        axios
          .delete('/api/DispatchCallSubscription/' + id)
          .then(response => {
            console.log(response);

            this.$bvToast.toast('The notification was successfully removed.', {
              title: 'Notification Remove',
              autoHideDelay: 5000,
              variant: 'success'
            });

            this.updateSubscriptions();
          })
          .catch(error => {
            console.log(error);
          });
      })
      .catch(err => {
        // An error occurred
      })      
    },
    getDistanceLabel(distance) {
      return this.distances.find(x => x.value == distance).text;
    },
    getCallTypeLabel(callType) {
      return this.callTypes.find(x => x.value == callType).text;
    },
    updateSubscriptions: function () {
      let id = this.getAuthenticatedUserId();
      if (!id) {
        this.subscriptions = [];
        return;
      }

      axios
        .get('/api/DispatchCallSubscription?filter=applicationUserId%3D%22' + id + '%22')
        .then(response => {
          console.log(response);
          
          this.subscriptions = response.data;
        })
        .catch(error => {
          console.log(error);

          this.subscriptions = [];
        });      
    },
    updateDistance: function () {
      if (this.circle)
        this.circle.setMap(null);

      this.circle = new google.maps.Circle({
        strokeColor: '#bd2130',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: '#0d2240',
        fillOpacity: 0.10,
        map: this.map,
        center: this.userPosition,
        radius: (this.distance * 0.3048) // Feet to meters
      });
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

      this.showJumbotron = false;

      this.userPosition = location;
      this.userPositionLabel = 'my location';

      this.map.setCenter(location);
      this.map.setZoom(15);

      this.updateDistance();

      axios
        .get('/api/Geocoding/FromCoordinates?latitude=' + location.lat + '&longitude=' + location.lng)
        .then(response => {
          console.log(response);
          if (!response.data.Property)
            return;

          this.number = response.data.Property.HOUSE_NR_LO;
          this.streetDirection = response.data.Property.SDIR;
          this.streetName = response.data.Property.STREET;
          this.streetType = response.data.Property.STTYPE;

          this.userPositionLabel = this.number + ' ' + this.streetDirection + ' ' + this.streetName + ' ' + this.streetType;
        })
        .catch(error => {
          console.log(error);
        });      
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
    setLocationFromSubscription: function (subscription) {
      this.location = {
        lat: subscription.Point.coordinates[1], 
        lng: subscription.Point.coordinates[0]
      };

      this.number = subscription.HOUSE_NR;
      this.streetDirection = subscription.SDIR;
      this.streetName = subscription.STREET;
      this.streetType = subscription.STTYPE;

      this.userPosition = this.location;
      this.userPositionLabel = this.number + ' ' + this.streetDirection + ' ' + this.streetName + ' ' + this.streetType;

      this.distance = subscription.Distance;
      this.callType = subscription.DispatchCallType;

      this.map.setCenter(this.location);
      this.map.setZoom(15);

      this.updateDistance();
    },
    onSubmit: function () {
      this.showJumbotron = false;

      axios
        .get('/api/Geocoding/FromAddress?address=' + this.number + ' ' + this.streetDirection + ' ' + this.streetName + ' ' + this.streetType)
        .then(response => {
          let location = {
            lat: response.data.Geometry.Centroid.Coordinate[1], 
            lng: response.data.Geometry.Centroid.Coordinate[0]
          };

          this.userPosition = location;
          this.userPositionLabel = this.number + ' ' + this.streetDirection + ' ' + this.streetName + ' ' + this.streetType;

          this.map.setCenter(location);
          this.map.setZoom(15);

          this.updateDistance();

          this.addressLookupError = null;
        })
        .catch(error => {
          console.log(error);

          this.addressLookupError = 'That address doesn\'t seem to exist. Please try again.';
        });
    },
    onClear: function () {
      this.showJumbotron = true;
      this.number = null;
      this.streetDirection = '';
      this.streetName = '';
      this.streetType = '';
    },
    updateTab: function (tabKey) {
      if (!tabKey)
        tabKey = this.tabKey;

      if (tabKey == 'ap') {
        this.visibleMarkerCaches = ['ap']
        this.loadActivePoliceCalls();
      } else if (tabKey == 'rp') {
        this.visibleMarkerCaches = ['rp']
        this.loadRecentPoliceCalls();
      } else if (tabKey == 'af') {
        this.visibleMarkerCaches = ['af']
        this.loadActiveFireCalls();
      } else if (tabKey == 'rf') {
        this.visibleMarkerCaches = ['rf']
        this.loadRecentFireCalls();
      } else if (tabKey == 'a') {
        this.visibleMarkerCaches = ['rp', 'rf']
        this.loadRecentPoliceCalls();
        this.loadRecentFireCalls();
      }
    },
    loadActivePoliceCalls: function () {
      let now = moment().subtract(6, 'hours').format('YYYY-MM-DD HH:mm:ss');
      this.loadPoliceDispatchCalls('ap', 'Status%20%3D%20%22Service%20in%20Progress%22%20and%20ReportedDateTime%20%3E%3D%20%22' + encodeURIComponent(now) + '%22' + this.getBoundsFilter())
    },
    loadRecentPoliceCalls: function () {
      let now = moment().subtract(6, 'hours').format('YYYY-MM-DD HH:mm:ss');
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
      let now = moment().subtract(6, 'hours').format('YYYY-MM-DD HH:mm:ss');
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
    getBoundsFilter: function() {
      if (!this.bounds)
        return '';

      return '&northBound=' + this.bounds.ne.lat + '&southBound=' + this.bounds.sw.lat + '&eastBound=' + this.bounds.ne.lng + '&westBound=' + this.bounds.sw.lng;
    }
  },
  watch: {
    tabKey: function (newValue, oldValue) {
      this.updateTab(newValue);
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

        this.updateTab();

      }, 1000);
    });

    this.updateSubscriptions();
    this.getPosition();
  }
};
</script>