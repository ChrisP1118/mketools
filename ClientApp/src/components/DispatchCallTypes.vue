<template>
  <div>
    <page-title title="Dispatch Call Types" />
    <hr />

    <h2>Police Dispatch Call Types</h2>
    <p>This table shows how police dispatch calls are classified based on the nature of the call. "Is Major" and "Is Minor" are the fields that determine what level 
      of notification email to send. This is initially based on <a href="https://city.milwaukee.gov/ImageLibrary/Groups/mpdAuthors/SOP/COMMUNICATIONS-2501.pdf">MPS's
      Standard Operating Procedures for Communications document</a>, which lists suggestions on the number of personnel to be dispatched based on the call type. I've
      made a few modifications to it - if you can help clarify some of these, get in touch.</p>
    <b-table-lite :items="policeDispatchCallTypes.values">
    </b-table-lite>

    <h2>Fire Dispatch Call Types</h2>
    <p>This table shows how fire calls are classified based on the nature of the call.</p>
    <b-table-lite :items="fireDispatchCallTypes.values">
    </b-table-lite>

  </div>
</template>

<script>
import { mapState, mapGetters, mapActions } from 'vuex'

export default {
  name: 'DispatchCallTypes',
  props: {
  },
  computed: {
    ...mapState(['policeDispatchCallTypes', 'fireDispatchCallTypes']),
  },
  mounted () {
    this.$store.dispatch("loadPoliceDispatchCallTypes");
    this.$store.dispatch("loadFireDispatchCallTypes");
  }
}
</script>