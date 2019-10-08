import Vue from 'vue';
import Vuex from 'vuex';
import axios from "axios";

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
    }
  },
  actions: {
    loadStreetReferences({ commit }) {
      return new Promise((resolve, reject) => {
        console.log()
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
    }
  }
})