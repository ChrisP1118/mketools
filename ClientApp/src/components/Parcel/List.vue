<template>
  <div>
    <!-- <page-title title="Properties" /> -->
    <b-row v-show="!addressData">
      <b-col>
        <b-jumbotron class="text-center">
          <template v-slot:header>
            <img src="../../assets/MkeAlerts_100_60.png" style="margin-bottom: 8px;" />
            MKE Property Data
          </template>
          <template v-slot:lead>
            <div>
              Explore public assessment and ownership data for properties in Milwaukee
            </div>
            <small><a href="https://data.milwaukee.gov/dataset/mprop" target="_blank">More details are available here.</a></small>
          </template>
          <p>Enter an address to get started.</p>
          <address-lookup :addressData.sync="addressData" :locationData.sync="locationData" />
        </b-jumbotron>
      </b-col>
    </b-row>
    <b-row v-show="addressData" class="mb-3">
      <b-col>
        <b-card bg-variant="light">
          <b-card-text>
            <address-lookup :addressData.sync="addressData" :locationData.sync="locationData" />
          </b-card-text>
        </b-card>        
      </b-col>
    </b-row>
    <!-- <b-row class="mb-3">
      <b-col>
        <b-card bg-variant="light">
          <b-card-text>
            <address-lookup :addressData.sync="addressData" :locationData.sync="locationData" />
          </b-card-text>
        </b-card>        
      </b-col>
    </b-row> -->
    <!-- <p class="small">
      This page displays data, including assessment and ownership data, from public property records.
      <a href="https://data.milwaukee.gov/dataset/mprop" target="_blank">More details are available here.</a>
    </p> -->
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
  name: "ParcelList",
  props: {},
  data() {
    return {
      addressData: null,
      locationData: null,

      tableSettings: {
        endpoint: '/api/parcel',
        columns: [
          {
            key: 'taxkey',
            name: 'TAXKEY',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'address',
            name: 'Address',
            visible: true,
            sortable: false,
            filter: 'text'
          },
          {
            key: 'houseNumber',
            name: 'House Number',
            visible: false,
            sortable: true,
            filter: 'number'
          },
          {
            key: 'houseNumberHigh',
            name: 'House Number (High)',
            visible: false,
            sortable: true,
            filter: 'number'
          },
          {
            key: 'streetdir',
            name: 'Street Dir',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'streetname',
            name: 'Street Name',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'streettype',
            name: 'Street Type',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'muniname',
            name: 'Municipality',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'v-owners',
            name: 'Owners',
            visible: false,
            sortable: false,
            render: (x, row) => {
              return [
                row.ownername1,
                row.ownername2 ? ', ' + row.ownername2 : '',
                row.ownername3 ? ', ' + row.ownername3 : '',
              ].join('');
            }
          },
          {
            key: 'ownername1',
            name: 'Owner Name 1',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'ownername2',
            name: 'Owner Name 2',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'ownername3',
            name: 'Owner Name 3',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'v-owners-address',
            name: 'Owners Complete Address',
            visible: false,
            sortable: false,
            render: (x, row) => {
              return [
                row.owneraddr,
                ' ',
                row.ownerctyst,
                ' ',
                row.ownerzip ? (row.ownerzip.substring(0, 5) == '00000' ? '' : row.ownerzip.substring(0, 5)) : ''
              ].join('');
            }
          },
          {
            key: 'owneraddr',
            name: 'Owner Address',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'ownerctyst',
            name: 'Owner City/State',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'ownerzip',
            name: 'Owner Zip',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'condo_name',
            name: 'Condo Name',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'unitnumber',
            name: 'Unit Number',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'descriptio',
            name: 'Description',
            visible: false,
            sortable: true,
            filter: 'text'
          }
        ],
        includes: [
          'commonParcel'
          // This uses a fairly expensive view, so it really slows things down
          // 'currentProperty'
        ],
        getDefaultFilter: function () {
        },
        onRefreshingItems: function (items) {
          items.forEach(i => {
            if (!i.commonParcel)
              return;
            
            i.commonParcel.parcels = items.filter(x => x.map_id == i.map_id);
          })
        },
        rowClicked: function (item, context) {
          context.$router.push('/parcel/' + item.taxkey)
        },
        getItemInfoWindowText: function (item) {
          return this.$store.getters.getCommonParcelInfoWindow(item._raw.commonParcel);
        },
        getItemPolygonGeometry: function (item) {
          if (!item || !item._raw || !item._raw.commonParcel)
            return null;

          return item._raw.commonParcel.outline;
        },
        getItemId: function (item) {
          return item._raw.taxkey;
        },
        getItemPolygonColor: function (item) {
          return this.$store.getters.getCommonParcelPolygonColor(item._raw.commonParcel);
        },
        getItemPolygonFillColor: function (item) {
          return this.$store.getters.getCommonParcelPolygonFillColor(item._raw.commonParcel);
        },
        getItemPolygonFillOpacity: function (item) {
          return this.$store.getters.getCommonParcelPolygonFillOpacity(item._raw.commonParcel);
        },
        defaultLimit: 250
      }
    }
  },
  computed: {
    ...mapGetters(['getParcelInfoWindow']),
  },
  methods: {
    onRowClicked: function (rawItem) {
      this.$router.push('/parcel/' + rawItem.taxkey);
    }
  },
  mounted () {
  }
};
</script>