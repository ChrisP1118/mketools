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
            key: 'OWNER_NAME',
            name: 'OWNER_NAME',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'OWNER_MAIL',
            name: 'OWNER_MAIL',
            visible: true,
            sortable: true,
            filter: 'text'
          }
        ],
        getDefaultFilter: function () {
        },
        rowClicked: function (item, context) {
          context.$router.push('/property/' + item.TAXKEY)
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