<template>
  <div>
    <page-title title="Dispatch Calls" />
    <b-row>
      <b-col>
        <filtered-table :settings="tableSettings" @rowClicked="onRowClicked">
        </filtered-table>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import moment from 'moment'

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
        getItemInfoWindowText: function (item) {
          let raw = item._raw;

          let time = moment(raw.reportedDateTime).format('llll');
          let fromNow = moment(raw.reportedDateTime).fromNow();

          return '<p style="font-size: 150%; font-weight: bold;">' + raw.natureOfCall + '</p>' +
            raw.location + ' (Police District ' + raw.district + ')<hr />' +
            time + ' (' + fromNow + ')<br />' + 
            '<b><i>' + raw.status + '</i></b>';
        },
        getItemMarkerGeometry: function (item) {
          if (!item || !item._raw || !item._raw.geometry || !item._raw.geometry.coordinates[0] || !item._raw.geometry.coordinates[0][0])
            return null;
            
          return {
            lat: item._raw.geometry.coordinates[0][0][1],
            lng: item._raw.geometry.coordinates[0][0][0]
          };          
        },
        getItemIcon: function (item) {
          return this.$store.getters.getPoliceDispatchCallTypeIcon(item._raw.natureOfCall);
        },
        getItemId: function (item) {
          return item._raw.callNumber;
        }
      }
    }
  },
  computed: {
  },
  methods: {
    onRowClicked: function (rawItem) {
      this.$router.push('/policeDispatchCall/' + rawItem.callNumber);
    }
  },
  mounted () {
    this.$store.dispatch("loadPoliceDispatchCallTypes");
  }
};
</script>