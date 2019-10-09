import Vue from 'vue';
import Vuex from 'vuex';
import axios from "axios";
import moment from 'moment'

Vue.use(Vuex);

const STATE_UNLOADED = 0;
const STATE_LOADING = 1;
const STATE_LOADED = 2;

export const store = new Vuex.Store({
  state: {
    distances: [
      { text: '1/16 mile', value: 330 },
      { text: '1/8 mile', value: 660 },
      { text: '1/4 mile', value: 1320 },
      { text: '1/2 mile', value: 2640 },
      { text: '1 mile', value: 5280 }      
    ],
    callTypes: [
      { text: 'any police dispatch call', value: 'PoliceDispatchCall' },
      { text: 'any fire dispatch call', value: 'FireDispatchCall' },
      { text: 'any police or fire dispatch call', value: 'AllDispatchCall' },
      { text: 'any major crime or fire call', value: 'MajorCall' }
    ],
    streetReferences: {
      loadState: STATE_UNLOADED,
      streetDirections: [],
      streetNames: [],
      streetTypes: [],    
    },
    geocode: {
      cache: []
    },
    policeDispatchCallTypes: {
      loadState: STATE_UNLOADED,
      values: []
    },
    recentDispatchCalls: {
      loadState: STATE_UNLOADED,
      typesLoaded: 0,
      values: []
    }
  },
  mutations: {
    SET_STREET_REFERENCES_LOAD_STATE(state, loadState) {
      state.streetReferences.loadState = loadState;
    },
    LOAD_STREET_REFERENCES(state, streetReferences) {
      state.streetReferences.streetDirections = streetReferences.streetDirections;
      state.streetReferences.streetNames = streetReferences.streetNames;
      state.streetReferences.streetTypes = streetReferences.streetTypes;

      state.streetReferences.loadState = STATE_LOADED;
    },
    SET_POLICE_DISPATCH_CALL_TYPES_LOAD_STATE(state, loadState) {
      state.policeDispatchCallTypes.loadState = loadState;
    },
    LOAD_POLICE_DISPATCH_CALL_TYPES(state, values) {
      state.policeDispatchCallTypes.values = values;

      state.policeDispatchCallTypes.loadState = STATE_LOADED;
    },
    SET_RECENT_DISPATCH_CALLS_LOAD_STATE(state, loadState) {
      state.recentDispatchCalls.loadState = loadState;
      if (loadState == STATE_UNLOADED)
        state.recentDispatchCalls.typesLoaded = 0;
    },
    ADD_RECENT_DISPATCH_CALLS(state, values) {
      state.recentDispatchCalls.values.push(...values);
      ++state.recentDispatchCalls.typesLoaded;

      if (state.recentDispatchCalls.typesLoaded == 2)
        state.recentDispatchCalls.loadState = STATE_LOADED;
    },
    SET_RECENT_DISPATCH_CALL_MARKER(state, marker) {

    },
    CREATE_GEOCODE_CACHE_ITEM(state, position) {
      let cachedItem = state.geocode.cache.find(x => x.position.lat == position.lat && x.position.lng == position.lng);

      if (!cachedItem)
        state.geocode.cache.push({
          position: position,
          resolves: [],
          rejects: [],
          state: STATE_UNLOADED,
          property: null
        });
    },
    LOAD_GEOCODE_CACHE_ITEM(state, params) {
      let cachedItem = state.geocode.cache.find(x => x.position.lat == params.position.lat && x.position.lng == params.position.lng);

      cachedItem.state = STATE_LOADING;
      cachedItem.resolves.push(params.resolve);
      cachedItem.rejects.push(params.reject);
    },
    UPDATE_GEOCODE_CACHE_ITEM(state, params) {
      let cachedItem = state.geocode.cache.find(x => x.position.lat == params.position.lat && x.position.lng == params.position.lng);

      cachedItem.state = STATE_LOADED;
      cachedItem.property = params.property;
      cachedItem.resolves = [];
      cachedItem.rejects = [];
    }
  },
  actions: {
    loadStreetReferences({ commit }) {
      return new Promise((resolve, reject) => {
        if (this.state.streetReferences.loadState > 0)
          return;

        commit('SET_STREET_REFERENCES_LOAD_STATE', STATE_LOADING);

        axios
          .get('/api/streetReference')
          .then(response => {
            commit(
              'LOAD_STREET_REFERENCES',
              {
                streetDirections: response.data.streetDirections.map(x => { return x == null ? "" : x; }),
                streetNames: response.data.streetNames,
                streetTypes: response.data.streetTypes.map(x => { return x == null ? "" : x; })
              }
            );
            resolve();
          })
          .catch(error => {
            console.log(error);

            reject();
          });
      });
    },
    loadPoliceDispatchCallTypes({ commit }) {
      return new Promise((resolve, reject) => {
        if (this.state.policeDispatchCallTypes.loadState > 0)
          return;

        commit('SET_POLICE_DISPATCH_CALL_TYPES_LOAD_STATE', STATE_LOADING);

        axios
          .get('/api/policeDispatchCallType?limit=1000')
          .then(response => {
            commit('LOAD_POLICE_DISPATCH_CALL_TYPES', response.data);
            resolve();
          })
          .catch(error => {
            console.log(error);

            reject();
          });
      });
    },
    loadRecentDispatchCalls({ commit, getters }) {
      return new Promise((resolve, reject) => {
        if (this.state.recentDispatchCalls.loadState > 0)
          return;

        commit('SET_RECENT_DISPATCH_CALLS_LOAD_STATE', STATE_LOADING);

        let now = moment().subtract(6, 'hours').format('YYYY-MM-DD HH:mm:ss');
        let filter = 'ReportedDateTime%20%3E%3D%20%22' + encodeURIComponent(now) + '%22';
  
        axios
          .get('/api/policeDispatchCall?limit=1000&filter=' + filter)
          .then(response => {
            let x = response.data.filter(i => i.geometry && i.geometry.coordinates && i.geometry.coordinates[0] && i.geometry.coordinates[0][0]);
            let y = x.map(i => {
              let time = moment(i.reportedDateTime).format('llll');
              let fromNow = moment(i.reportedDateTime).fromNow();

              let icon = getters.getPoliceDispatchCallTypeIcon(i.natureOfCall);
              //let icon = 'red-circle.png';

              return {
                type: 'PoliceDispatch',
                id: i.callNumber,
                position: {
                  lat: i.geometry.coordinates[0][0][1],
                  lng: i.geometry.coordinates[0][0][0]
                },
                status: i.status,
                content: '<p style="font-size: 150%; font-weight: bold;">' + i.natureOfCall + '</p>' +
                  i.location + ' (Police District ' + i.district + ')<hr />' +
                  time + ' (' + fromNow + ')<br />' + 
                  '<b><i>' + i.status + '</i></b>',
                icon: 'https://maps.google.com/mapfiles/kml/paddle/' + icon
              };
            });

            commit('ADD_RECENT_DISPATCH_CALLS', y);
            resolve();
          })
          .catch(error => {
            console.log(error);

            reject();
          });

        axios
          .get('/api/fireDispatchCall?limit=1000&filter=' + filter)
          .then(response => {
            let x = response.data.filter(i => i.geometry && i.geometry.coordinates && i.geometry.coordinates[0] && i.geometry.coordinates[0][0]);
            let y = x.map(i => {
              let time = moment(i.reportedDateTime).format('llll');
              let fromNow = moment(i.reportedDateTime).fromNow();

              //let icon = this.getPoliceDispatchCallTypeIcon(i.natureOfCall);
              let icon = 'orange-circle.png';

              return {
                type: 'FireDispatch',
                id: i.cfs,
                position: {
                  lat: i.geometry.coordinates[0][0][1],
                  lng: i.geometry.coordinates[0][0][0]
                },
                disposition: i.disposition,
                content: '<p style="font-size: 150%; font-weight: bold;">' + i.natureOfCall + '</p>' +
                  i.address + (i.apt ? ' APT. #' + i.apt : '') + '<hr />' +
                  time + ' (' + fromNow + ')<br />' + 
                  '<b><i>' + i.disposition + '</i></b>',
                icon: 'https://maps.google.com/mapfiles/kml/paddle/' + icon
              }
            });

            commit('ADD_RECENT_DISPATCH_CALLS', y);
            resolve();
          })
          .catch(error => {
            console.log(error);

            reject();
          });
      });
    },
    setRecentDispatchCallMarker({ commit }) {

    },
    getAddressFromCoordinates(context, position) {
      context.commit('CREATE_GEOCODE_CACHE_ITEM', position);
      let cachedItem = context.state.geocode.cache.find(x => x.position.lat == position.lat && x.position.lng == position.lng);

      if (cachedItem.state == STATE_LOADED) {
        return new Promise(function(resolve, reject) {
          resolve(cachedItem.property);
        });
      }
    
      return new Promise(function(resolve, reject) {
        let makeCall = cachedItem.state == STATE_UNLOADED;
        context.commit('LOAD_GEOCODE_CACHE_ITEM', {
          position:  position,
          resolve: resolve,
          reject: reject
        });

        if (!makeCall)
          return;

        axios
          .get('/api/geocoding/fromCoordinates?latitude=' + position.lat + '&longitude=' + position.lng)
          .then(response => {
              cachedItem.resolves.forEach(r => {
                r(response.data.property);
              });

              context.commit('UPDATE_GEOCODE_CACHE_ITEM', {
                position: position,
                property: response.data.property
              });
            })
          .catch(error => {
            cachedItem.rejects.forEach(r => {
              r();
            });

            context.commit('UPDATE_GEOCODE_CACHE_ITEM', {
              position: position,
              property: null
            });
          });
        });

    }
  },
  getters: {
    getDistanceLabel: state => distance => {
      if (!distance)
        return '';
  
      return state.distances.find(x => x.value == distance).text;
    },  
    getCallTypeLabel: state => callType => {
      if (!callType)
        return '';

      return state.callTypes.find(x => x.value == callType).text;
    },
    getPoliceDispatchCallTypeIcon: state => natureOfCall => {
      let type = state.policeDispatchCallTypes.values.find(x => x.natureOfCall == natureOfCall);

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
    },
    getRecentDispatchCalls: state => type => {
      return state.recentDispatchCalls.values;
    }
  }
})