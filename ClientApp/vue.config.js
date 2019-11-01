module.exports = {
  configureWebpack: {
    devtool: 'source-map'
  },
  pwa: {
    // configure the workbox plugin
    workboxPluginMode: 'InjectManifest',
    workboxOptions: {
        // swSrc is required in InjectManifest mode.
        swSrc: 'src/service-worker.js',
    }
  }    
}