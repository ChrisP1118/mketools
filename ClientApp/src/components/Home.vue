<template>
  <div>
    <b-row>
      <b-col>
        <b-jumbotron class="text-center">
          <template v-slot:header>
            <img src="../assets/MkeTools_100_60.png" />
            MKE Tools
          </template>
          <template v-slot:lead>
            Fun, free, and useful tools for Milwaukee residents<br />
            <span class="text-muted">Built using open data</span>
          </template>
        </b-jumbotron>
      </b-col>
    </b-row>

    <b-row>
      <b-col>

        <b-card-group deck>
          <b-card v-for="item in items" v-bind:key="item.name" class="text-center">
            <b-link :to="item.url">
              <b-card-img :src="getImage(item.image)" alt="Image" bottom></b-card-img>
            </b-link>
            <h4 class="mt-3">
              <b-link :to="item.url">{{item.name}}</b-link>
            </h4>
            <b-card-text>
              {{item.description}}
            </b-card-text>
            <!-- <template v-slot:footer>
              <small class="text-muted">Last updated 3 mins ago</small>
            </template> -->
          </b-card>
        </b-card-group>
  
      </b-col>
    </b-row>
  </div>
</template>

<script>

export default {
  name: "Home",
  props: {},
  data() {
    return {
      items: [
        {
          name: 'MKE Alerts',
          description: 'See and get notified of police and fire dispatch calls in Milwaukee',
          image: 'MkeAlerts',
          url: '/alerts'
        },
        {
          name: 'MKE Trash Day',
          description: 'Get notified before garbage and recycling pickup days in Milwaukee',
          image: 'MkeTrashDay',
          url: '/pickupDates'
        },
        {
          name: 'MKE Properties',
          description: 'Explore public assessment and ownership data for properties in Milwaukee',
          image: 'MkeProperties',
          url: '/parcel'
        },
        {
          name: 'MKE Historic Photos',
          description: 'Explore Milwaukee Public Library\'s historic photo collection',
          image: 'MkeHistoricPhotos',
          url: '/historicPhotoLocation/explore'
        },
        {
          name: 'MKE Tools',
          description: 'Build your own application using open data',
          image: 'MkeTools',
          url: '/developers'
        },

      ]
    }
  },
  computed: {
  },
  methods: {
    getImage: function (image) {
      return require('../assets/' + image + '_512_307.png');
    }
  },
  async mounted () {
  }
};
</script>