<template>
  <div>
    <b-row class="mt-3">
      <b-col>
        <h1>{{title}}</h1>
      </b-col>
    </b-row>
  </div>
</template>

<script>
export default {
  name: 'PageTitle',
  props: [ 'title' ],
  watch: {
    // This would be called anytime the value of title changes
    title(newValue, oldValue) {
      console.log('change from ' + oldValue + ' to ' + newValue);
    }
  }
}
</script>