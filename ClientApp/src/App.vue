<template>
  <div>

    <b-container fluid>

      <b-navbar toggleable="md" type="dark" variant="primary">

        <b-navbar-toggle target="nav_collapse"></b-navbar-toggle>
        <b-navbar-brand :to="brandRoot">
          <img :src="getImage(brandImage)" :alt="brandName" />
          <span class="navbar-brand-name">
            {{brandName}}
          </span>
        </b-navbar-brand>

        <b-collapse is-nav id="nav_collapse">

          <b-navbar-nav>
            <b-nav-item-dropdown text="Alerts">
              <b-dropdown-item to="/alerts">Alerts</b-dropdown-item>
              <b-dropdown-item to="/policeDispatchCall">Police Calls</b-dropdown-item>
              <b-dropdown-item to="/fireDispatchCall">Fire Calls</b-dropdown-item>
              <b-dropdown-item to="/crime">Crimes</b-dropdown-item>
            </b-nav-item-dropdown>
            <b-nav-item to="/pickupDates">Trash Day</b-nav-item>
            <b-nav-item to="/parcel">Properties</b-nav-item>
            <b-nav-item to="/historicPhotoLocation/explore">Historic Photos</b-nav-item>
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
            <div>This is not an official City of Milwaukee website. This site is not affiliated in any way with the City of Milwaukee, Milwaukee County, Milwaukee Police Department, Milwaukee Fire Department, Milwaukee Public Library, or any other government agency.</div>
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
    },
    brand: function () {
      let retVal = 'Tools';

      if (this.$route.matched.length > 0 && this.$route.matched[0].meta.brand)
        retVal = this.$route.matched[0].meta.brand;

      return retVal;
    },
    brandName: function () {
      switch (this.brand) {
        case 'Alerts': return 'MKE Alerts';
        case 'TrashDay': return 'MKE Trash Day';
        case 'Properties': return 'MKE Properties';
        case 'HistoricPhotos': return 'MKE Historic Photos';
        default: return 'MKE Tools'
      }
    },
    brandImage: function () {
      return 'Mke' + this.brand + '_60_36.png';
    },
    brandRoot: function () {
      switch (this.brand) {
        case 'Alerts': return '/alerts';
        case 'TrashDay': return '/pickupDates';
        case 'Properties': return '/parcel';
        case 'HistoricPhotos': return '/historicPhotoLocation/explore';
        default: return '/'
      }

    }
  },
  methods: {
    showToast: function (text, options) {
      this.$bvToast.toast(text, options);
    },
    getImage: function (file) {
      return require('./assets/' + file);
    }
  }
}
</script>
