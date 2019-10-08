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
        endpoint: '/api/crime',
        columns: [
          {
            key: 'incidentNum',
            name: 'Incident Number',
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
            key: 'weaponUsed',
            name: 'Weapon Used',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'typeOfCrime',
            name: "Type of Crime",
            visible: true,
            sortable: true,
            filter: 'text'
          }
        ],
        getDefaultFilter: function () {
        },
        rowClicked: function (item, context) {
          context.$router.push('/crime/' + item.id)
        },
        getItemInfoWindowText: function (item) {
          let raw = item._raw;
          return raw.location;
        },
        getItemMarkerGeometry: function (item) {
          if (!item || !item._raw || !item._raw.point || !item._raw.point.coordinates)
            return null;

          return {
            lat: item._raw.point.coordinates[1],
            lng: item._raw.point.coordinates[0]
          };          
        },
        getItemId: function (item) {
          return item._raw.incidentNum;
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