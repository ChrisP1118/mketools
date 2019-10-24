<template>
  <div>
    <page-loading v-if="!item" />
    <page-title v-if="item" :title="pageTitle" />
    <b-link to="/property">All Properties</b-link>
    <div v-if="item">
      <b-row>
        <b-col xs="12" md="6">
          <b-card class="mt-3">
            <h4 slot="header">Owner</h4>
            <ul>
              <li>Owner Name 1: {{item.owner_name_1}}</li>
              <li>Owner Name 2: {{item.owner_name_2}}</li>
              <li>Owner Name 3: {{item.owner_name_3}}</li>
              <li>Owner Mailing Address: {{item.owner_mail_addr}}</li>
              <li>Owner City and State: {{item.owner_city_state}}</li>
              <li>Owner Zip: {{item.owner_zip}}</li>
            </ul>            
          </b-card>
          <b-card class="mt-3">
            <h4 slot="header">Assessment</h4>
            <ul>
              <li>Current Assessment Year: {{item.yr_assmt}}</li>
              <li>Land: {{item.c_A_LAND}}</li>
              <li>Improvements: {{item.c_A_IMPRV}}</li>
              <li>Total: {{item.c_A_TOTAL}}</li>
              <li>Tax Key: {{item.taxkey}}</li>
            </ul>
          </b-card>
          <b-card class="mt-3">
            <h4 slot="header">Property</h4>
            <ul>
              <li>Number of Units: {{item.nr_units}}</li>
              <li>Number of Stories: {{item.nr_stories}}</li>
              <li>Year Built: {{item.yr_built}}</li>
              <li>Lot Area: {{item.lot_area}} sq. ft.</li>
            </ul>
          </b-card>
        </b-col>
        <b-col xs="12" md="6">
          <b-card class="mt-3">
            <h4 slot="header">Location</h4>
            <ul>
              <li>House Number Low: {{item.house_nr_lo}}</li>
              <li>House Number High: {{item.house_nr_hi}}</li>
              <li>House Number Suffix: {{item.house_nr_sfx}}</li>
              <li>Street Direction: {{item.sdir}}</li>
              <li>Street: {{item.street}}</li>
              <li>Street Type: {{item.sttype}}</li>
            </ul>
            <nearby-map :position="position"></nearby-map>
          </b-card>
        </b-col>
      </b-row>
      <b-row class="mt-3">
        <b-col>
          <b-button-toolbar key-nav>
            <b-button-group class="mx-2">
              <b-button type="button" @click="onClose">Close</b-button>
            </b-button-group>
          </b-button-toolbar>
        </b-col>
      </b-row>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "PropertyView",
  props: ['id'],
  data() {
    return {
      item: null,
      position: null
    };
  },
  computed: {
    pageTitle: function () {
      let x = this.item.house_nr_lo;
      if (this.item.house_nr_hi != this.item.house_nr_lo)
        x += '-' + this.item.house_nr_hi;
      if (this.item.house_nr_sfx)
        x += this.item.house_nr_sfx;

      x += ' ' + this.item.sdir + ' ' + this.item.street + ' ' + this.item.sttype;
      return x;
    }
  },
  methods: {
    load: function () {
      let url = '/api/property/' + this.id;

      axios
        .get(url)
        .then(response => {
          console.log(response);

          this.item = response.data;
          this.position = this.$store.getters.getGeometryPosition(this.item.parcel.outline);
        })
        .catch(error => {
          console.log(error);
        });
    },
    onClose(evt) {
      this.$router.push('/property');
    }
  },
  watch: {
    $route (to, from){
      this.load();
    }
  },
  mounted () {
    this.load();
    // Use this instead of the previous line to test the "Loading" bar
    //window.setTimeout(this.load, 3000);
  }
};
</script>