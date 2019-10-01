<script>
import axios from "axios";

export default {
  name: "AuthMixin",
  props: {},
  data() {
    return {
      // Sign up form
      signUpEmail: null,
      signUpPassword: null,
      signUpConfirm: null,

      // Log in form
      logInEmail: null,
      logInPassword: null,
      googleSignInParams: {
        client_id: '66835382455-403e538rnmmpmcp5tocmndleh30g4i5d.apps.googleusercontent.com'
      },
      fbSignInParams: {
        scope: 'email,user_likes',
        return_scopes: true
      }
    }
  },
  computed: {
    authUser: function () {
      return this.$root.$data.authenticatedUser.username;
    }
  },
  methods: {
    onGoogleSignInSuccess (googleUser) {
      // `googleUser` is the GoogleUser object that represents the just-signed-in user.
      // See https://developers.google.com/identity/sign-in/web/reference#users
      const profile = googleUser.getBasicProfile();
      console.log('ID: ' + profile.getId());
      console.log('Full Name: ' + profile.getName());
      console.log('Given Name: ' + profile.getGivenName());
      console.log('Family Name: ' + profile.getFamilyName());
      console.log('Image URL: ' + profile.getImageUrl());
      console.log('Email: ' + profile.getEmail());

      // The ID token you need to pass to your backend:
      var id_token = googleUser.getAuthResponse().id_token;
      console.log("ID Token: " + id_token);

      axios.post('/api/Account/LoginExternalCredential',
      {
        "provider": "Google",
        "externalId": profile.getId(),
        "email": profile.getEmail()
      })
      .then(response => {
        this.processAuthResponse(response);
      })
      .catch(error => {
        console.log(error);
      });      
    },
    onGoogleSignInError (error) {
      // `error` contains any error occurred.
      console.log('OH NOES', error);
    },
    onFacebookSignInSuccess (response) {
      FB.api('/me?fields=name,email', user => {
        console.log('ID: ' + user.id);
        console.log('Name: ' + user.name);
        console.log('Email: ' + user.email);
        console.log(user);

        axios.post('/api/Account/LoginExternalCredential',
        {
          "provider": "Facebook",
          "externalId": user.id,
          "email": user.email
        })
        .then(response => {
          this.processAuthResponse(response);
        })
        .catch(error => {
          console.log(error);
        });        
      });
    },
    onFacebookSignInError (error) {
      console.log('OH NOES', error);
    },
    onSignUp: function (evt) {
      axios.post('/api/Account/Register',
      {
        "email": this.signUpEmail,
        "password": this.signUpPassword
      })
      .then(response => {
        this.processAuthResponse(response);
      })
      .catch(error => {
        console.log(error);
      });
    },
    onLogIn: function (evt) {
      axios.post('/api/Account/Login',
      {
        "email": this.logInEmail,
        "password": this.logInPassword
      })
      .then(response => {
        this.processAuthResponse(response);
      })
      .catch(error => {
        console.log(error);
      });
    },
    processAuthResponse: function (response, checkForRedirect) {
      console.log(response);
      if (response.status == 200) {

        axios.defaults.headers.common['Authorization'] = "Bearer " + response.data.JwtToken;
        this.$root.$data.authenticatedUser.username = response.data.UserName;
        this.$root.$data.authenticatedUser.id = response.data.Id;
        this.$root.$data.authenticatedUser.roles = response.data.Roles;

        localStorage.setItem('jwt', response.data.JwtToken);
        localStorage.setItem('username', response.data.UserName);
        localStorage.setItem('id', response.data.Id);
        localStorage.setItem('roles', response.data.Roles);

        if (!checkForRedirect)
          return;

        if (this.$route.query.redirect)
          this.$router.push(this.$route.query.redirect);
        else
          this.$router.push('/');
      }
    }
  }
};
</script>