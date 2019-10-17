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
      The code for this application is open source and <a href="https://github.com/ChrisP1118/mkealerts" target="_blank">available on GitHub</a>.
    </p>
    <b-button href="https://github.com/ChrisP1118/mkealerts">View on GitHub</b-button>
    <hr />
    <h2>Data Sources</h2>
    <p>This site uses data entirely from public sources. These sources include:</p>
    <ul>
      <li><a href="https://data.milwaukee.gov/dataset/parcel-outlines" target="_blank">Milwaukee Open Data: Parcel Polygons</a></li>
      <li><a href="https://data.milwaukee.gov/dataset/wibr" target="_blank">Milwaukee Open Data: WIBR Crime Data</a></li>
      <li><a href="https://data.milwaukee.gov/dataset/mprop" target="_blank">Milwaukee Open Data: Master Property File</a></li>
      <li><a href="https://data.milwaukee.gov/dataset/mai" target="_blank">Milwaukee Open Data: Master Address Index</a></li>
      <li><a href="https://data.milwaukee.gov/dataset/streets" target="_blank">Milwaukee Open Data: Streets</a></li>
      <li><a href="https://itmdapps.milwaukee.gov/MPDCallData/index.jsp?district=All" target="_blank">Milwaukee Police Department Dispatch Calls</a></li>
      <li><a href="https://itmdapps.milwaukee.gov/MilRest/mfd/calls" target="_blank">Milwaukee Fire Department Dispatch Calls</a></li>
    </ul>
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