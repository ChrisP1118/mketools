<template>
  <div>
    <page-title title="Historic Photos" />
    <b-row class="mb-3">
      <b-col>
        <b-card bg-variant="light" class="mb-3">
          <b-card-text class="text-center">
            This page lets you explore historic photos from the <a href="http://www.mpl.org/special_collections/images/index.php?slug=milwaukee-historic-photos" target="_blank">
            Milwaukee Public Library's historic photos collection</a>.
          </b-card-text>
        </b-card>
        <b-card bg-variant="light">
          <b-card-text>
            <address-lookup :addressData.sync="addressData" :locationData.sync="locationData" />
          </b-card-text>
        </b-card>
      </b-col>
    </b-row>
    <b-row>
      <b-col lg="4" xl="6" class="mb-3">
        <div>
        <b-button-toolbar>
          <b-button-group>
            <b-dropdown :text="selectedOverlay.text">
              <b-dropdown-item v-for="overlayOption in overlays" v-bind:key="overlayOption.value" @click="overlay = overlayOption.value">{{overlayOption.text}}</b-dropdown-item>
            </b-dropdown>
            <b-form-spinbutton v-if="overlay != ''" v-model="opacity" min="0" max="1" step="0.1"></b-form-spinbutton>
          </b-button-group>
          <b-button-group>
            <b-dropdown text="Eras">
              <b-dropdown-item-button v-for="eraOption in eras" v-bind:key="eraOption.value" @click="eraOption.checked = !eraOption.checked">
                <font-awesome-icon icon="square" v-if="!eraOption.checked" />
                <font-awesome-icon icon="check-square" v-if="eraOption.checked" />
                {{eraOption.text}}
              </b-dropdown-item-button>
            </b-dropdown>
          </b-button-group>
        </b-button-toolbar>
        </div>
        <!-- <b-form-select v-model="overlay" :options="overlays" class="w-75"></b-form-select> -->
        <!-- <b-form-spinbutton v-if="overlay != ''" v-model="opacity" min="0" max="1" step="0.1"></b-form-spinbutton> -->
        <div class="explore-map-wrapper">
          <l-map class="explore-map" :zoom="zoom" :center="center" @update:zoom="zoomUpdated" @update:center="centerUpdated" @update:bounds="boundsUpdated" @ready="refreshItems">
            <l-image-overlay v-if="selectedOverlay" :url="selectedOverlay.url" :opacity="opacity" :bounds="[[selectedOverlay.boundN, selectedOverlay.boundW], [selectedOverlay.boundS, selectedOverlay.boundE]]"></l-image-overlay>
            <l-tile-layer :url="tileUrl" :attribution="attribution"></l-tile-layer>
            <l-marker v-for="marker in markers" v-bind:key="marker.id" :lat-lng="marker.position" :icon="marker.icon" @click="onMarkerClick(marker.id)">
            </l-marker>
          </l-map>
        </div>
      </b-col>
      <b-col lg="8" xl="6">
        <div v-if="!selectedItem">
          <div v-if="editOverlay && selectedOverlay">
            <div>
              <b-input v-model="opacity"></b-input>
            </div>
            <div>
              E: <b-input v-model="selectedOverlay.boundE" type="number"></b-input>
            </div>
            <div>
              W: <b-input v-model="selectedOverlay.boundW" type="number"></b-input>
            </div>
            <div>
              N: <b-input v-model="selectedOverlay.boundN" type="number"></b-input>
            </div>
            <div>
              S: <b-input v-model="selectedOverlay.boundS" type="number"></b-input>
            </div>
          </div>

          <p>Click on a marker on the map to view photos from that location.</p>
        </div>
        <div v-if="selectedItem">
          <b-card-group columns>
            <b-card v-for="(historicPhoto, index) in filteredHistoricPhotos" v-bind:key="index">
              <template v-slot:header>
                <span class="float-right" v-if="historicPhoto.year">
                  <b-badge>{{historicPhoto.year}}</b-badge>
                </span>
                <h4>
                  <a :href="historicPhoto.url" target="_blank">{{historicPhoto.title}}</a>
                </h4>
              </template>
              <b-card-img :src="historicPhoto.imageUrl" @click="onThumbnailClick(historicPhoto)"></b-card-img>
              <b-card-title>{{historicPhoto.place}}</b-card-title>
              <b-card-sub-title>{{historicPhoto.currentAddress}}</b-card-sub-title>
              <b-card-text>
                {{historicPhoto.description}}
                <p v-if="historicPhoto.date">
                  <em>{{historicPhoto.date}}</em>
                </p>
              </b-card-text>
            </b-card>
          </b-card-group>
        </div>
      </b-col>
    </b-row>
    <b-modal id="modalFullSizeImage" size="lg" scrollable :title="selectedHistoricPhoto ? selectedHistoricPhoto.title : ''">
      <div v-if="selectedHistoricPhoto" class="text-center">
        <b-img :src="selectedHistoricPhoto.imageUrl"></b-img>
      </div>
    </b-modal>
  </div>
</template>

<script>
import axios from "axios";
import moment from 'moment'

export default {
  name: "HistoricPhotoLocationExplore",
  props: {},
  data() {
    return {
      opacity: 0.4,
      addressData: null,
      locationData: null,
      tileUrl: 'https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png',
      //tileUrl: 'http://a.tile.stamen.com/toner/{z}/{x}/{y}.png',
      zoom: 17,
      center: [43.0417177, -87.9098005],
      // Set this to true to modify overlay coordinates
      editOverlay: false,
      overlay: '',
      overlays: [
        {
          value: '',
          text: 'No Map Overlay',
          url: '/Pixel.png',
          boundE: -87.852897,
          boundW: -88.056214,
          boundN: 43.145601,
          boundS: 42.907076
        },
        {
          value: '1887',
          text: '1887 - Map of Milwaukee',
          url: '/Mke-1887.jpg',
          boundE: -87.865597,
          boundW: -87.960374,
          boundN: 43.078201,
          boundS: 43.000276
        },
        {
          value: '1901',
          text: '1901 - Map of Milwaukee',
          url: '/Mke-1901.jpg',
          boundE: -87.863937,
          boundW: -87.98829,
          boundN: 43.099701,
          boundS: 42.963626
        },
        {
          value: '1912',
          text: '1912 - Map of Milwaukee',
          url: '/Mke-1912.jpg',
          boundE: -87.855597,
          boundW: -87.99109,
          boundN: 43.095801,
          boundS: 42.980526
        },
        {
          value: '1931Transit',
          text: '1931 - Milwaukee Street Car and Bus Routes',
          url: '/Mke-1931-Transit.jpg',
          boundE: -87.852897,
          boundW: -88.056214,
          boundN: 43.145601,
          boundS: 42.907076
        },
        {
          value: '1934',
          text: '1934 - Map of Milwaukee',
          url: '/Mke-1934.jpg',
          boundE: -87.856597,
          boundW: -88.051674,
          boundN: 43.14341,
          boundS: 42.90983
        }
      ],
      bounds: {
        _northEast: {
          lat: 43.04344109368516,
          lng: -87.90466894743822
        },
        _southWest: {
          lat: 43.04001447050734,
          lng: -87.91456093428515
        }
      },
      eras: [
        {
          value: '1880',
          text: 'Before 1880',
          filter: x => x.filter(y => y.date < 1880),
          checked: true
        },
        {
          value: '1880-1900',
          text: '1880-1900',
          filter: x => x.filter(y => y.date >= 1880 && y.date < 1900),
          checked: true
        },
        {
          value: '1900-1920',
          text: '1900-1920',
          filter: x => x.filter(y => y.date >= 1900 && y.date < 1920),
          checked: true
        },
        {
          value: '1920-1960',
          text: '1920-1960',
          checked: true
        },
        {
          value: '>=1960',
          text: 'After 1960',
          checked: true
        }
      ],
      attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors',
      markers: [],
      items: [],
      selectedItem: null,
      selectedHistoricPhoto: null
    }
  },
  computed: {
    selectedOverlay () {
      return this.overlays.find(x => x.value == this.overlay);
    },
    filteredHistoricPhotos () {
      if (!this.selectedItem || !this.selectedItem.historicPhotos)
        return [];

      return this.selectedItem.historicPhotos;

      // let retVal = [];
      // this.eras.filter(era => era.checked).forEach(era => retVal.push(era.filter(selectedItem.historicPhotos)));

      // return retVal;
    }
  },
  methods: {
    zoomUpdated (zoom) {
      this.zoom = zoom;
    },
    centerUpdated (center) {
      this.center = center;
    },
    boundsUpdated (bounds) {
      this.bounds = bounds;
      this.refreshItems();
    },
    onMarkerClick (itemId) {
      this.selectedItem = this.items.find(x => x.id == itemId);
    },
    onThumbnailClick (historicPhoto) {
      this.selectedHistoricPhoto = historicPhoto;
      this.$bvModal.show('modalFullSizeImage');
    },
    refreshItems () {
      if (!this.bounds)
        return;

      let url ='/api/historicPhotoLocation?includes=historicPhotos&limit=200';

      url += 
        '&northBound=' + this.bounds._northEast.lat +
        '&southBound=' + this.bounds._southWest.lat +
        '&eastBound=' + this.bounds._northEast.lng +
        '&westBound=' + this.bounds._southWest.lng;

      axios
        .get(url)
        .then(response => {
          this.items = response.data;

          this.markers = [];
          this.items.forEach(item => {

          if (!item || !item.geometry || !item.geometry.coordinates)
            return null;

            let geometry = {
              lat: item.geometry.coordinates[1],
              lng: item.geometry.coordinates[0]
            }

            let icon = 'https://maps.google.com/mapfiles/kml/paddle/wht-blank.png';

            this.markers.push({
              id: item.id,
              icon: L.icon({
                iconUrl: icon,
                iconSize: [40, 40],
                iconAnchor: [20, 0]
              }),
              position: geometry,
            });
          });

        })
        .catch(error => {
          console.log(error);
        });

    }
  },
  watch: {
    locationData: function (newValue, oldValue) {
      console.log(newValue);
      this.center = {
        lat: newValue.lat,
        lng: newValue.lng
      }
    }
  },
  mounted () {
    this.refreshItems();
  }  
};
</script>