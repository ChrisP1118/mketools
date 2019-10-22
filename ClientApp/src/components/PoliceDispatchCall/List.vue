<template>
  <div>
    <page-title title="Dispatch Calls" />
    <p class="small">This list contains police dispatch calls as reported by the Milwaukee Police Department. The data is updated every 5 minutes, but there's a lag
      of around 30-45 minutes between when the calls are made and when the data is available. A call does not necessarily indicate a crime, and the time/location of
      the call does not necessarily indicate the time/location of the underlying incident.
      <a href="https://itmdapps.milwaukee.gov/MPDCallData/index.jsp?district=All" target="_blank">More details are available here.</a>
    </p>
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
            '<b><i>' + raw.status + '</i></b>' +
            '<hr />' +
            '<p style="font-size: 125%;"><a href="#/policeDispatchCall/' + raw.callNumber + '">Details</a></p>';
        },
        getItemMarkerPosition: function (item) {
          if (!item || !item._raw || !item._raw.geometry)
            return null;

          return this.$store.getters.getGeometryPosition(item._raw.geometry);
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