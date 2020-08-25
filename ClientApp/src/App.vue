<template>
  <div>

    <b-container fluid>

      <b-navbar toggleable="md" type="dark" variant="primary">

        <b-navbar-toggle target="nav_collapse"></b-navbar-toggle>
        <b-navbar-brand to="/">
          <img src="./assets/MkeAlerts_60_36.png" style="max-height: 36px; max-width: 60px; border: 1px solid white;" alt="MKE Alerts" />
          MKE Alerts
        </b-navbar-brand>

        <b-collapse is-nav id="nav_collapse">

          <b-navbar-nav>
            <b-nav-item to="/policeDispatchCall">Police Calls</b-nav-item>
            <b-nav-item to="/fireDispatchCall">Fire Calls</b-nav-item>
            <b-nav-item to="/crime">Crimes</b-nav-item>
            <b-nav-item to="/pickupDates">Garbage and Recycling</b-nav-item>
            <b-nav-item to="/parcel">Properties</b-nav-item>
            <b-nav-item to="/historicPhotoLocation">Historic Photos</b-nav-item>
            <b-nav-item to="/historicPhotoLocation/explore">Explore</b-nav-item>
          </b-navbar-nav>

          <b-navbar-nav class="ml-auto">
            <b-nav-item right to="/applicationUser" v-if="isSiteAdmin">Users</b-nav-item>
            <b-nav-item right to="/about">About</b-nav-item>
            <b-nav-item right to="/developers">Developers</b-nav-item>
            <b-nav-item-dropdown right v-if="isSiteAdmin">
              <template slot="button-content">
                Debug
              </template>
              <b-dropdown-item href="/hangfire" target="_blank">Hangfire Dashboard</b-dropdown-item>
              <b-dropdown-item href="/swagger" target="_blank">Swagger UI</b-dropdown-item>
            </b-nav-item-dropdown>
            <b-nav-item-dropdown right>
              <template slot="button-content">
                {{$root.$data.authenticatedUser.username ? $root.$data.authenticatedUser.username : 'Not Logged In'}}
              </template>
              <b-dropdown-item to="/login" v-if="!$root.$data.authenticatedUser.username">Log In</b-dropdown-item>
              <b-dropdown-item to="/logout" v-if="$root.$data.authenticatedUser.username">Log Out</b-dropdown-item>
            </b-nav-item-dropdown>
            
          </b-navbar-nav>

        </b-collapse>
      </b-navbar>


      <!-- Ths if/else here clears the keep-alive when a user logs in/out: https://stackoverflow.com/questions/52967418/refresh-pages-in-vue-js-keep-alive-section -->
      <!-- <keep-alive v-if="$root.$data.authenticatedUser.username">
        <router-view :key="$route.fullPath"></router-view>
      </keep-alive>
      <router-view v-else></router-view> -->
      <router-view></router-view>

      <b-row class="mt-3">
        <b-col>
          <hr />
        </b-col>
      </b-row>
      <b-row class="mt-3">
        <b-col>
          <b-alert show variant="info" class="small text-center">
            <div>This is not an official City of Milwaukee website. This site is not affiliated in any way with the City of Milwaukee, Milwaukee County, Milwaukee Police Department, Milwaukee Fire Department, or any other government agency.</div>
          </b-alert>
        </b-col>
      </b-row>
    </b-container>

  </div>
</template>

<script>

export default {
  name: 'app',
  computed: {
    isSiteAdmin: function () {
      return this.$root.$data.authenticatedUser && this.$root.$data.authenticatedUser.roles && this.$root.$data.authenticatedUser.roles.includes('SiteAdmin');
    }
  },
  methods: {
      showToast: function (text, options) {
        this.$bvToast.toast(text, options);
      }
  }
}
</script>
