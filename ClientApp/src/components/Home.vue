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
              <b-form-select v-model="callType" :options="dataStore.callTypes" />
              within 
              <b-form-select v-model="distance" :options="dataStore.distances" />
              of {{addressDataString}}.
              <div>
                <b-form-group>
                  <b-button type="submit" variant="primary" v-b-modal.subscription-modal>Get Email Notifications</b-button>
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
          <b-form-radio-group v-model="mapFilter" :options="mapFilters" buttons button-variant="outline-primary" size="sm" />
        </b-form-group>        
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <basic-map :filter="mapFilter" :locationData="locationData" :distance="distance" />
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        This data is not real-time. Police dispatch call data is available within 30-90 minutes of the call; file dispatch call data is available within 15-105 minutes of the call.
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
          <b-form-select v-model="callType" :options="dataStore.callTypes" />
          within 
          <b-form-select v-model="distance" :options="dataStore.distances" />
          of {{addressDataString}}.
          <div>
            <b-form-group>
              <b-button type="submit" variant="primary" v-b-modal.subscription-modal>Create Email Notification</b-button>
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
import dataStore from './DataStore.vue';

export default {
  name: "Home",
  mixins: [AuthMixin],
  props: {},
  data() {
    return {
      dataStore: dataStore,

      addressData: null,
      locationData: null,

      mapFilter: '',
      mapFilters: [
        { text: 'All', value: '' },
        { text: 'Active Police Calls', value: 'ap' },
        { text: 'Recent Police Calls', value: 'rp' },
        { text: 'Active Fire Calls', value: 'af' },
        { text: 'Recent Fire Calls', value: 'rf' },
      ],

      // Notifications
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

      subscriptions: [],
    }
  },
  computed: {
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
    addSubscription: function () {
      axios
        .post('/api/DispatchCallSubscription', {
          ApplicationUserId: this.$root.$data.authenticatedUser.id,
          DispatchCallType: this.callType,
          Distance: this.distance,
          Point: {
            type: "Point",
            coordinates: [
              this.locationData.lng,
              this.locationData.lat
            ]
          },
          HOUSE_NR: this.addressData.number,
          SDIR: this.addressData.streetDirection,
          STREET: this.addressData.streetName,
          STTYPE: this.addressData.streetType
        })
        .then(response => {
          console.log(response);

          this.$bvToast.toast('An email will be sent to ' + this.$root.$data.authenticatedUser.username + ' whenever there\'s ' + this.dataStore.getCallTypeLabel(this.callType) + ' within ' + this.dataStore.getDistanceLabel(this.distance) + ' of ' + this.addressDataString + '.', {
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
          .delete('/api/DispatchCallSubscription/' + subscription.Id)
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
        lat: subscription.Point.coordinates[1], 
        lng: subscription.Point.coordinates[0]
      };

      this.addressData = {
        number: subscription.HOUSE_NR,
        streetDirection: subscription.SDIR,
        streetName: subscription.STREET,
        streetType: subscription.STTYPE
      }
      this.distance = subscription.Distance;
      this.callType = subscription.DispatchCallType;
    }
  },
  watch: {
  },
  async mounted () {
    this.updateSubscriptions();
  }
};
</script>