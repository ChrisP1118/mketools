<template>
  <div>
    <page-loading v-if="!item" />
    <page-title v-if="item" :title="pageTitle" />
    <b-link to="/parcel">All Properties</b-link>
    <div v-if="item">
      <b-row>
        <b-col xs="12" md="6">
          <b-card class="mt-3">
            <h4 slot="header">Description</h4>
            <ul>
              <li>Address: {{item.address}}</li>
              <li>Municipality: {{item.muniname}}</li>
              <li>Zip: {{item.par_zip}}<span v-if="item.par_zip_ex">-{{item.par_zip_ex}}</span></li>
              <li>Description: {{item.descriptio}}</li>
              <li>Legal Description: {{item.legaldescr}}</li>
              <li>Parcel Type: {{item.parcel_typ}}</li>
              <li>Tax Key: {{item.taxkey}}</li>
            </ul>            
          </b-card>
          <b-card class="mt-3">
            <h4 slot="header">Owner</h4>
            <ul>
              <li v-if="item.condo_name">Condo Name: <b-link :to="'/parcel?condo_name=' + item.condo_name">{{item.condo_name}}</b-link></li>
              <li>Owner 1: <b-link :to="'/parcel?ownername1=' + item.ownername1">{{item.ownername1}}</b-link></li>
              <li v-if="item.ownername2">Owner 2: <b-link :to="'/parcel?ownername2=' + item.ownername2">{{item.ownername2}}</b-link></li>
              <li v-if="item.ownername3">Owner 3: <b-link :to="'/parcel?ownername3=' + item.ownername3">{{item.ownername3}}</b-link></li>
              <li v-if="item.owneraddr">Address: <b-link :to="'/parcel?owneraddr=' + item.owneraddr">{{item.owneraddr}}</b-link></li>
              <li v-if="item.ownerctyst">City/State: {{item.ownerctyst}}</li>
              <li v-if="item.ownerzip">Zip Code: {{item.ownerzip}}</li>
              <li v-if="property && property.last_name_chg">Last Name Change: {{property.last_name_chg}}</li>
            </ul>            
          </b-card>
          <b-card class="mt-3">
            <h4 slot="header">Details</h4>
            <ul>
              <li v-if="property">Owner Occupied: {{property.own_ocpd == 'O' ? 'Yes' : 'No'}}</li>
              <li v-if="property">Year Built: {{property.yr_built}}</li>
              <!-- This is unreliable with mprop2019 -->
              <!-- <li>Area: {{property.bldg_area}}</li> -->
              <!-- <li>Number of Units: {{property.nr_units}}</li>
              <li>Number of Stories: {{property.nr_stories}}</li>
              <li>Bedrooms: {{property.bedrooms}}</li>
              <li>Bathrooms: {{property.baths}}</li>
              <li>Powder Rooms: {{property.powder_rooms}}</li>
              <li>Attic: {{property.attic != 'N' ? 'Yes' : 'No'}}</li>
              <li>Basement: {{property.basement != 'N' ? 'Yes' : 'No'}}</li> -->
            </ul>            
          </b-card>
        </b-col>
        <b-col xs="12" md="6">
          <b-card class="mt-3">
            <h4 slot="header">Map</h4>
            <nearby-map :position="position"></nearby-map>
          </b-card>
          <b-card class="mt-3" v-if="addresses.length > 1">
            <h4 slot="header">Addresses</h4>
            <b-table-lite :items="addresses">
            </b-table-lite>
          </b-card>
        </b-col>
      </b-row>
      <b-row>
        <b-col>
          <b-card class="mt-3">
            <h4 slot="header">History</h4>
            <b-table-lite :items="propertiesSimple">
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
      position: null,
      property: {},
      properties: [],
      assessments: [],
      addresses: []
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
    }
  },
  methods: {
    load: function () {
      let url = '/api/parcel/' + this.id + '?includes=' + encodeURIComponent('commonParcel,properties,addresses');

      axios
        .get(url)
        .then(response => {
          this.item = response.data;
          this.position = this.$store.getters.getGeometryPosition(this.item.commonParcel.outline);

          // If these are computed, they don't refresh when the page changes - I'm guessing it's something to do with the sort, which sorts in place?
          this.properties = this.item.properties.sort((a, b) => new moment(b.sourceDate).unix() - new moment(a.sourceDate).unix());

          this.property = this.properties[0];

          this.propertiesSimple = this.properties.map(x => ({
            year: new moment(x.sourceDate).year(),
            land: x.c_a_land,
            improvements: x.c_a_imprv,
            total: x.c_a_total,
            exemptLand: x.c_a_exm_land,
            exemptImprovements: x.c_a_exm_imprv,
            exemptTotal: x.c_a_exm_total,
            owner: x.owner_name_1 + (x.owner_name_2 ? ', ' + x.owner_name_2 : '') + (x.owner_name_3 ? ', ' + x.owner_name_3 : ''),
            ownerAddress: (x.owner_mail_addr ?  x.owner_mail_addr + ', ' : '') + x.owner_city_state + ' ' + x.owner_zip
          }));

          this.addresses = this.item.addresses.sort((a, b) => ((a.houseNumber - b.houseNumber) * 10000) + (a.unit - b.unit)).map(x => ({
            houseNo: x.houseno,
            houseSx: x.housesx,
            dir: x.dir,
            street: x.street,
            sttype: x.sttype,
            pdir: x.pdir,
            unit: x.unit,
            fulladdr: x.fulladdr
          }));

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