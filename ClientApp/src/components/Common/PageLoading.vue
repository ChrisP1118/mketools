<template>
  <div>
    <b-row>
      <b-col>
        <div class="text-center">
          <b-spinner label="Spinning"></b-spinner>
          Loading...
        </div>
      </b-col>
    </b-row>
  </div>
</template>

<script>
export default {
  name: 'PageLoading',
  props: {
  }
}
</script>