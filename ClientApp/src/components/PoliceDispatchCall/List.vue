<template>
  <div>
    <page-title title="Dispatch Calls" />
    <b-row>
      <b-col>
        <filtered-table :settings="tableSettings">
        </filtered-table>
      </b-col>
    </b-row>
  </div>
</template>

<script>
export default {
  name: "PoliceDispatchCallList",
  props: {},
  data() {
    return {
      tableSettings: {
        endpoint: '/api/policeDispatchCall',
        columns: [
          {
            key: 'callNumber',
            name: 'Call Number',
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
            key: 'location',
            name: 'Location',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'district',
            name: 'District',
            visible: true,
            sortable: true,
            filter: 'number'
          },
          {
            key: 'natureOfCall',
            name: 'Nature of Call',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'status',
            name: 'Status',
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
          return raw.location + '<br />' +
            raw.natureOfCall + '<br />' +
            raw.status;
        },
        getItemMarkerGeometry: function (item) {
          if (!item || !item._raw || !item._raw.geometry || !item._raw.geometry.coordinates[0] || !item._raw.geometry.coordinates[0][0])
            return null;
            
          return {
            lat: item._raw.geometry.coordinates[0][0][1],
            lng: item._raw.geometry.coordinates[0][0][0]
          };          
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