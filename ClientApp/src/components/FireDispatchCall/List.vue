<template>
  <div>
    <page-title title="Fire Dispatch Calls" />
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
  name: "FireDispatchCallList",
  props: {},
  data() {
    let base = this;
    return {
      tableSettings: {
        endpoint: '/api/fireDispatchCall',
        columns: [
          {
            key: 'cfs',
            name: 'CFS',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'reportedDateTime',
            name: 'Date/Time',
            visible: true,
            sortable: true,
            filter: 'date'
          },
          {
            key: 'address',
            name: 'Address',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'city',
            name: 'City',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'natureOfCall',
            name: 'Nature of Call',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'disposition',
            name: 'Disposition',
            visible: true,
            sortable: true,
            filter: 'text'
          }
        ],
        defaultSortColumn: 'reportedDateTime',
        defaultSortOrder: 'desc',
        getDefaultFilter: function () {
        },
        openInfoWindowOnRowClick: true,
        getItemInfoWindowText: function (item) {
          let raw = item._raw;
          return raw.address + '<br />' +
            raw.natureOfCall + '<br />' +
            raw.disposition;
        },
        getItemMarkerGeometry: function (item) {
          if (!item || !item._raw)
            return null;
            
          return item._raw.geometry;
        },
        getItemId: function (item) {
          return item._raw.callNumber;
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