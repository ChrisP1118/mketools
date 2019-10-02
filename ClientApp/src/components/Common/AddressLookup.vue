<template>
  <div>
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
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: 'AddressLookup',
  mixins: [],
  props: {},
  data() {
    return {
      number: null,
      streetDirection: '',
      streetName: '',
      streetType: '',
      streetDirections: [],
      streetNames: [],
      streetTypes: [],
      addressLookupError: null,
    }
  },
  computed: {
  },
  methods: {

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
    }
  },
  mounted () {
    this.loadStreetReferences();
    this.getPosition();
  }
};
</script>