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
        <p class="small">This list contains historic photos from the <a href="http://www.mpl.org/special_collections/images/index.php?slug=milwaukee-historic-photos" target="_blank">Milwaukee
          Public Library's historic photos collection</a>.
        </p>
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <div style="height: 80vh; width: 100%;">
          <l-map style="height: 100%; width: 100%" :zoom="zoom" :center="center" @update:zoom="zoomUpdated" @update:center="centerUpdated" @update:bounds="boundsUpdated">
            <l-tile-layer :url="tileUrl" :attribution="attribution"></l-tile-layer>
            <l-marker v-for="marker in markers" v-bind:key="marker.id" :lat-lng="marker.position" :icon="marker.icon" @click="onMarkerClick(marker.id)">
            </l-marker>
          </l-map>
        </div>
      </b-col>
      <b-col>
        <div v-if="!selectedItem">
        </div>
        <div v-if="selectedItem">
          <b-card-group columns>
            <b-card v-for="(historicPhoto, index) in selectedItem.historicPhotos" v-bind:key="index" :img-src="historicPhoto.imageUrl">
              <template v-slot:header>
                <h4>{{historicPhoto.title}}</h4>
              </template>
                <b-card-title>{{historicPhoto.place}}</b-card-title>
                <b-card-sub-title>{{historicPhoto.currentAddress}}</b-card-sub-title>
                <b-card-text>
                  {{historicPhoto.description}}
                  {{historicPhoto.year}}
                </b-card-text>
                <a :href="historicPhoto.url" target="_blank">View Detail</a>
            </b-card>
          </b-card-group>
        </div>
      </b-col>
    </b-row>
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
      addressData: null,
      locationData: null,
      tileUrl: 'https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png',
      zoom: 17,
      center: [43.0417177, -87.9098005],
      bounds: null,
      attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors',
      markers: [],
      items: [],
      selectedItem: null
    }
  },
  methods: {
    zoomUpdated (zoom) {
      this.zoom = zoom;
      console.log('Zoom: ' + zoom);
    },
    centerUpdated (center) {
      this.center = center;
      console.log('Center: ' + center.lat + ', ' + center.lng);
    },
    boundsUpdated (bounds) {
      console.log(bounds);
      this.bounds = bounds;
      this.refreshItems();
    },
    onMarkerClick (itemId) {
      this.selectedItem = this.items.find(x => x.id == itemId);
    },
    refreshItems () {
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