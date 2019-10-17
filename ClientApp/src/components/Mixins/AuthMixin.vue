<script>
import axios from "axios";

export default {
  name: "AuthMixin",
  props: [
    'defaultAuthPage'
  ],
  data() {
    return {
      authPage: null,

      // Sign up form
      signUpEmail: null,
      signUpPassword: null,
      signUpConfirm: null,

      // Log in form
      logInEmail: null,
      logInPassword: null,

      // Reset password request form
      requestPasswordResetEmail: null,
      requestPasswordResetDone: false,

      // Reset password form
      resetPasswordEmail: null,
      resetPasswordToken: null,
      resetPasswordPassword: null,
      resetPasswordConfirm: null,

      // External providers
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
    getAuthenticatedUserId: function () {
      this.refreshAuthenticationData();

      return this.$root.$data.authenticatedUser.id;
    },
    refreshAuthenticationData: function () {
      if (!this.$root.$data.authenticatedUser.id && localStorage.getItem('jwt') != null) {
        // Use the stored credentials
        axios.defaults.headers.common['Authorization'] = "Bearer " + localStorage.getItem('jwt');

        // On the initial page request/load, $root.$data doesn't yet exist -- not sure what to do about that
        // https://forum.vuejs.org/t/accessing-root-instance-data-methods-on-beforeeach-guards/6148/11
        this.$root.$data.authenticatedUser.username = localStorage.getItem('username');
        this.$root.$data.authenticatedUser.id = localStorage.getItem('id');
        this.$root.$data.authenticatedUser.roles = localStorage.getItem('roles');

        this.$emit('authenticated', this.$root.$data.authenticatedUser);
      }
    },
    onGoogleSignInSuccess (googleUser) {
      // `googleUser` is the GoogleUser object that represents the just-signed-in user.
      // See https://developers.google.com/identity/sign-in/web/reference#users
      const profile = googleUser.getBasicProfile();

      // The ID token you need to pass to your backend:
      var id_token = googleUser.getAuthResponse().id_token;

      axios.post('/api/account/loginExternalCredential',
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
      console.log('Google Sign In Error', error);
    },
    onFacebookSignInSuccess (response) {
      FB.api('/me?fields=name,email', user => {
        console.log('ID: ' + user.id);
        console.log('Name: ' + user.name);
        console.log('Email: ' + user.email);
        console.log(user);

        axios.post('/api/account/loginExternalCredential',
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
      console.log('Facebook Sign In Error', error);
    },
    onSignUp: function (evt) {
      axios.post('/api/account/register',
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
      axios.post('/api/account/login',
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
    onRequestPasswordReset: function (evt) {
      axios.post('/api/account/requestPasswordReset',
      {
        "email": this.requestPasswordResetEmail
      })
      .then(response => {
        //this.processAuthResponse(response);
        this.requestPasswordResetDone = true;
      })
      .catch(error => {
        console.log(error);
      });
    },
    onResetPassword: function (evt) {
      axios.post('/api/account/resetPassword',
      {
        "email": this.resetPasswordEmail,
        "token": this.resetPasswordToken,
        "password": this.resetPasswordPassword
      })
      .then(response => {
        this.processAuthResponse(response);
      })
      .catch(error => {
        console.log(error);
      });
    },
    processAuthResponse: function (response) {
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

        this.$emit('authenticated', this.$root.$data.authenticatedUser);
      }
    }
  },
  mounted () {
    if (this.defaultAuthPage)
      this.authPage = this.defaultAuthPage;
    else
      this.authPage = 'create';
  }
};
</script>