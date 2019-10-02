<template>
  <div>
    <b-row>
      <b-col>
        <div class="text-center">
          Log in with: 
          <b-button>
            <g-signin-button :params="googleSignInParams" @success="onGoogleSignInSuccess" @error="onGoogleSignInError">
              <font-awesome-icon :icon="{ prefix: 'fab', iconName: 'google' }"/>
              Google
            </g-signin-button>
          </b-button>
          <b-button>
            <fb-signin-button :params="fbSignInParams" @success="onFacebookSignInSuccess" @error="onFacebookSignInError">
              <font-awesome-icon :icon="{ prefix: 'fab', iconName: 'facebook' }"/>
              Facebook
            </fb-signin-button>
          </b-button>
        </div>
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <div class="strike">
           <span>or</span>
        </div>
      </b-col>
    </b-row>
    <b-row>
      <b-col md="3"></b-col>
      <b-col md="6" class="text-center">
        <div v-if="page == 'create'">
          <h2>Sign Up</h2>
          <b-form @submit.prevent="onSignUp">
            <b-form-group>
              <label class="sr-only" for="signUpEmail">Email Address</label>
              <b-form-input v-model="signUpEmail" id="signUpEmail" type="email" placeholder="Email Address" required />
            </b-form-group>
            <b-form-group>
              <label class="sr-only" for="signUpPassword">Password</label>
              <b-form-input v-model="signUpPassword" id="signUpPassword" type="password" placeholder="Password" required />
            </b-form-group>
            <b-form-group>
              <label class="sr-only" for="signUpConfirm">Confirm Password</label>
              <b-form-input v-model="signUpConfirm" id="signUpConfirm" type="password" placeholder="Confirm Password" required />
            </b-form-group>
            <b-button type="submit" variant="primary">Sign Up</b-button>
          </b-form>
          <div>
            <a href="#" @click.prevent="page = 'login'">Already have an account? Log in.</a>
          </div>
          <div>
            <a href="#" @click.prevent="page = 'reset'">Forgot your password? Reset it.</a>
          </div>          
        </div>
        <div v-if="page == 'login'">
          <h2>Log In</h2>
          <b-form @submit.prevent="onLogIn">
            <b-form-group>
              <label class="sr-only" for="logInEmail">Email Address</label>
              <b-form-input v-model="logInEmail" id="logInEmail" type="email" placeholder="Email Address" required />
            </b-form-group>
            <b-form-group>
              <label class="sr-only" for="logInPassword">Password</label>
              <b-form-input v-model="logInPassword" id="logInPassword" type="password" placeholder="Password" required />
            </b-form-group>
            <b-button type="submit" variant="primary">Log In</b-button>
          </b-form>
          <div>
            <a href="#" @click.prevent="page = 'create'">Don't have an account? Sign up.</a>
          </div>
          <div>
            <a href="#" @click.prevent="page = 'reset'">Forgot your password? Reset it.</a>
          </div>          
        </div>
        <div v-if="page == 'reset'">
          <h2>Reset Password</h2>
          <b-form>
            <b-form-group>
              <label class="sr-only" for="EmailAddress">Email Address</label>
              <b-form-input v-model="emailAddress" id="EmailAddress" placeholder="Email Address" type="email" />
            </b-form-group>
            <b-button>Reset Password</b-button>
          </b-form>
          <div>
            <a href="#" @click.prevent="page = 'login'">Already have an account? Log in.</a>
          </div>
          <div>
            <a href="#" @click.prevent="page = 'create'">Don't have an account? Sign up.</a>
          </div>
        </div>
      </b-col>
      <b-col md="3"></b-col>
    </b-row>
  </div>
</template>

<script>
import axios from "axios";
import AuthMixin from '../Mixins/AuthMixin.vue';

export default {
  name: "AuthForm",
  mixins: [AuthMixin],
  props: {},
  data() {
    return {
      page: 'create',
      emailAddress: '',
      password: '',
      confirmPassword: '',
    }
  },
  computed: {
  },
  methods: {

  },
  watch: {
  },
  async mounted () {
  }
};
</script>