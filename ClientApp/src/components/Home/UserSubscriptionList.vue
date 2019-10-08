<template>
  <div>
    <div v-if="subscriptions.length == 1">
      <div v-for="subscription in subscriptions" v-bind:key="subscription.Id" class="text-center">
        <a href="#" @click.prevent="selectSubscription(subscription)">
          An email will be sent to {{authUser}} whenever there's 
          {{getCallTypeLabel(subscription.dispatchCallType)}} 
          within {{getDistanceLabel(subscription.distance)}} 
          of {{subscription.house_nr}} {{subscription.sdir}} {{subscription.street}} {{subscription.sttype}}.</a>
        <a href="#" class="small" @click.prevent="deleteSubscription(subscription)">(Delete)</a>
      </div>
    </div>
    <div v-if="subscriptions.length > 1">
      <div class="text-center">
        Emails will be sent to {{authUser}} whenever there's:
        <div v-for="subscription in subscriptions" v-bind:key="subscription.Id">
          <a href="#" @click.prevent="selectSubscription(subscription)">
            {{getCallTypeLabel(subscription.dispatchCallType)}} 
            within {{getDistanceLabel(subscription.distance)}}
            of {{subscription.house_nr}} {{subscription.sdir}} {{subscription.street}} {{subscription.sttype}}.</a>
          <a href="#" class="small" @click.prevent="deleteSubscription(subscription)">(Delete)</a>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import dataStore from '../DataStore.vue';
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
      dataStore: dataStore
    }
  },
  computed: {
    ...mapGetters(['getCallTypeLabel', 'getDistanceLabel']),
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