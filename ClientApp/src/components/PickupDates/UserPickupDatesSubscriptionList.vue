<template>
  <div>
    <div v-if="subscriptions.length == 1">
      <div v-for="subscription in subscriptions" v-bind:key="subscription.Id" class="text-center">
        <a href="#" @click.prevent="selectSubscription(subscription)">
          An email will be sent to {{authUser}} at {{getNotificationTimeLabel(subscription.hoursBefore)}}
          every garbage or recycling pickup day for
          {{subscription.laddr}} {{subscription.sdir}} {{subscription.sname}} {{subscription.stype}}.</a>
        <a href="#" class="small" style="padding-left: 5px;" @click.prevent="deleteSubscription(subscription)">(Delete)</a>
      </div>
    </div>
    <div v-if="subscriptions.length > 1">
      <div class="text-center">
        Emails will be sent to {{authUser}}:
        <div v-for="subscription in subscriptions" v-bind:key="subscription.Id">
          <a href="#" @click.prevent="selectSubscription(subscription)">
            At {{getNotificationTimeLabel(subscription.hoursBefore)}}
            every garbage or recycling pickup day for
            {{subscription.laddr}} {{subscription.sdir}} {{subscription.sname}} {{subscription.stype}}.</a>
          <a href="#" class="small" style="padding-left: 5px;" @click.prevent="deleteSubscription(subscription)">(Delete)</a>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import AuthMixin from '../Mixins/AuthMixin.vue';
import { mapState, mapGetters } from 'vuex'

export default {
  name: "UserSubscriptionList",
  mixins: [AuthMixin],
  props: [
    'subscriptions'
  ],
  data() {
    return {
    }
  },
  computed: {
    ...mapGetters(['getNotificationTimeLabel']),
  },
  methods: {
    deleteSubscription: function (subscription) {
      this.$emit('deleted', subscription);
    },
    selectSubscription: function (subscription) {
      this.$emit('selected', subscription);
    }
  },
  async mounted () {
  }
};
</script>