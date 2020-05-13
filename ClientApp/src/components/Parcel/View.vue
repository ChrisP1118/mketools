<template>
  <div>
    <page-loading v-if="!item" />
    <page-title v-if="item" :title="pageTitle" />
    <b-link to="/parcel">All Parcels</b-link>
    <div v-if="item">
      <b-row>
        <b-col xs="12" md="6">
          <b-card class="mt-3">
            <h4 slot="header">Owner</h4>
            <ul>
              <li>Owner Name 1: {{item.ownername1}}</li>
              <li>Owner Name 2: {{item.ownername2}}</li>
              <li>Owner Name 3: {{item.ownername3}}</li>
              <li>Owner Mailing Address: {{item.owneraddr}}</li>
              <li>Owner City and State: {{item.ownerctyst}}</li>
              <li>Owner Zip: {{item.ownerzip}}</li>
            </ul>            
          </b-card>
          <b-card class="mt-3">
            <h4 slot="header">Assessment</h4>
            <ul>
              <li>Land: {{item.landvalue}}</li>
              <li>Improvements: {{item.impvalue}}</li>
              <li>Total: {{item.assessedva}}</li>
              <li>Tax Key: {{item.taxkey}}</li>
            </ul>
          </b-card>
          <!-- <b-card class="mt-3">
            <h4 slot="header">Property</h4>
            <ul>
              <li>Number of Units: {{item.nr_units}}</li>
              <li>Number of Stories: {{item.nr_stories}}</li>
              <li>Year Built: {{item.yr_built}}</li>
              <li>Lot Area: {{item.lot_area}} sq. ft.</li>
            </ul>
          </b-card> -->
        </b-col>
        <b-col xs="12" md="6">
          <b-card class="mt-3">
            <h4 slot="header">Map</h4>
            <nearby-map :position="position"></nearby-map>
          </b-card>
          <b-card class="mt-3">
            <h4 slot="header">Addresses</h4>
            <b-table-lite :items="addresses">
            </b-table-lite>
          </b-card>
        </b-col>
      </b-row>
      <b-row>
        <b-col>
          <b-card class="mt-3">
            <h4 slot="header">Annual Property Records</h4>
            <b-table-lite :items="properties">
            </b-table-lite>
          </b-card>
        </b-col>
      </b-row>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import moment from 'moment'

export default {
  name: "ParcelView",
  props: ['id'],
  data() {
    return {
      item: null,
      position: null
    };
  },
  computed: {
    pageTitle: function () {
      let x = this.item.houseNumber;
      if (this.item.houseNumberHigh != this.item.houseNumber && this.houseNumberHigh > 0)
        x += '-' + this.item.houseNumberHigh;
      if (this.item.housenrsfx)
        x += this.item.housenrsfx;

      x += ' ' + this.item.streetdir + ' ' + this.item.streetname + ' ' + this.item.streettype;
      return x;
    },
    properties: function () {
      return this.item.properties.sort((a, b) => new moment(b.sourceDate).unix() - new moment(a.sourceDate).unix());
    },
    addresses: function () {
      return this.item.addresses.sort((a, b) => ((a.houseNumber - b.houseNumber) * 10000) + (a.unit - b.unit)).map(function(x) {
        return {
          houseNo: x.houseno,
          houseSx: x.housesx,
          dir: x.dir,
          street: x.street,
          sttype: x.sttype,
          pdir: x.pdir,
          unit: x.unit,
          fulladdr: x.fulladdr
        };
      });
    }
  },
  methods: {
    load: function () {
      let url = '/api/parcel/' + this.id + '?includes=' + encodeURIComponent('commonParcel,properties,addresses');

      axios
        .get(url)
        .then(response => {
          console.log(response);

          this.item = response.data;
          this.position = this.$store.getters.getGeometryPosition(this.item.commonParcel.outline);
        })
        .catch(error => {
          console.log(error);
        });
    },
    onClose(evt) {
      this.$router.push('/parcel');
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