<template>
  <div>
    <page-title title="Unsubscribe" />
  </div>
</template>

<script>
import axios from "axios";
import moment from 'moment'

export default {
  name: "DispatchCallSubscriptionUnsubscribe",
  props: [],
  data() {
    return {
      subscriptionId: null,
      applicationUserId: null,
      hash: null
    };
  },
  computed: {
  },
  methods: {
    load: function () {
      this.subscriptionId = this.$route.query.subscriptionId;
      this.applicationUserId = this.$route.query.applicationUserId;
      this.hash = this.$route.query.hash;

      axios
        .post('/api/dispatchCallSubscription/unsubscribe', {
          subscriptionId: this.subscriptionId,
          applicationUserId: this.applicationUserId,
          hash: this.hash
        })
        .then(response => {
          this.$bvToast.toast('You will no longer receive these notifications.', {
            title: 'Unsubscribed',
            autoHideDelay: 5000,
            variant: 'success'
          });

          this.$router.push('/');
        })
        .catch(error => {
          console.log(error);
        });
    },
  },
  async mounted () {
    this.load();
  }
};
</script>