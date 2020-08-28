<template>
  <div>
    <page-title title="Developers" />
    <hr />
    
    <h2>Use the API</h2>
    <p>
      This site has an API that is available for use in other applications. The API is freely available and documented with <a href="/swagger">an OpenAPI spec</a>.
      If you use this API in your own application, please follow the restrictions/requests outlined in the API spec.
    </p>
    <b-button href="/swagger">Read API Documentation</b-button>
    <hr />

    <h2>Source Code</h2>
    <p>
      The code for this application is open source and <a href="https://github.com/ChrisP1118/mketools" target="_blank">available on GitHub</a>.
    </p>
    <b-button href="https://github.com/ChrisP1118/mketools">View on GitHub</b-button>
    <hr />

    <h2>Open Source Disclaimers</h2>
    <p>This site uses <a href="http://osm.org/copyright" target="_blank">OpenStreetMap</a> for mapping.</p>
  </div>
</template>

<script>
export default {
  name: 'Developers',
  props: {
  }
}
</script>