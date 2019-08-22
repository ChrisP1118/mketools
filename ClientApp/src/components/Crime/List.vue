<template>
  <div>
    <page-title title="Crimes" />
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
  name: "CrimeList",
  props: {},
  data() {
    let base = this;
    return {
      tableSettings: {
        endpoint: '/api/Crime',
        columns: [
          {
            key: 'IncidentNum',
            name: 'Incident Number',
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
            key: 'WeaponUsed',
            name: 'Weapon Used',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'TypeOfCrime',
            name: "Type of Crime",
            visible: true,
            sortable: true,
            filter: 'text'
          }
        ],
        getDefaultFilter: function () {
        },
        rowClicked: function (item, context) {
          context.$router.push('/crime/' + item.TAXKEY)
        },
        getItemInfoWindowText: function (item) {
          let raw = item._raw;
          return raw.Location;
        },
        getItemMarkerGeometry: function (item) {
          if (!item || !item._raw)
            return null;
            
          return item._raw.Geometry;
        },
        getItemId: function (item) {
          return item._raw.IncidentNum;
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