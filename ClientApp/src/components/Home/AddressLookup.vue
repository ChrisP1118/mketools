<template>
  <div>
    <b-alert :show="isGettingAddress" variant="info">
      <div class="text-center">
        <b-spinner variant="primary" label="Loading"></b-spinner>
        Loading address...
      </div>          
    </b-alert>
    <b-form @submit="onSubmit" @submit.stop.prevent>
      <b-form-row class="justify-content-center">
        <b-form-group>
          <label class="sr-only" for="Number">Number</label>
          <b-form-input v-model="number" id="Number" placeholder="Number" type="number" />
        </b-form-group>
        <b-form-group>
          <label class="sr-only" for="Direction">Direction</label>
          <b-form-select v-model="streetDirection" id="Direction" :options="streetReferences.streetDirections" />
        </b-form-group>
        <b-form-group>
          <label class="sr-only" for="Street">Street</label>
          <b-form-select v-model="streetName" id="Street" :options="streetReferences.streetNames" />
        </b-form-group>
        <b-form-group>
          <label class="sr-only" for="StreetType">Street Type</label>
          <b-form-select v-model="streetType" id="StreetType" :options="streetReferences.streetTypes" />
        </b-form-group>
        <b-form-group>
          <b-button type="submit" variant="primary">Go</b-button>
          <b-button type="button" @click="onClear">Clear</b-button>
        </b-form-group>
      </b-form-row>
      <b-alert variant="danger" show v-if="addressLookupError" class="text-center">{{addressLookupError}}</b-alert>
    </b-form>
  </div>
</template>

<script>
import axios from "axios";
import { mapState, mapGetters, mapActions } from 'vuex'

export default {
  name: 'AddressLookup',
  mixins: [],
  props: [
    'addressData',
    'locationData'
  ],
  data() {
    return {
      number: null,
      streetDirection: '',
      streetName: '',
      streetType: '',
      addressLookupError: null,

      lat: null,
      lng: null,
    }
  },
  computed: {
    ...mapState(['streetReferences'])
  },
  methods: {
    getPosition: function () {
      if (!("geolocation" in navigator))
        return;

      navigator.geolocation.getCurrentPosition(this.gotPosition);
    },
    gotPosition: function (position) {
      this.lat = position.coords.latitude;
      this.lng = position.coords.longitude;

      this.isGettingAddress = true;

      this.$store.dispatch('getAddressFromCoordinates', {
        lat: this.lat, 
        lng: this.lng
      }).then(property => {
        this.number = property.house_nr_lo;
        this.streetDirection = property.sdir;
        this.streetName = property.street;
        this.streetType = property.sttype;

        this.emitAddressData();
        this.emitLocationData();

        this.isGettingAddress = false;
      });
    },
    onSubmit: function () {
      axios
        .get('/api/geocoding/fromAddress?address=' + this.number + ' ' + this.streetDirection + ' ' + this.streetName + ' ' + this.streetType)
        .then(response => {
          this.addressLookupError = null;

          this.lat = response.data.geometry.centroid.coordinate[1];
          this.lng = response.data.geometry.centroid.coordinate[0];

          this.emitAddressData();
          this.emitLocationData();
        })
        .catch(error => {
          console.log(error);

          this.addressLookupError = 'That address doesn\'t seem to exist. Please try again.';
        });
    },
    onClear: function () {
      this.number = null;
      this.streetDirection = '';
      this.streetName = '';
      this.streetType = '';

      this.lat = null;
      this.lng = null;

      this.emitAddressData();
      this.emitLocationData();
    },
    emitAddressData: function () {
      if (!this.number)
        this.$emit('update:addressData', null);
      else
        this.$emit('update:addressData', {
          number: this.number,
          streetDirection: this.streetDirection,
          streetName: this.streetName,
          streetType: this.streetType
        });
    },
    emitLocationData: function () {
      if (!this.lat)
        this.$emit('update:locationData',  null);
      else
        this.$emit('update:locationData', {
          lat: this.lat,
          lng: this.lng
        });
    }
  },
  mounted () {
    this.getPosition();
  },
  created() {
    this.$store.dispatch("loadStreetReferences");
  },
  watch: {
    addressData: function (newValue, oldValue) {
      if (!newValue) {
        this.number = null;
        this.streetDirection = '';
        this.streetName = '';
        this.streetType = '';
      } else {
        this.number = newValue.number;
        this.streetDirection = newValue.streetDirection;
        this.streetName = newValue.streetName;
        this.streetType = newValue.streetType;
      }
    },
    locationData: function (newValue, oldValue) {
      if (!newValue) {
        this.lat = null;
        this.lng = null;
      } else {
        this.lat = newValue.lat;
        this.lng = newValue.lng;
      }
    }
  }
};
</script>