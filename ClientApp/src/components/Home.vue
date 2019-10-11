<template>
  <div>
    <b-row v-show="!addressData">
      <b-col>
        <b-jumbotron class="text-center" header="MKE Alerts" lead="See and get notified of police calls and crimes in Milwaukee">
          <div v-if="subscriptions.length > 0">
            <user-subscription-list :subscriptions="subscriptions" @selected="onSubscriptionSelected" @deleted="onSubscriptionDeleted" />
            <hr />
          </div>
          <p v-if="subscriptions.length == 0">Enter an address below to get started.</p>
          <address-lookup :addressData.sync="addressData" :locationData.sync="locationData" />
          <p><i>Remember! An increased awareness of crime does not necessarily indicate an increase in crime.</i></p>
        </b-jumbotron>
      </b-col>
    </b-row>
    <b-row v-show="addressData" class="mb-3">
      <b-col>
        <b-card bg-variant="light">
          <b-card-text>
            <address-lookup :addressData.sync="addressData" :locationData.sync="locationData" />
            <hr />
            <user-subscription-list :subscriptions="subscriptions" @selected="onSubscriptionSelected" @deleted="onSubscriptionDeleted" />
            <hr v-if="subscriptions.length > 0" />
            <b-form inline class="justify-content-center" @submit.stop.prevent>
              Email me whenever there's
              <b-form-select v-model="callType" :options="callTypes" />
              within 
              <b-form-select v-model="distance" :options="distances" />
              of {{addressDataString}}.
              <div>
                <b-form-group>
                  <b-button type="submit" variant="primary" @click="addSubscription">Create Email Notification</b-button>
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
          <b-form-radio-group v-model="mapFilterType" :options="mapFilterTypes" buttons button-variant="outline-primary" size="sm" />
          <span v-show="mapLastUpdated" class="ml-3">Last updated at {{mapLastUpdated}}</span>
        </b-form-group>        
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <basic-map :filterType="mapFilterType" :locationData="locationData" :distance="distance" @updated="onMapUpdated" />
      </b-col>
    </b-row>
    <b-row class="mt-3">
      <b-col>
        <b-alert show variant="secondary">
          <h2>This is not an official City of Milwaukee website.</h2>
          <p>This site is not affiliated in any way with the City of Milwaukee, Milwaukee Police Department, Milwaukee Fire Department, or any other government agency.</p>
          <p>The data on this site is not real-time. Police dispatch call data is available within 30-90 minutes of the call; fire dispatch call data is available within 15-105 minutes of the call.</p>
        </b-alert>
      </b-col>
    </b-row>
    <b-modal id="subscription-modal" size="lg" title="Sign Up for Email Notifications" 
      header-bg-variant="primary" header-text-variant="light" hide-footer footer-bg-variant="info" footer-text-variant="dark">
      <div v-if="!authUser">
        <auth-form></auth-form>
      </div>
      <div v-if="authUser">
        <b-form inline class="justify-content-center" @submit.stop.prevent="addSubscription">
          Email {{authUser}} whenever there's
          <b-form-select v-model="callType" :options="callTypes" />
          within 
          <b-form-select v-model="distance" :options="distances" />
          of {{addressDataString}}.
          <div>
            <b-form-group>
              <b-button type="submit" variant="primary">Create Email Notification</b-button>
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
import { mapState, mapGetters } from 'vuex'

export default {
  name: "Home",
  mixins: [AuthMixin],
  props: {},
  data() {
    return {
      addressData: null,
      locationData: null,

      mapFilterType: '',
      mapFilterTypes: [
        { text: 'All', value: '' },
        { text: 'Police Dispatch Calls', value: 'PoliceDispatchCall' },
        { text: 'Fire Dispatch Calls', value: 'FireDispatchCall' },
      ],
      mapLastUpdated: null,

      // Notifications
      distance: 660,
      callType: 'AnyMajorDispatchCall',

      subscriptions: [],
    }
  },
  computed: {
    ...mapState(['distances', 'callTypes']),
    ...mapGetters(['getCallTypeLabel', 'getDistanceLabel']),
    addressDataString: function () {
      if (!this.addressData)
        return '';
      
      return this.addressData.number + ' ' + this.addressData.streetDirection + ' ' + this.addressData.streetName + ' ' + this.addressData.streetType;
    }
  },
  methods: {
    updateSubscriptions: function (subscription) {
      let id = this.getAuthenticatedUserId();
      if (!id) {
        this.subscriptions = [];
        return;
      }

      axios
        .get('/api/dispatchCallSubscription?filter=applicationUserId%3D%22' + id + '%22')
        .then(response => {
          console.log(response);
          
          this.subscriptions = response.data;
        })
        .catch(error => {
          console.log(error);

          this.subscriptions = [];
        });      
    },
    addSubscription: function () {
      if (!this.authUser) {
        this.$bvModal.show('subscription-modal');
        return;
      }

      axios
        .post('/api/dispatchCallSubscription', {
          applicationUserId: this.$root.$data.authenticatedUser.id,
          dispatchCallType: this.callType,
          distance: this.distance,
          point: {
            type: "Point",
            coordinates: [
              this.locationData.lng,
              this.locationData.lat
            ]
          },
          house_nr: this.addressData.number,
          sdir: this.addressData.streetDirection,
          street: this.addressData.streetName,
          sttype: this.addressData.streetType
        })
        .then(response => {
          console.log(response);

          this.$bvToast.toast('An email will be sent to ' + this.$root.$data.authenticatedUser.username + ' whenever there\'s ' + this.getCallTypeLabel(this.callType) + ' within ' + this.getDistanceLabel(this.distance) + ' of ' + this.addressDataString + '.', {
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
    onSubscriptionDeleted: function (subscription) {
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
          .delete('/api/DispatchCallSubscription/' + subscription.id)
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
    onSubscriptionSelected: function (subscription) {
      this.locationData = {
        lat: subscription.point.coordinates[1], 
        lng: subscription.point.coordinates[0]
      };

      this.addressData = {
        number: subscription.house_nr,
        streetDirection: subscription.sdir,
        streetName: subscription.street,
        streetType: subscription.sttype
      }
      this.distance = subscription.distance;
      this.callType = subscription.dispatchCallType;
    },
    onMapUpdated: function (lastUpdate) {
      this.mapLastUpdated = lastUpdate;
    }
  },
  watch: {
  },
  created() {
    this.$store.dispatch("loadStreetReferences").then(() => {
    });
  },
  async mounted () {
    this.updateSubscriptions();
  }
};
</script>