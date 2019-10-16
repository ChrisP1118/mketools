<template>
  <div>
    <page-title title="Fire Dispatch Calls" />
    <b-row>
      <b-col>
        <hr />
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
            '<b><i>' + raw.disposition + '</i></b>' +
            '<hr />' +
            '<p style="font-size: 125%;"><a href="#/fireDispatchCall/' + raw.cfs + '">Details</a></p>';
        },
        getItemMarkerGeometry: function (item) {
          if (!item || !item._raw || !item._raw.geometry)
            return null;

          return this.$store.getters.getGeometryPosition(item._raw.geometry);
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
      this.$router.push('/fireDispatchCall/' + rawItem.cfs);
    }
  },
  mounted () {
    this.$store.dispatch("loadFireDispatchCallTypes");
  }
};
</script>