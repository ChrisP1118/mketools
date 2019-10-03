<template>
  <div>
    <page-loading v-if="!item" />
    <page-title v-if="item" :title="pageTitle" />
    <div v-if="item">
      <b-row>
        <b-col xs="12" md="6">
          <b-card class="mt-3">
            <h4 slot="header">Basic</h4>
            {{item.taxkey}}
          </b-card>
          <b-card class="mt-3">
            <h4 slot="header">Owner</h4>
            {{item.owner_name}}
          </b-card>
        </b-col>
        <b-col xs="12" md="6">
          <b-card class="mt-3">
            <h4 slot="header">Property</h4>
            ... to do ...
          </b-card>
          <b-card class="mt-3">
            <h4 slot="header">Assessment</h4>
            ... to do ...
          </b-card>
        </b-col>
      </b-row>
      <b-row class="mt-3">
        <b-col>
          <b-button-toolbar key-nav>
            <b-button-group class="mx-2">
              <b-button type="button" @click="onClose">Close</b-button>
            </b-button-group>
          </b-button-toolbar>
        </b-col>
      </b-row>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "PropertyView",
  props: ['id'],
  data() {
    return {
      item: null
    };
  },
  computed: {
    pageTitle: function () {
      return 'Property: ' + this.item.taxkey;
    }
  },
  methods: {
    load: function () {
      let url = '/api/property/' + this.id;

      axios
        .get(url)
        .then(response => {
          console.log(response);

          // TODO: Check for 200?

          this.item = response.data;
        })
        .catch(error => {
          console.log(error);
        });
    },
    onClose(evt) {
      this.$router.push('/property');
    }
  },
  mounted () {
    this.load();
    // Use this instead of the previous line to test the "Loading" bar
    //window.setTimeout(this.load, 3000);
  }
};
</script>