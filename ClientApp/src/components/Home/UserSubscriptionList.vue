<template>
  <div>
    <div v-for="subscription in subscriptions" v-bind:key="subscription.Id" class="text-center">
      <a href="#" @click.prevent="selectSubscription(subscription)">An email will be sent to {{authUser}} whenever there's {{dataStore.getCallTypeLabel(subscription.DispatchCallType)}} within {{dataStore.getDistanceLabel(subscription.Distance)}} 
      of {{subscription.HOUSE_NR}} {{subscription.SDIR}} {{subscription.STREET}} {{subscription.STTYPE}}.</a>
      <a href="#" class="small" @click.prevent="deleteSubscription(subscription)">(Delete)</a>
    </div>
  </div>
</template>

<script>
import dataStore from '../DataStore.vue';
import AuthMixin from '../Mixins/AuthMixin.vue';

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