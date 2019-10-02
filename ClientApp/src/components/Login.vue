<template>
  <div>
    <page-title title="Log In" />
    <b-row v-if="authUser">
      <b-col>
        <p>You are already logged in as {{$root.$data.authenticatedUser.username}}.</p>
      </b-col>
    </b-row>
    <auth-form v-if="!authUser" @authenticated="onAuthenticated" />
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
    }
  },
  methods: {
    onAuthenticated: function (authenticatedUser) {
      if (this.$route.query.redirect)
        this.$router.push(this.$route.query.redirect);
      else
        this.$router.push('/');
    }
  },
  mounted () {
  }
};
</script>