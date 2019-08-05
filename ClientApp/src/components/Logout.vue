<template>
  <div>
    <page-title title="Log Out" />
    <b-row>
      <b-col>
        <p>You have been logged out.</p>
        <b-link to="/">Continue</b-link>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "Logout",
  props: {},
  data() {
    return {
    }
  },
  methods: {
  },
  mounted () {
    axios.defaults.headers.common['Authorization'] = null;
    this.$root.$data.authenticatedUser.username = null;
    this.$root.$data.authenticatedUser.id = null;
    this.$root.$data.authenticatedUser.roles = null;

    localStorage.removeItem('jwt');
    localStorage.removeItem('username');
    localStorage.removeItem('id');
    localStorage.removeItem('roles');
  }
};
</script>