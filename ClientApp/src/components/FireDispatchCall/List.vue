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
        endpoint: '/api/FireDispatchCall',
        columns: [
          {
            key: 'CFS',
            name: 'CFS',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'ReportedDateTime',
            name: 'Date/Time',
            visible: true,
            sortable: true,
            filter: 'date'
          },
          {
            key: 'Address',
            name: 'Address',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'City',
            name: 'City',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'NatureOfCall',
            name: 'Nature of Call',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'Disposition',
            name: 'Disposition',
            visible: true,
            sortable: true,
            filter: 'text'
          }
        ],
        getDefaultFilter: function () {
        },
        rowClicked: function (item, context) {
          context.$router.push('/fireDispatchCall/' + item.CFS)
        },
        getItemInfoWindowText: function (item) {
          let raw = item._raw;
          return raw.Location + '<br />' +
            raw.NatureOfCall + '<br />' +
            raw.Status;
        },
        getItemMarkerGeometry: function (item) {
          if (!item || !item._raw)
            return null;
            
          return item._raw.Geometry;
        },
        getItemId: function (item) {
          return item._raw.CallNumber;
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