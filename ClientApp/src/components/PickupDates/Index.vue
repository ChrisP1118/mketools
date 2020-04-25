<template>
  <div>
    <b-row v-show="!addressData">
      <b-col>
        <b-jumbotron class="text-center" lead="Get notified before garbage and recycling pickup days">
          <template v-slot:header>
            <img src="../../assets/MkeAlerts_100_60.png" style="margin-bottom: 8px;" />
            MKE Trash Day Alerts
          </template>
          <!-- <div v-if="subscriptions.length > 0">
            <user-subscription-list :subscriptions="subscriptions" @selected="onSubscriptionSelected" @deleted="onSubscriptionDeleted" />
            <hr />
          </div>
          <p v-if="subscriptions.length == 0">Enter an address below to get started.</p> -->
          <address-lookup :addressData.sync="addressData" :noGeolookup="true" />
          <p><i>Enter an address above to get started.</i></p>
        </b-jumbotron>
      </b-col>
    </b-row>
    <b-row v-show="addressData" class="mb-3">
      <b-col>
        <b-card bg-variant="light">
          <b-card-text>
            <address-lookup :addressData.sync="addressData" :noGeolookup="true"/>
            <!-- <hr />
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
            </b-form> -->
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
    <b-row class="mt-3">
      <b-col>
        <b-alert show variant="secondary">
          <h2>This is not an official City of Milwaukee website.</h2>
          <p>This site is not affiliated in any way with the City of Milwaukee or any other government agency.</p>
        </b-alert>
      </b-col>
    </b-row>
    <!-- <b-modal id="subscription-modal" size="lg" title="Sign Up for Email Notifications" 
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
    </b-modal> -->
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

      subscriptions: [],
    }
  },
  computed: {
    ...mapState(['streetReferences']),
    ...mapGetters(['getAddressData']),
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
  },
  watch: {
    addressData: function (newValue, oldValue) {
      console.log(newValue);
      
      axios
        .get('/api/pickupDates/fromAddress?number=' + this.addressData.number + '&direction=' + this.addressData.streetDirection + '&street=' + this.addressData.streetName + '&suffix=' + this.addressData.streetType)
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
  }
};
</script>