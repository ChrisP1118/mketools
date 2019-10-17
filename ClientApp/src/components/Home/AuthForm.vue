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
        <div v-if="authPage == 'create'">
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
            <a href="#" @click.prevent="authPage = 'login'">Already have an account? Log in.</a>
          </div>
          <div>
            <a href="#" @click.prevent="authPage = 'requestReset'">Forgot your password? Reset it.</a>
          </div>          
        </div>
        <div v-if="authPage == 'login'">
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
            <a href="#" @click.prevent="authPage = 'create'">Don't have an account? Sign up.</a>
          </div>
          <div>
            <a href="#" @click.prevent="authPage = 'requestReset'">Forgot your password? Reset it.</a>
          </div>          
        </div>
        <div v-if="authPage == 'requestReset'">
          <h2>Request Password Reset</h2>
          <b-alert v-if="requestPasswordResetDone" variant="success" show>An email has been sent to the address you provided with a link to reset your password.</b-alert>
          <b-form @submit.prevent="onRequestPasswordReset">
            <b-form-group>
              <label class="sr-only" for="EmailAddress">Email Address</label>
              <b-form-input v-model="requestPasswordResetEmail" id="EmailAddress" placeholder="Email Address" type="email" />
            </b-form-group>
            <b-button type="submit" variant="primary">Reset Password</b-button>
          </b-form>
          <div>
            <a href="#" @click.prevent="authPage = 'login'">Already have an account? Log in.</a>
          </div>
          <div>
            <a href="#" @click.prevent="authPage = 'create'">Don't have an account? Sign up.</a>
          </div>
        </div>
        <div v-if="authPage == 'reset'">
          <h2>Reset Password</h2>
          <b-form @submit.prevent="onResetPassword">
            <b-form-group>
              <label class="sr-only" for="resetPasswordEmail">Email Address</label>
              <b-form-input v-model="resetPasswordEmail" id="resetPasswordEmail" type="email" placeholder="Email Address" required />
            </b-form-group>
            <b-form-group>
              <label class="sr-only" for="resetPasswordPassword">New Password</label>
              <b-form-input v-model="resetPasswordPassword" id="resetPasswordPassword" type="password" placeholder="Password" required />
            </b-form-group>
            <b-form-group>
              <label class="sr-only" for="resetPasswordConfirm">New Confirm Password</label>
              <b-form-input v-model="resetPasswordConfirm" id="resetPasswordConfirm" type="password" placeholder="Confirm Password" required />
            </b-form-group>
            <b-button type="submit" variant="primary">Reset Password</b-button>
          </b-form>
          <div>
            <a href="#" @click.prevent="authPage = 'login'">Already have an account? Log in.</a>
          </div>
          <div>
            <a href="#" @click.prevent="authPage = 'create'">Don't have an account? Sign up.</a>
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
    }
  },
  computed: {
  },
  methods: {

  },
  watch: {
  },
  mounted () {
    if (this.$route.query.token) {
      console.log('Resetting');
      this.resetPasswordToken = this.$route.query.token;
      this.authPage = 'reset';
    }
  }
};
</script>