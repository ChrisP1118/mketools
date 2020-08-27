<template>
  <div>
    <b-row>
      <b-col>
        <b-jumbotron class="text-center" lead="Fun and useful tools built on open data">
          <template v-slot:header>
            <img src="../assets/MkeTools_100_60.png" />
            MKE Tools
          </template>
        </b-jumbotron>
        <div class="text-center">
          <p>This site takes data from several government sources and tries to make it a bit more accessible and useful.</p>
          <hr />
          <p><b-link to="/alerts">Find police and fire dispatch calls near you</b-link></p>
          <p><b-link to="/pickupDates">Get notified before each garbage and recycling pickup day</b-link></p>
          <p><b-link to="/parcel">View property ownership and assessment data</b-link></p>
          <p><b-link to="/historicPhotoLocation/explore">Explore historic photos from Milwaukee's past</b-link></p>
          <p><b-link to="/developers">Build your own application using open data</b-link></p>
        </div>
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
    }
  },
  computed: {
  },
  methods: {
  },
  async mounted () {
  }
};
</script>