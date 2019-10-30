<template>
  <div>
    <page-title title="Crimes" />
    <b-row class="mb-3">
      <b-col>
        <b-card bg-variant="light">
          <b-card-text>
            <address-lookup :addressData.sync="addressData" :locationData.sync="locationData" />
          </b-card-text>
        </b-card>        
      </b-col>
    </b-row>
    <p class="small">This list contains crimes reported in the city of Milwaukee. This is very different from the police dispatch call data, which includes all
      types of police calls, not just crimes. In other words, not every crime listed here has a corresponding dispatch call and vice versa. Location data for crimes
      is a bit inexact. The location data provided by MPD often seems to indicate the block where the crime occurred, but may not exactly match the address. This data
      is updated daily, but it often takes several days for the data to be available. 
      <a href="https://data.milwaukee.gov/dataset/wibr" target="_blank">More details are available here.</a>
    </p>
    <b-row>
      <b-col>
        <hr />
        <filtered-table :settings="tableSettings" :locationData="locationData" @rowClicked="onRowClicked">
        </filtered-table>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import axios from "axios";
import moment from 'moment'

export default {
  name: "CrimeList",
  props: {},
  data() {
    let base = this;
    return {
      addressData: null,
      locationData: null,

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
          let time = moment(raw.reportedDateTime).format('llll');
          let fromNow = moment(raw.reportedDateTime).fromNow();

          let v = [];

          v.push('<div><span style="float: right;">');
          v.push(fromNow);
          v.push('</span>');
          v.push('<span style="font-size: 125%; font-weight: bold;">')
          v.push(raw.crimeTypes)
          v.push('</span></div>');

          v.push('<hr style="margin-top: 5px; margin-bottom: 5px;" />');

          v.push('<div><b>');
          v.push(raw.location);
          v.push('</b></div>');

          v.push('<div>');
          v.push(time);
          v.push(' (<i>');
          v.push(fromNow);
          v.push('</i>)</div>' );

          if (raw.weaponUsed) {
            v.push('<div>');
            v.push('Weapon used: ');
            v.push(raw.weaponUsed);
            v.push('</div>');
          }

          v.push('<div><a href="#/crime/');
          v.push(raw.incidentNum);
          v.push('">Details</a></div>');
          
          return v.join('');
        },
        getItemMarkerPosition: function (item) {
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