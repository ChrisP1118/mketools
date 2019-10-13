<template>
  <div>
    <page-title title="Fire Dispatch Calls" />
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
        getItemInfoWindowText: function (item) {
          let raw = item._raw;

          let time = moment(raw.reportedDateTime).format('llll');
          let fromNow = moment(raw.reportedDateTime).fromNow();

          return '<p style="font-size: 150%; font-weight: bold;">' + raw.natureOfCall + '</p>' +
            raw.address + (raw.apt ? ' APT. #' + raw.apt : '') + '<hr />' +
            time + ' (' + fromNow + ')<br />' + 
            '<b><i>' + raw.disposition + '</i></b>';
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
          return this.$store.getters.getFireDispatchCallTypeIcon(item._raw.natureOfCall);
        },
        getItemId: function (item) {
          return item._raw.cfs;
        }
      }
    }
  },
  methods: {
    onRowClicked: function (rawItem) {
      this.$router.push('/fireDispatchCall/' + rawItem.callNumber);
    }
  },
  mounted () {
    this.$store.dispatch("loadFireDispatchCallTypes");
  }
};
</script>