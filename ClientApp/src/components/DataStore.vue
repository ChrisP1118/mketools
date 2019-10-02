<script>
import Vue from "vue";
import axios from "axios";

let ref = {
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
        .get('/api/StreetReference')
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
            .get('/api/Geocoding/FromCoordinates?latitude=' + latitude + '&longitude=' + longitude)
            .then(response => {
              console.log(response);
              if (!response.data.Property)
                return;

              cachedItem.property = response.data.Property;

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
  }
};

export default Vue.observable(ref);
</script>