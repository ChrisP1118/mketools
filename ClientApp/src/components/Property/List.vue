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
        endpoint: '/api/Property',
        columns: [
          {
            key: 'TAXKEY',
            name: 'TAXKEY',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'HOUSE_NR_HI',
            name: 'HOUSE_NR_HI',
            visible: true,
            sortable: true,
            filter: 'number'
          },
          {
            key: 'HOUSE_NR_LO',
            name: 'HOUSE_NR_LO',
            visible: true,
            sortable: true,
            filter: 'number'
          },
          {
            key: 'SDIR',
            name: 'SDIR',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'STREET',
            name: 'STREET',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'STTYPE',
            name: 'STTYPE',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'OWNER_NAME_1',
            name: 'OWNER_NAME_1',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'OWNER_MAIL_ADDR',
            name: 'OWNER_MAIL_ADDR',
            visible: false,
            sortable: true,
            filter: 'text'
          }
        ],
        getDefaultFilter: function () {
        },
        rowClicked: function (item, context) {
          context.$router.push('/property/' + item.TAXKEY)
        },
        getItemInfoWindowText: function (item) {
          let raw = item._raw;
          return (raw.HOUSE_NR_LO == raw.HOUSE_NR_HI ? raw.HOUSE_NR_LO : raw.HOUSE_NR_LO + '-' + raw.HOUSE_NR_HI) + ' ' + raw.SDIR + ' ' + raw.STREET + ' ' + raw.STTYPE + '<br />' +
            raw.OWNER_NAME_1;
        },
        getItemPolygonGeometry: function (item) {
          if (!item || !item._raw || !item._raw.Parcel)
            return null;

          return item._raw.Parcel.Outline;
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