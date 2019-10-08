import Vue from 'vue';
import Vuex from 'vuex';

Vue.use(Vuex);

export const store = new Vuex.Store({
  state: {
    count: 0,
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
    ]  
  },
  mutations: {
    increment (state) {
      state.count++
    }
  },
  actions: {

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