<template>
  <div>
    <b-row v-show="!addressData">
      <b-col>
        <b-jumbotron class="text-center" lead="Get notified before garbage and recycling pickup days">
          <template v-slot:header>
            <img src="../../assets/MkeAlerts_100_60.png" style="margin-bottom: 8px;" />
            MKE Trash Day Alerts
          </template>
          <div v-if="subscriptions.length > 0">
            <user-pickup-dates-subscription-list :subscriptions="subscriptions" @selected="onSubscriptionSelected" @deleted="onSubscriptionDeleted" />
            <hr />
          </div>
          <p v-if="subscriptions.length == 0">Enter an address below to get started.</p>
          <address-lookup :addressData.sync="addressData" :noGeolookup="true" />
        </b-jumbotron>
      </b-col>
    </b-row>
    <b-row v-show="addressData" class="mb-3">
      <b-col>
        <b-card bg-variant="light">
          <b-card-text>
            <address-lookup :addressData.sync="addressData" :noGeolookup="true"/>
            <hr />
            <user-pickup-dates-subscription-list :subscriptions="subscriptions" @selected="onSubscriptionSelected" @deleted="onSubscriptionDeleted" />
            <hr v-if="subscriptions.length > 0" />
            <b-form inline class="justify-content-center" @submit.stop.prevent>
              Email me at
              <b-form-select v-model="hoursBefore" :options="notificationTimes" />
              every garbage or recycling pickup day for {{addressDataString}}.
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
    <b-row class="mt-3">
      <b-col>
        <b-card-group v-if="pickupDates">
          <b-card class="text-center">
            <template v-slot:header>
              <h4>
                <font-awesome-icon icon="trash" />
                Next Garbage Pickup
              </h4>                
            </template>
            <b-card-text>
              <h5>{{formattedNextGarbagePickupDate}}</h5>
            </b-card-text>
          </b-card>
          <b-card class="text-center">
            <template v-slot:header>
              <h4>
                <font-awesome-icon icon="recycle" />
                Next Recycling Pickup
              </h4>                
            </template>
            <b-card-text>
              <h5>{{formattedNextRecyclingPickupDate}}</h5>
            </b-card-text>
          </b-card>
        </b-card-group>
      </b-col>
    </b-row>
    <b-modal id="subscription-modal" size="lg" title="Sign Up for Email Notifications" 
      header-bg-variant="primary" header-text-variant="light" hide-footer footer-bg-variant="info" footer-text-variant="dark">
      <div v-if="!authUser">
        <auth-form></auth-form>
      </div>
      <div v-if="authUser">
        <b-form inline class="justify-content-center" @submit.stop.prevent="addSubscription">
          Email {{authUser}} at
          <b-form-select v-model="hoursBefore" :options="notificationTimes" />
          every garbage or recycling pickup day for {{addressDataString}}.
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
import moment from 'moment'
import AuthMixin from '../Mixins/AuthMixin.vue';
import { mapState, mapGetters, mapActions } from 'vuex'

export default {
  name: "PickupDatesIndex",
  mixins: [AuthMixin],
  props: {},
  data() {
    return {
      addressData: null,
      pickupDates: null,

      hoursBefore: -6,

      subscriptions: [],
    }
  },
  computed: {
    ...mapState(['streetReferences', 'notificationTimes']),
    ...mapGetters(['getAddressData', 'getNotificationTimeLabel']),
    addressDataString: function () {
      if (!this.addressData)
        return '';
      
      return this.addressData.number + ' ' + this.addressData.streetDirection + ' ' + this.addressData.streetName + ' ' + this.addressData.streetType;
    },
    formattedNextGarbagePickupDate: function () {
      return moment(this.pickupDates.nextGarbagePickupDate).format('dddd, MMMM Do');
    },
    formattedNextRecyclingPickupDate: function () {
      return moment(this.pickupDates.nextRecyclingPickupDate).format('dddd, MMMM Do');
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
        .get('/api/pickupDatesSubscription?filter=applicationUserId%3D%22' + id + '%22')
        .then(response => {
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
        .post('/api/pickupDatesSubscription', {
          applicationUserId: this.$root.$data.authenticatedUser.id,
          hoursBefore: this.hoursBefore,
          laddr: this.addressData.number,
          sdir: this.addressData.streetDirection,
          sname: this.addressData.streetName,
          stype: this.addressData.streetType
        })
        .then(response => {
          console.log(response);

          this.$bvToast.toast('An email will be sent to ' + this.$root.$data.authenticatedUser.username + ' at ' + this.getNotificationTimeLabel(this.houseBefore) + ' every garbage of recycling pickup day for ' + this.addressDataString + '.', {
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
          .delete('/api/PickupDatesSubscription/' + subscription.id)
          .then(response => {
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
      this.addressData = {
        number: subscription.laddr,
        streetDirection: subscription.sdir,
        streetName: subscription.sname,
        streetType: subscription.stype
      };

      this.hoursBefore = subscription.hoursBefore;
    },
  },
  watch: {
    addressData: function (newValue, oldValue) {
      axios
        .get('/api/pickupDates/fromAddress?laddr=' + this.addressData.number + '&sdir=' + this.addressData.streetDirection + '&sname=' + this.addressData.streetName + '&stype=' + this.addressData.streetType)
        .then(response => {
          this.pickupDates = response.data;
          })
        .catch(error => {
          console.log(error);
          });
    },
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