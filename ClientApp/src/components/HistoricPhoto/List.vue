<template>
  <div>
    <page-title title="Historic Photos" />
    <b-row class="mb-3">
      <b-col>
        <b-card bg-variant="light">
          <b-card-text>
            <address-lookup :addressData.sync="addressData" :locationData.sync="locationData" />
          </b-card-text>
        </b-card>        
      </b-col>
    </b-row>
    <p class="small">This list contains historic photos from the <a href="http://www.mpl.org/special_collections/images/index.php?slug=milwaukee-historic-photos" target="_blank">Milwaukee
      Public Library's historic photos collection</a>.
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
import moment from 'moment'

export default {
  name: "HistoricPhotoList",
  props: {},
  data() {
    return {
      addressData: null,
      locationData: null,

      tableSettings: {
        endpoint: '/api/historicPhoto',
        columns: [
          {
            key: 'id',
            name: 'ID',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'year',
            name: 'Year',
            visible: true,
            sortable: true
          },
          {
            key: 'title',
            name: 'Title',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'currentAddress',
            name: 'Current Address',
            visible: true,
            sortable: false,
            filter: 'text'
          },
          {
            key: 'place',
            name: 'Place',
            visible: true,
            sortable: false,
            filter: 'text'
          },
        ],
        defaultSortColumn: 'id',
        defaultSortOrder: 'asc',
        getDefaultFilter: function () {
        },
        getItemInfoWindowText: function (item) {
          let raw = item._raw;

          let v = [];

          v.push('<div style="font-size: 125%; width: 300px;"><a href="' + raw.url + '" target="_blank">' + raw.title + '</a>' + (raw.year ? ' (' + raw.year + ')' : '') + '</div>');
          v.push('<a href="' + raw.url + '" target="_blank"><img src="' + raw.imageUrl + '" style="max-width: 300px; max-height: 300px;"></img></a>');
          v.push('<div>' + raw.currentAddress + '</div>');

          return v.join('');
        },
        getItemMarkerPosition: function (item) {
          if (!item || !item._raw || !item._raw.geometry || !item._raw.geometry.coordinates)
            return null;

          return {
            lat: item._raw.geometry.coordinates[1],
            lng: item._raw.geometry.coordinates[0]
          };          
        },
        getItemId: function (item) {
          return item._raw.id;
        }
      }
    }
  },
  methods: {
    onRowClicked: function (rawItem) {
      console.log(rawItem.id);
      //this.$router.push('/crime/' + rawItem.incidentNum);
    }
  },
  mounted () {
  }
};
</script>