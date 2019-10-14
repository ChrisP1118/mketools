<template>
  <div>
    <page-title title="Crimes" />
    <p class="small">Crime data is updated weekly.</p>
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
            key: 'crimeTypes',
            name: "Type of Crime",
            visible: true,
            sortable: false
          },
          {
            key: 'ald',
            name: 'Aldermanic District',
            visible: false,
            sortable: true,
            filter: 'number'
          },
          {
            key: 'police',
            name: 'Police District',
            visible: false,
            sortable: true,
            filter: 'number'
          },
          {
            key: 'ward',
            name: 'Ward',
            visible: false,
            sortable: true,
            filter: 'number'
          },
          {
            key: 'zip',
            name: 'Zip Code',
            visible: false,
            sortable: true,
            filter: 'number'
          },
          {
            key: 'arson',
            name: 'Arson',
            visible: false,
            sortable: true,
            filter: 'select',
            selectOptions: [
              { text: '', value: "0" },
              { text: 'Arson', value: "1" }
            ]
          },
          {
            key: 'assaultOffense',
            name: 'Assault Offense',
            visible: false,
            sortable: true,
            filter: 'select',
            selectOptions: [
              { text: '', value: "0" },
              { text: 'Assault Offense', value: "1" }
            ]
          },
          {
            key: 'burglary',
            name: 'Burglary',
            visible: false,
            sortable: true,
            filter: 'select',
            selectOptions: [
              { text: '', value: "0" },
              { text: 'Burglary', value: "1" }
            ]
          },
          {
            key: 'criminalDamage',
            name: 'Criminal Damage',
            visible: false,
            sortable: true,
            filter: 'select',
            selectOptions: [
              { text: '', value: "0" },
              { text: 'Criminal Damage', value: "1" }
            ]
          },
          {
            key: 'homicide',
            name: 'Homicide',
            visible: false,
            sortable: true,
            filter: 'select',
            selectOptions: [
              { text: '', value: "0" },
              { text: 'Homicide', value: "1" }
            ]
          },
          {
            key: 'lockedVehicle',
            name: 'Locked Vehicle',
            visible: false,
            sortable: true,
            filter: 'select',
            selectOptions: [
              { text: '', value: "0" },
              { text: 'Locked Vehicle', value: "1" }
            ]
          },
          {
            key: 'robbery',
            name: 'Robbery',
            visible: false,
            sortable: true,
            filter: 'select',
            selectOptions: [
              { text: '', value: "0" },
              { text: 'Robbery', value: "1" }
            ]
          },
          {
            key: 'sexOffense',
            name: 'Sex Offense',
            visible: false,
            sortable: true,
            filter: 'select',
            selectOptions: [
              { text: '', value: "0" },
              { text: 'Sex Offense', value: "1" }
            ]
          },
          {
            key: 'theft',
            name: 'Theft',
            visible: false,
            sortable: true,
            filter: 'select',
            selectOptions: [
              { text: '', value: "0" },
              { text: 'Theft', value: "1" }
            ]
          },
          {
            key: 'vehicleTheft',
            name: 'Vehicle Theft',
            visible: false,
            sortable: true,
            filter: 'select',
            selectOptions: [
              { text: '', value: "0" },
              { text: 'Vehicle Theft', value: "1" }
            ]
          },
        ],
        defaultSortColumn: 'reportedDateTime',
        defaultSortOrder: 'desc',
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
    onRowClicked: function (rawItem) {
      console.log(rawItem.incidentNum);
      this.$router.push('/crime/' + rawItem.incidentNum);
    }
  },
  mounted () {
  }
};
</script>