<template>
  <div>
    <div class="map" id="basicMap" />
  </div>
</template>

<script>
import gmapsInit from './Common/googlemaps';

export default {
  name: "BasicMap",
  mixins: [],
  props: {},
  data() {
    return {
      addressData: null,
      locationData: null,

      // Mapping
      mapCacheViews: [
        { text: 'All', value: 'a' },
        { text: 'Active Police Calls', value: 'ap' },
        { text: 'Recent Police Calls', value: 'rp' },
        { text: 'Active Fire Calls', value: 'af' },
        { text: 'Recent Fire Calls', value: 'rf' },
      ],
      google: null,
      map: null,
      bounds: null,
      tabKey: 'a',
      visibleMarkerCaches: ['rp', 'rf'],
      markerCache: {
        ap: [],
        rp: [],
        af: [],
        rf: []
      },
      markerCacheBounds: {
        ap: null,
        rp: null,
        af: null,
        rf: null
      },
      markerWrappers: [],
      mapFull: false,
      mapItemLimit: 100,

      // Notifications
      distance: 660,
      distances: [
        { text: '1/16 mile', value: 330 },
        { text: '1/8 mile', value: 660 },
        { text: '1/4 mile', value: 1320 },
        { text: '1/2 mile', value: 2640 },
        { text: '1 mile', value: 5280 }
      ],
      callType: 'MajorCall',
      callTypes: [
        { text: 'any police dispatch call', value: 'PoliceDispatchCall' },
        { text: 'any fire dispatch call', value: 'FireDispatchCall' },
        { text: 'any police or fire dispatch call', value: 'AllDispatchCall' },
        { text: 'any major crime or fire call', value: 'MajorCall' }
      ],
      circle: null,

      subscriptions: [],
    }
  },
  computed: {
  },
  methods: {
    updateTab: function (tabKey) {
      if (!tabKey)
        tabKey = this.tabKey;

      if (tabKey == 'ap') {
        this.visibleMarkerCaches = ['ap']
        this.loadActivePoliceCalls();
      } else if (tabKey == 'rp') {
        this.visibleMarkerCaches = ['rp']
        this.loadRecentPoliceCalls();
      } else if (tabKey == 'af') {
        this.visibleMarkerCaches = ['af']
        this.loadActiveFireCalls();
      } else if (tabKey == 'rf') {
        this.visibleMarkerCaches = ['rf']
        this.loadRecentFireCalls();
      } else if (tabKey == 'a') {
        this.visibleMarkerCaches = ['rp', 'rf']
        this.loadRecentPoliceCalls();
        this.loadRecentFireCalls();
      }
    },
    loadActivePoliceCalls: function () {
      let now = moment().subtract(6, 'hours').format('YYYY-MM-DD HH:mm:ss');
      this.loadPoliceDispatchCalls('ap', 'Status%20%3D%20%22Service%20in%20Progress%22%20and%20ReportedDateTime%20%3E%3D%20%22' + encodeURIComponent(now) + '%22' + this.getBoundsFilter())
    },
    loadRecentPoliceCalls: function () {
      let now = moment().subtract(6, 'hours').format('YYYY-MM-DD HH:mm:ss');
      this.loadPoliceDispatchCalls('rp', 'ReportedDateTime%20%3E%3D%20%22' + encodeURIComponent(now) + '%22' + this.getBoundsFilter())
    },
    loadPoliceDispatchCalls: function (cacheKey, filter) {
      if (!this.areBoundsCached(cacheKey)) {
        this.showMarkers();
      } else {
        axios
          .get('/api/PoliceDispatchCall?offset=0&limit=' + this.mapItemLimit + '&order=ReportedDateTime%20desc&filter=' + filter)
          .then(response => {
            response.data.forEach(i => {
              if (this.markerCache[cacheKey].find(x => x.id == i.CallNumber))
                return;

              let time = moment(i.ReportedDateTime).format('llll');
              let fromNow = moment(i.ReportedDateTime).fromNow();

              // http://kml4earth.appspot.com/icons.html#paddle
              let icon = 'wht-blank.png';
              switch (i.NatureOfCall) {

                // Red: Violent crime
                case 'BATTERY':
                case 'FIGHT':
                case 'BATTERY DV':
                case 'HOLDUP ALARM':
                  icon = 'red-blank.png'; break;

                case 'SHOTSPOTTER':
                case 'SHOOTING':
                case 'SHOTS FIRED':
                  icon = 'red-circle.png'; break;

                // Orange: Non-violent serious crime
                case 'THEFT': 
                case 'DRUG DEALING':
                case 'OVERDOSE':
                case 'STOLEN VEHICLE':
                case 'ENTRY':
                case 'ENTRY TO AUTO':
                case 'PROPERTY DAMAGE':
                  icon ='orange-blank.png'; break;

                // Blue: Traffic
                case 'TRAFFIC STOP':
                  icon = 'blu-blank.png'; break;
              }

              this.markerCache[cacheKey].push({
                id: i.CallNumber,
                geometry: i.Geometry,
                content: '<p style="font-size: 150%; font-weight: bold;">' + i.NatureOfCall + '</p>' +
                  i.Location + ' (Police District ' + i.District + ')<hr />' +
                  time + ' (' + fromNow + ')<br />' + 
                  '<b><i>' + i.Status + '</i></b>',
                icon: 'https://maps.google.com/mapfiles/kml/paddle/' + icon,
                marker: null,
                state: 'Hidden'
              })
            });

            this.markerCacheBounds[cacheKey] = this.bounds;
            this.showMarkers();
          })
          .catch(error => {
            console.log(error);
          });
      }
    },
    loadActiveFireCalls: function () {
      let now = moment().subtract(6, 'hours').format('YYYY-MM-DD HH:mm:ss');
      this.loadFireDispatchCalls('af', 'Disposition%20%3D%20%22ACTIVE%22%20and%20ReportedDateTime%20%3E%3D%20%22' + encodeURIComponent(now) + '%22' + this.getBoundsFilter())
    },
    loadRecentFireCalls: function () {
      let now = moment().subtract(6, 'hours').format('YYYY-MM-DD HH:mm:ss');
      this.loadFireDispatchCalls('rf', 'ReportedDateTime%20%3E%3D%20%22' + encodeURIComponent(now) + '%22' + this.getBoundsFilter())
    },
    loadFireDispatchCalls: function (cacheKey, filter) {
      if (!this.areBoundsCached(cacheKey)) {
        this.showMarkers();
      } else {
        axios
          .get('/api/FireDispatchCall?offset=0&limit=' + this.mapItemLimit + '&order=ReportedDateTime%20desc&filter=' + filter)
          .then(response => {

            response.data.forEach(i => {
              if (this.markerCache[cacheKey].find(x => x.id == i.CFS))
                return;

              let time = moment(i.ReportedDateTime).format('llll');
              let fromNow = moment(i.ReportedDateTime).fromNow();

              // http://kml4earth.appspot.com/icons.html#paddle
              let icon = 'wht-blank.png';

              if (i.NatureOfCall == 'EMS')
                icon = 'orange-blank.png';
              else if (i.NatureOfCall.includes('Fire'))
                icon = 'red-blank.png';

              this.markerCache[cacheKey].push({
                id: i.CFS,
                geometry: i.Geometry,
                content: '<p style="font-size: 150%; font-weight: bold;">' + i.NatureOfCall + '</p>' +
                  i.Address + (i.Apt ? ' APT. #' + i.Apt : '') + '<hr />' +
                  time + ' (' + fromNow + ')<br />' + 
                  '<b><i>' + i.Disposition + '</i></b>',
                icon: 'https://maps.google.com/mapfiles/kml/paddle/' + icon,
                marker: null,
                state: 'Hidden'
              })
            });

            this.markerCacheBounds[cacheKey] = this.bounds;
            this.showMarkers();
          })
          .catch(error => {
            console.log(error);
          });
      }
    },
    areBoundsCached: function (cacheKey) {
      let cacheBounds = this.markerCacheBounds[cacheKey];
      let loadBounds = this.bounds;

      return cacheBounds == null ||
        loadBounds.ne.lng > cacheBounds.ne.lng ||
        loadBounds.sw.lng < cacheBounds.sw.lng ||
        loadBounds.ne.lat > cacheBounds.ne.lat ||
        loadBounds.sw.lat < cacheBounds.sw.lat;
    },
    showMarkers: function () {
      if (!google)
        return;

      for (let cacheKey in this.markerCache) {
        let cache = this.markerCache[cacheKey];
        cache.forEach(markerDetail => {
          if (markerDetail.state == 'Visible')
            markerDetail.state = 'Hide';
        });
      };

      this.visibleMarkerCaches.forEach(cacheKey => {
        this.markerCache[cacheKey].forEach(markerDetail => {

          if (!markerDetail.geometry)
            return;

          if (!markerDetail.geometry || !markerDetail.geometry.coordinates || !markerDetail.geometry.coordinates[0] || !markerDetail.geometry.coordinates[0][0])
            return;

          if (markerDetail.marker) {
            if (markerDetail.state == 'Hide') {
              markerDetail.state = 'Visible';
            } else if (markerDetail.state == 'Hidden') {
              markerDetail.state = 'Visible';
              markerDetail.marker.setMap(this.map);
            }

            return;
          }

          markerDetail.state = 'Visible';
          
          let point = markerDetail.geometry.coordinates[0][0];

          markerDetail.marker = new google.maps.Marker({
            position: {
              lat: point[1],
              lng: point[0]
            },
            icon: {
              url: markerDetail.icon,
              scaledSize: new google.maps.Size(50, 50),
            },
            map: this.map
          });

          markerDetail.marker.addListener('click', e => {
            if (this.openInfoWindow)
              this.openInfoWindow.close();

            this.openInfoWindow = new google.maps.InfoWindow({
              content: markerDetail.content
            });
            this.openInfoWindow.open(this.map, markerDetail.marker);
          });
        });
      });

      // TODO: Set mapFull
      //this.mapFull = totalCount >= this.mapItemLimit;

      for (let cacheKey in this.markerCache) {
        let cache = this.markerCache[cacheKey];
        cache.forEach(markerDetail => {
          if (markerDetail.state == 'Hide') {
            markerDetail.state = 'Hidden';
            markerDetail.marker.setMap(null);
          }
        });
      };

    },
    getBoundsFilter: function() {
      if (!this.bounds)
        return '';

      return '&northBound=' + this.bounds.ne.lat + '&southBound=' + this.bounds.sw.lat + '&eastBound=' + this.bounds.ne.lng + '&westBound=' + this.bounds.sw.lng;
    }
  },
  watch: {
    tabKey: function (newValue, oldValue) {
      this.updateTab(newValue);
    },
    locationData: function (newValue, oldValue) {
      this.map.setCenter(newValue);
      this.map.setZoom(15);

      this.updateDistance(this.distance);
    },
  },
  async mounted () {
    //this.loadStreetReferences();

    this.google = await gmapsInit();
    this.map = new google.maps.Map(document.getElementById('basicMap'), {
      center: { lat: 43.0315528, lng: -87.9730566 },
      zoom: 12,
      gestureHandling: 'greedy'
    });

    let boundsChangedTimeout = null;

    this.map.addListener('bounds_changed', e => {

      if (boundsChangedTimeout != null)
        clearTimeout(boundsChangedTimeout);

      boundsChangedTimeout = setTimeout(() => {
        let bounds = this.map.getBounds();
        let ne = bounds.getNorthEast();
        let sw = bounds.getSouthWest();
        this.bounds = {
          ne: {
            lat: ne.lat(),
            lng: ne.lng()
          },
          sw: {
            lat: sw.lat(),
            lng: sw.lng()
          }
        };

        this.updateTab();

      }, 1000);
    });

    this.updateSubscriptions();
  }
};
</script>