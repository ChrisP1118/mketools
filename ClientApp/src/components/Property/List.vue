<template>
  <div>
    <page-title title="Properties" />
    <b-row class="mb-3">
      <b-col>
        <b-card bg-variant="light">
          <b-card-text>
            <address-lookup :addressData.sync="addressData" :locationData.sync="locationData" />
          </b-card-text>
        </b-card>        
      </b-col>
    </b-row>
    <p class="small">
      This page displays data from the city's Master Property Record. It contains assessment data -- and more than 90 other data points -- for each property in
      the city. 
      <a href="https://data.milwaukee.gov/dataset/mprop" target="_blank">More details are available here.</a>
    </p>
    <b-row>
      <b-col>
        <hr />
        <filtered-table :settings="tableSettings" :locationData="locationData" :defaultZoomWithLocationData="18" @rowClicked="onRowClicked">
        </filtered-table>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import axios from "axios";
import { mapGetters } from 'vuex'

export default {
  name: "PropertyList",
  props: {},
  data() {
    return {
      addressData: null,
      locationData: null,

      tableSettings: {
        endpoint: '/api/property',
        columns: [
          {
            key: 'taxkey',
            name: 'TAXKEY',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'house_nr_lo',
            name: 'HOUSE_NR_LO',
            visible: true,
            sortable: true,
            filter: 'number'
          },
          {
            key: 'house_nr_hi',
            name: 'HOUSE_NR_HI',
            visible: true,
            sortable: true,
            filter: 'number'
          },
          {
            key: 'sdir',
            name: 'SDIR',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'street',
            name: 'STREET',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'sttype',
            name: 'STTYPE',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'owner_name_1',
            name: 'OWNER_NAME_1',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'owner_name_2',
            name: 'OWNER_NAME_2',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'owner_name_3',
            name: 'OWNER_NAME_3',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'owner_mail_addr',
            name: 'OWNER_MAIL_ADDR',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'owner_city_state',
            name: 'OWNER_CITY_STATE',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'owner_zip',
            name: 'OWNER_ZIP',
            visible: false,
            sortable: true,
            filter: 'text'
          }
        ],
        getDefaultFilter: function () {
        },
        rowClicked: function (item, context) {
          context.$router.push('/property/' + item.taxkey)
        },
        getItemInfoWindowText: function (item) {
          return this.$store.getters.getPropertyInfoWindow(item._raw);
        },
        getItemPolygonGeometry: function (item) {
          if (!item || !item._raw || !item._raw.parcel)
            return null;

          return item._raw.parcel.outline;
        },
        getItemId: function (item) {
          return item._raw.taxkey;
        },
        getItemPolygonColor: function (item) {
          return this.$store.getters.getPropertyItemPolygonColor(item._raw);
        },
        getItemPolygonFillColor: function (item) {
          return this.$store.getters.getPropertyItemPolygonFillColor(item._raw);
        },
        getItemPolygonFillOpacity: function (item) {
          return this.$store.getters.getPropertyItemPolygonFillOpacity(item._raw);
        },
        defaultLimit: 100
      }
    }
  },
  computed: {
    ...mapGetters(['getPropertyInfoWindow']),
  },
  methods: {
    onRowClicked: function (rawItem) {
      this.$router.push('/property/' + rawItem.taxkey);
    }
  },
  mounted () {
  }
};
</script>