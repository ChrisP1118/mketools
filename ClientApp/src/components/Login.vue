<template>
  <div>
    <page-title title="Log In" />
    <b-row v-if="isAuthenticated">
      <b-col>
        <p>You are already logged in as {{$root.$data.authenticatedUser.username}}.</p>
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <b-form @submit="onLogIn">
          <b-form-group label="Email Address" label-for="logInEmail">
            <b-form-input v-model="logInEmail" id="logInEmail" type="email" placeholder="Email Address" required />
          </b-form-group>
          <b-form-group label="Password" label-for="logInPassword">
            <b-form-input v-model="logInPassword" id="logInPassword" type="password" placeholder="Password" required />
          </b-form-group>
          <b-button type="submit" variant="primary">Log In</b-button>
        </b-form>
      </b-col>
      <b-col>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import axios from "axios";
import AuthMixin from './Mixins/AuthMixin.vue';

export default {
  name: "Login",
  mixins: [AuthMixin],
  props: {},
  data() {
    return {
      isAuthenticated: false,
      username: '',
      password: ''
    }
  },
  methods: {
  },
  mounted () {
    if (localStorage.getItem('jwt') != null) {
      // Use the stored credentials
      axios.defaults.headers.common['Authorization'] = "Bearer " + localStorage.getItem('jwt');

      // On the initial page request/load, $root.$data doesn't yet exist -- not sure what to do about that
      // https://forum.vuejs.org/t/accessing-root-instance-data-methods-on-beforeeach-guards/6148/11
      this.$root.$data.authenticatedUser.username = localStorage.getItem('username');
      this.$root.$data.authenticatedUser.id = localStorage.getItem('id');
      this.$root.$data.authenticatedUser.roles = localStorage.getItem('roles');

      if (this.$route.query.redirect)
        this.$router.push(this.$route.query.redirect);
      else
        this.$router.push('/');
        
    }

    axios
      .get('/api/Account/Test')
      .then(response => {
        console.log(response);

        this.isAuthenticated = response.status == 200;
      })
      .catch(error => {
        console.log(error);

        this.isAuthenticated = false;
      });
  }
};
</script>