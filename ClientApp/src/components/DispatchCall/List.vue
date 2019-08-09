<template>
  <div>
    <page-title title="Dispatch Calls" />
    <b-row>
      <b-col>
        <filtered-table :settings="tableSettings">
        </filtered-table>
      </b-col>
      <b-col>
        <filtered-table-map>
        </filtered-table-map>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "DispatchCallList",
  props: {},
  data() {
    let base = this;
    return {
      tableSettings: {
        endpoint: '/api/DispatchCall',
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
        getDefaultFilter: function () {
        },
        rowClicked: function (item, context) {
          context.$router.push('/dispatchCall/' + item.TAXKEY)
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