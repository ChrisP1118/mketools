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
        <b-form @submit="onSubmit">
          <b-form-group label="User name" label-for="username">
            <b-form-input id="username" v-model="username" required placeholder="User name"></b-form-input>
          </b-form-group>
          <b-form-group label="Password" label-for="password">
            <b-form-input type="password" id="password" v-model="password" required placeholder="Password"></b-form-input>
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

export default {
  name: "Login",
  props: {},
  data() {
    return {
      isAuthenticated: false,
      username: '',
      password: ''
    }
  },
  methods: {
    onSubmit(evt) {
      evt.preventDefault();
      axios.post('/api/Account/Login',
      {
        "userName": this.username,
        "password": this.password
      })
      .then(response => {
        console.log(response);
        if (response.status == 200) {

          axios.defaults.headers.common['Authorization'] = "Bearer " + response.data.jwtToken;
          this.$root.$data.authenticatedUser.username = response.data.userName;
          this.$root.$data.authenticatedUser.id = response.data.id;
          this.$root.$data.authenticatedUser.roles = response.data.roles;

          localStorage.setItem('jwt', response.data.jwtToken);
          localStorage.setItem('username', response.data.userName);
          localStorage.setItem('id', response.data.id);
          localStorage.setItem('roles', response.data.roles);

          if (this.$route.query.redirect)
            this.$router.push(this.$route.query.redirect);
          else
            this.$router.push('/');
        }
      })
      .catch(error => {
        console.log(error);
      });
    }
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