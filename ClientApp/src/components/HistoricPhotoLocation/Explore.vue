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
        <b-form-select v-model="overlay" :options="overlays" class="w-75"></b-form-select>
        <b-form-spinbutton v-model="opacity" min="0" max="1" step="0.1" inline class="w-25"></b-form-spinbutton>
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
          {{overlay}}
          {{selectedOverlay}}
          <div v-if="selectedOverlay">
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
            <b-card v-for="(historicPhoto, index) in selectedItem.historicPhotos" v-bind:key="index">
              <template v-slot:header>
                <h4><a :href="historicPhoto.url" target="_blank">{{historicPhoto.title}}</a></h4>
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
      opacity: 0.2,
      addressData: null,
      locationData: null,
      tileUrl: 'https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png',
      zoom: 17,
      center: [43.0417177, -87.9098005],
      overlay: '1931Transit',
      overlays: [
        {
          value: '',
          text: 'None'
        },
        {
          value: '1931Transit',
          text: '1931 - Street Car and Bus Routes',
          url: '/Mke-1931-Transit.jpg',
          boundE: -87.852897,
          boundW: -88.056214,
          boundN: 43.145601,
          boundS: 42.907076,
        }
      ],
      // Overlay editing
      //boundE: -87.855527,
      //boundW: -88.057924,
      //boundN: 43.144601,
      //boundS: 42.907076,
      // overlayBounds: [
      //   [43.133601, -88.044924],
      //   [42.898076, -87.856327]
      //   // [
      //   //   43.19065313818291,
      //   //   -87.6551432203354
      //   // ],
      //   // [
      //   //   42.84223446764269,
      //   //   -88.28823037853854
      //   // ]
      // ],
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
      // maxBounds: {
      //   _northEast: {
      //     lat: 43.19065313818291,
      //     lng: -87.6551432203354
      //   },
      //   _southWest: {
      //     lat: 42.84223446764269,
      //     lng: -88.28823037853854
      //   }
      // },
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
      console.log('bounds');
      console.log(bounds);
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

      let url ='/api/historicPhotoLocation?includes=historicPhotos&limit=100';

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