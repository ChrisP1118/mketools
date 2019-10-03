<template>
  <div>
    <page-title title="Properties" />
    <b-row>
      <b-col>
        <filtered-table :settings="tableSettings">
        </filtered-table>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "PropertyList",
  props: {},
  data() {
    let base = this;
    return {
      tableSettings: {
        endpoint: '/api/property',
        columns: [
          {
            key: 'taxkey',
            name: 'TAXKEY',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'house_nr_hi',
            name: 'HOUSE_NR_HI',
            visible: true,
            sortable: true,
            filter: 'number'
          },
          {
            key: 'house_nr_lo',
            name: 'HOUSE_NR_LO',
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
          let raw = item._raw;
          return (raw.house_nr_lo == raw.house_nr_hi ? raw.house_nr_lo : raw.house_nr_lo + '-' + raw.house_nr_hi) + ' ' + raw.sdir + ' ' + raw.street + ' ' + raw.sttype + '<br />' +
            raw.owner_name_1;
        },
        getItemPolygonGeometry: function (item) {
          if (!item || !item._raw || !item._raw.parcel)
            return null;

          return item._raw.parcel.outline;
        },
        getItemId: function (item) {
          return item._raw.taxkey;
        }
      }
    }
  },
  methods: {
  },
  mounted () {
  }
};
</script>