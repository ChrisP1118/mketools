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
        endpoint: '/api/PoliceDispatchCall',
        columns: [
          {
            key: 'CallNumber',
            name: 'Call Number',
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
            key: 'Location',
            name: 'Location',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'District',
            name: 'District',
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
            key: 'Status',
            name: 'Status',
            visible: true,
            sortable: true,
            filter: 'text'
          }
        ],
        defaultSortColumn: 'ReportedDateTime',
        defaultSortOrder: 'desc',
        getDefaultFilter: function () {
        },
        rowClicked: function (item, context) {
          context.$router.push('/policeDispatchCall/' + item.CallNumber)
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