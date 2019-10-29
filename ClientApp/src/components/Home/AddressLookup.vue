<template>
  <div>
    <b-alert :show="isGettingAddress" variant="info">
      <div class="text-center">
        <b-spinner variant="primary" label="Loading"></b-spinner>
        Finding your address based on your current location...
      </div>          
    </b-alert>
    <b-form @submit="onSubmit" @submit.stop.prevent>
      <b-form-row class="justify-content-center" style="margin-bottom: -15px;">
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

      isGettingAddress: false
    }
  },
  computed: {
    ...mapState(['streetReferences']),
    ...mapGetters(['getAddressData', 'getLocationData']),
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

          this.lat = response.data.geometry.coordinates[0][0][1];
          this.lng = response.data.geometry.coordinates[0][0][0];

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
      let addressData = null;
      if (this.number) {
        addressData = {
          number: this.number,
          streetDirection: this.streetDirection,
          streetName: this.streetName,
          streetType: this.streetType
        };
      }

      this.$store.dispatch('setAddressData', addressData);
      this.$emit('update:addressData', addressData);
    },
    emitLocationData: function () {
      let locationData = null;
      if (this.lat) {
        locationData = {
          lat: this.lat,
          lng: this.lng
        };
      }

      this.$store.dispatch('setLocationData', locationData);
      this.$emit('update:locationData', locationData);
    }
  },
  mounted () {
    let addressData = this.getAddressData();
    let locationData = this.getLocationData();

    // if (addressData)
    //   console.log(addressData.number + ' ' + addressData.streetName);
    // else
    //   console.log('null');

    // if (locationData)
    //   console.log(locationData.lat + ', ' + locationData.lng);
    // else
    //   console.log('null');

    if (locationData || addressData) {
      this.number = addressData.number;
      this.streetDirection = addressData.streetDirection;
      this.streetName = addressData.streetName;
      this.streetType = addressData.streetType;

      this.lat = locationData.lat;
      this.lng = locationData.lng;

      this.emitAddressData();
      this.emitLocationData();
    } else {
      this.getPosition();
    }
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