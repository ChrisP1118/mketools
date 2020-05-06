import Vue from 'vue';
import Vuex from 'vuex';
import axios from "axios";
import moment from 'moment'

Vue.use(Vuex);

const STATE_UNLOADED = 0;
const STATE_LOADING = 1;
const STATE_LOADED = 2;

export const store = new Vuex.Store({
  addressData: null,
  locationData: null,
  state: {
    distances: [
      { text: '1/16 mile', value: 330 },
      { text: '1/8 mile', value: 660 },
      { text: '1/4 mile', value: 1320 },
      //{ text: '1/2 mile', value: 2640 },
      //{ text: '1 mile', value: 5280 }      
    ],
    callTypes: [
      { text: 'any major police dispatch call', value: 'JustMajorPoliceDispatchCall'},
      { text: 'any major or minor police dispatch call', value: 'MinorOrMajorPoliceDispatchCall'},
      //{ text: 'any police dispatch call', value: 'AnyPoliceDispatchCall'},
      { text: 'any major fire dispatch call', value: 'JustMajorFireDispatchCall'},
      { text: 'any fire dispatch call', value: 'AnyFireDispatchCall'},
      { text: 'any major police or fire dispatch call', value: 'AnyMajorDispatchCall' }
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
    fireDispatchCallTypes: {
      loadState: STATE_UNLOADED,
      values: []
    },
    notificationTimes: [
      { text: '6am the day before', value: -18 },
      { text: '10am the day before', value: -14 },
      { text: '2pm the day before', value: -10 },
      { text: '6pm the day before', value: -6 },
      { text: '10pm the day before', value: -2 },
      { text: '6am the morning of', value: 6 },
    ]
  },
  mutations: {
    SET_ADDRESS_DATA(state, addressData) {
      state.addressData = addressData;
    },
    SET_LOCATION_DATA(state, locationData) {
      state.locationData = locationData;
    },
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
    SET_FIRE_DISPATCH_CALL_TYPES_LOAD_STATE(state, loadState) {
      state.fireDispatchCallTypes.loadState = loadState;
    },
    LOAD_FIRE_DISPATCH_CALL_TYPES(state, values) {
      state.fireDispatchCallTypes.values = values;

      state.fireDispatchCallTypes.loadState = STATE_LOADED;
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
      cachedItem.address = params.address;
      cachedItem.resolves = [];
      cachedItem.rejects = [];
    }
  },
  actions: {
    loadStreetReferences({ commit }) {
      return new Promise((resolve, reject) => {
        if (this.state.streetReferences.loadState == STATE_LOADED)
          resolve();

        if (this.state.streetReferences.loadState == STATE_LOADING)
          return;

        commit('SET_STREET_REFERENCES_LOAD_STATE', STATE_LOADING);

        axios
          .get('/api/streetReference?municipality=Milwaukee')
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
        if (this.state.policeDispatchCallTypes.loadState == STATE_LOADED)
          resolve();

        if (this.state.policeDispatchCallTypes.loadState == STATE_LOADING)
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
    loadFireDispatchCallTypes({ commit }) {
      return new Promise((resolve, reject) => {
        if (this.state.fireDispatchCallTypes.loadState == STATE_LOADED)
          resolve();

        if (this.state.fireDispatchCallTypes.loadState == STATE_LOADING)
          return;

        commit('SET_FIRE_DISPATCH_CALL_TYPES_LOAD_STATE', STATE_LOADING);

        axios
          .get('/api/fireDispatchCallType?limit=1000')
          .then(response => {
            commit('LOAD_FIRE_DISPATCH_CALL_TYPES', response.data);
            resolve();
          })
          .catch(error => {
            console.log(error);

            reject();
          });
      });
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
                r(response.data.commonParcel.parcels[0]);
              });

              context.commit('UPDATE_GEOCODE_CACHE_ITEM', {
                position: position,
                parcel: response.data.commonParcel.parcels[0]
              });
            })
          .catch(error => {
            cachedItem.rejects.forEach(r => {
              r();
            });

            context.commit('UPDATE_GEOCODE_CACHE_ITEM', {
              position: position,
              parcel: null
            });
          });
        });

    },
    setAddressData(context, addressData) {
      context.commit('SET_ADDRESS_DATA', addressData);
    },
    setLocationData(context, locationData) {
      context.commit('SET_LOCATION_DATA', locationData);
    }
  },
  getters: {
    getDistanceLabel: state => distance => {
      if (!distance)
        return '';

      let x = state.distances.find(x => x.value == distance);
      if (x)  
        return x.text;

      return distance + ' feet';
    },  
    getCallTypeLabel: state => callType => {
      if (!callType)
        return '';

      return state.callTypes.find(x => x.value == callType).text;
    },
    getNotificationTimeLabel: state => notificationTime => {
      if (!notificationTime)
        return '';

      return state.notificationTimes.find(x => x.value == notificationTime).text;
    },
    getPoliceDispatchCallTypeIcon: state => natureOfCall => {
      let type = state.policeDispatchCallTypes.values.find(x => x.natureOfCall == natureOfCall);

      if (!type)
        return 'https://maps.google.com/mapfiles/kml/paddle/wht-blank.png';

      if (type.isCritical)
        return 'https://maps.google.com/mapfiles/kml/paddle/red-circle.png';

      if (type.isViolent)
        return 'https://maps.google.com/mapfiles/kml/paddle/red-blank.png';

      if (type.isProperty)
        return 'https://maps.google.com/mapfiles/kml/paddle/orange-blank.png';

      if (type.isDrug)
        return 'https://maps.google.com/mapfiles/kml/paddle/purple-blank.png';

      if (type.isTraffic)
        return 'https://maps.google.com/mapfiles/kml/paddle/ylw-blank.png';

      return 'https://maps.google.com/mapfiles/kml/paddle/wht-blank.png';
    },
    getFireDispatchCallTypeIcon: state => natureOfCall => {
      let type = state.fireDispatchCallTypes.values.find(x => x.natureOfCall == natureOfCall);

      if (!type)
        return 'https://maps.google.com/mapfiles/kml/paddle/wht-blank.png';

      if (type.isCritical)
        return 'https://maps.google.com/mapfiles/kml/paddle/red-square.png';

      if (type.isFire)
        return 'https://maps.google.com/mapfiles/kml/paddle/red-blank.png';

      if (type.isMedical)
        return 'https://maps.google.com/mapfiles/kml/paddle/orange-blank.png';

      return 'https://maps.google.com/mapfiles/kml/paddle/wht-blank.png';
    },
    getGeometryPosition: state => geometry => {
      if (!geometry)
        return null;
        
      if (geometry.type == 'Point') {
        if (!geometry.coordinates)
          return null;
          
        return {
          lat: geometry.coordinates[1],
          lng: geometry.coordinates[0]
        };
      } else if (geometry.type == 'Polygon') {
        if (!geometry.coordinates || !geometry.coordinates[0] || !geometry.coordinates[0][0])
          return null;

        return {
          lat: geometry.coordinates[0][0][1], 
          lng: geometry.coordinates[0][0][0]
        };
      }
    },
    getAddressData: state => () => {
      return state.addressData;
    },
    getLocationData: state => () => {
      return state.locationData;
    },
    getPropertyInfoWindow: state => property => {
      let address = property.house_nr_lo;
      if (property.house_nr_hi != property.house_nr_lo)
        address += '-' + property.house_nr_hi;
      address += ' ' + property.sdir + ' ' + property.street + ' ' + property.sttype;

      let owner = property.owner_name_1;
      if (property.owner_name_2)
        owner += '<br />' + property.owner_name_2;
      if (property.owner_name_3)
        owner += '<br />' + property.owner_name_3;
      owner += '<br />' + property.owner_mail_addr + '<br />' + property.owner_city_state;

      let full = '<div style="font-size: 125%; font-weight: bold;"><a href="#/property/' + property.taxkey + '">' + address + '</a></div><div>' + owner + '</div>';
      
      return full;
    },
    getCommonParcelInfoWindow: (state, getters) => commonParcel => {
      if (commonParcel.parcels.length == 1) {
        return getters.getPropertyInfoWindow(commonParcel.parcels[0].property);
      } else {
        let options = [];
        commonParcel.parcels.forEach(i => {
          let propertyInfo = getters.getPropertyInfoWindow(i.property);

          let address = i.property.house_nr_lo;
          if (i.property.house_nr_hi != i.property.house_nr_lo)
            address += '-' + i.property.house_nr_hi;
          address += ' ' + i.property.sdir + ' ' + i.property.street + ' ' + i.property.sttype;

          options.push('<option data-text="' + encodeURIComponent(propertyInfo) + '">' + address + '</option>');
        });

        let retVal = '<select onchange="this.nextSibling.innerHTML = decodeURIComponent(this.querySelector(\':checked\').getAttribute(\'data-text\'))">' + options.join('') + '</select><div>' + getters.getPropertyInfoWindow(commonParcel.parcels[0].property) + '</div>';
        
        if (commonParcel.parcels[0].condoName)
          retVal = '<div style="font-size: 125%; font-weight: bold;">' + commonParcel.parcels[0].condoName + '</div>' + retVal;

        return retVal;
      }
    },
    getPropertyPolygonColor: state => property => {
      return '#333333';
    },
    getCommonParcelPolygonColor: state => commonParcel => {
      return '#333333';
    },
    getPropertyPolygonWeight: state => property => {
      return 1;
    },
    getCommonParcelPolygonWeight: state => commonParcel => {
      return 1;
    },
    getPropertyPolygonFillColor: state => property => {
      if (property.c_a_class == 1)
        // Residential
        return '#28a745';
      else if (property.c_a_class == 5)
        // Condominiums
        return '#f7a800';
      else if (property.c_a_class == 2 || property.c_a_class == 4)
        // Mercantile
        return '#fd7e14';
      else if (property.c_a_class == 3)
        // Manufacturing
        return '#dc3545';
      else if (property.c_a_class == 7)
        // Mercantile apartments
        return '#28a745';
      else
        return '#3f3f3f';
    },
    getCommonParcelPolygonFillColor: (state, getters) => commonParcel => {
      let property = commonParcel.parcels[0].property;
      return getters.getPropertyPolygonFillColor(property);
    },
    getPropertyPolygonFillOpacity: state => property => {
      if (property.c_a_class == 1)
        // Residential
        return 0.2;
      else
        return 0.4;
    },    
    getCommonParcelPolygonFillOpacity: (state, getters) => commonParcel => {
      let property = commonParcel.parcels[0].property;
      return getters.getPropertyPolygonFillOpacity(property);
    },    
  }
})