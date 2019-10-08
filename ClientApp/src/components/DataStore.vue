<script>
import Vue from "vue";
import axios from "axios";

let ref = {
  policeDispatchCallTypes: {
    state: 0,
    values: [],

    load: function () {
      if (ref.policeDispatchCallTypes.state != 0)
        return;

      ref.policeDispatchCallTypes.state = 1;
      axios
        .get('/api/policeDispatchCallType?limit=1000')
        .then(response => {
          ref.policeDispatchCallTypes.values = response.data;
          ref.policeDispatchCallTypes.state = 2;
        })
        .catch(error => {
          console.log(error);

          ref.policeDispatchCallTypes.state = 0;
        });
    },
    getIcon: function (natureOfCall) {
      // http://kml4earth.appspot.com/icons.html#paddle

      let type = ref.policeDispatchCallTypes.values.find(x => x.natureOfCall == natureOfCall);

      if (!type)
        return 'wht-blank.png';

      if (type.isCritical)
        return 'red-circle.png';

      if (type.isViolent)
        return 'red-blank.png';

      if (type.isProperty)
        return 'orange-blank.png';

      if (type.isDrug)
        return 'purple-blank.png';

      if (type.isTraffic)
        return 'ylw-blank.png';

      return 'wht-blank.png';      
    }
  },
  streetReferences: {
    state: 0,
    streetDirections: [],
    streetNames: [],
    streetTypes: [],

    load: function () {
      if (ref.streetReferences.state != 0)
        return;

      ref.streetReferences.state = 1;
      axios
        .get('/api/streetReference')
        .then(response => {
          ref.streetReferences.streetDirections = response.data.streetDirections.map(x => { return x == null ? "" : x; });
          ref.streetReferences.streetNames = response.data.streetNames;
          ref.streetReferences.streetTypes = response.data.streetTypes.map(x => { return x == null ? "" : x; });

          ref.streetReferences.state = 2;
        })
        .catch(error => {
          console.log(error);

          ref.streetReferences.state = 0;
        });
    }
  },
  geocode: {
    cache: [],

    addressFromCoordinates: function (latitude, longitude) {
      let cachedItem = ref.geocode.cache.find(x => x.latitude == latitude && x.longitude == longitude);

      if (!cachedItem) {
        cachedItem = {
          latitude: latitude,
          longitude: longitude,
          resolves: [],
          rejects: [],
          state: 0,
          property: null
        };
        ref.geocode.cache.push(cachedItem);
      }

      if (cachedItem.property) {
        return new Promise(function(resolve, reject) {
          resolve(cachedItem.property);
        });
      }

      let promise = new Promise(function(resolve, reject) {
        cachedItem.resolves.push(resolve);
        cachedItem.rejects.push(reject);

        if (cachedItem.state == 0) {
          cachedItem.state = 1;
          axios
            .get('/api/geocoding/fromCoordinates?latitude=' + latitude + '&longitude=' + longitude)
            .then(response => {
              console.log(response);
              if (!response.data.property)
                return;

              cachedItem.property = response.data.property;

              cachedItem.resolves.forEach(r => {
                r(cachedItem.property);
              });
              cachedItem.resolves = [];
            })
            .catch(error => {
              cachedItem.rejects.forEach(r => {
                r(error);
              });
              cachedItem.resolves = [];
            });
          }
      });

      return promise;
    }
  },
  // distances: [
  //   { text: '1/16 mile', value: 330 },
  //   { text: '1/8 mile', value: 660 },
  //   { text: '1/4 mile', value: 1320 },
  //   { text: '1/2 mile', value: 2640 },
  //   { text: '1 mile', value: 5280 }
  // ],
  // callTypes: [
  //   { text: 'any police dispatch call', value: 'PoliceDispatchCall' },
  //   { text: 'any fire dispatch call', value: 'FireDispatchCall' },
  //   { text: 'any police or fire dispatch call', value: 'AllDispatchCall' },
  //   { text: 'any major crime or fire call', value: 'MajorCall' }
  // ],
  // getDistanceLabel: function(distance) {
  //   if (!distance)
  //     return '';

  //   return ref.distances.find(x => x.value == distance).text;
  // },
  // getCallTypeLabel: function(callType) {
  //   if (!callType)
  //     return '';

  //   return ref.callTypes.find(x => x.value == callType).text;
  // }
};

export default Vue.observable(ref);
</script>