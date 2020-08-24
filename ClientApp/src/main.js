import Vue from 'vue'
import VueRouter from 'vue-router'
import BootstrapVue from 'bootstrap-vue'
import App from './App.vue'
import Vuex from 'vuex'

import GSignInButton from 'vue-google-signin-button'
import FBSignInButton from 'vue-facebook-signin-button'

import './custom.scss'

import { store } from './store/store';

// Font Awesome
import { library } from '@fortawesome/fontawesome-svg-core'
import { faSquare, faCheckSquare, faTable, faEdit, faSortAmountUp, faSortAmountDown, faTrash, faPlus, faGlobe, faRecycle } from '@fortawesome/free-solid-svg-icons'
import { faFacebook, faGoogle } from '@fortawesome/free-brands-svg-icons'
//import { fas } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

// Mixins
import AuthMixin from './components/Mixins/AuthMixin.vue'

// Form Controls
import TextControl from './components/Controls/TextControl.vue'
import TextareaControl from './components/Controls/TextareaControl.vue'
import SelectControl from './components/Controls/SelectControl.vue'
import CheckboxControl from './components/Controls/CheckboxControl.vue'

// Widgets
import PageLoading from './components/Common/PageLoading.vue'
import PageTitle from './components/Common/PageTitle.vue'
import FilteredTable from './components/Common/FilteredTable.vue'
import FilteredTableMap from './components/Common/FilteredTableMap.vue'
import NearbyMap from './components/Common/NearbyMap.vue'

import AddressLookup from './components/Home/AddressLookup.vue'
import AuthForm from './components/Home/AuthForm.vue'
import UserDispatchCallSubscriptionList from './components/Home/UserDispatchCallSubscriptionList.vue'
import UserPickupDatesSubscriptionList from './components/PickupDates/UserPickupDatesSubscriptionList.vue'
import BasicMap from './components/Home/BasicMap.vue'

// Pages
import About from './components/About.vue'
import DispatchCallTypes from './components/DispatchCallTypes.vue'
import Developers from './components/Developers.vue'
import Login from './components/Login.vue'
import Logout from './components/Logout.vue'
import Home from './components/Home.vue'
import ParcelList from './components/Parcel/List.vue'
import ParcelView from './components/Parcel/View.vue'
import ApplicationUserList from './components/ApplicationUser/List.vue'
import ApplicationUserFields from './components/ApplicationUser/Fields.vue'
import ApplicationUserEdit from './components/ApplicationUser/Edit.vue'
import PoliceDispatchCallList from './components/PoliceDispatchCall/List.vue'
import PoliceDispatchCallView from './components/PoliceDispatchCall/View.vue'
import FireDispatchCallList from './components/FireDispatchCall/List.vue'
import FireDispatchCallView from './components/FireDispatchCall/View.vue'
import CrimeList from './components/Crime/List.vue'
import CrimeView from './components/Crime/View.vue'
import HistoricPhotoList from './components/HistoricPhoto/List.vue'
import DispatchCallSubscriptionUnsubscribe from './components/DispatchCallSubscription/Unsubscribe.vue'
import PickupDatesSubscriptionUnsubscribe from './components/PickupDatesSubscription/Unsubscribe.vue'
import PickupDatesIndex from './components/PickupDates/Index.vue'

// Leaflet
import { LMap, LTileLayer, LMarker, LPopup, LCircle, LPolygon } from 'vue2-leaflet';
import { Icon } from 'leaflet'
import 'leaflet/dist/leaflet.css'

Vue.component('l-map', LMap);
Vue.component('l-tile-layer', LTileLayer);
Vue.component('l-marker', LMarker);
Vue.component('l-polygon', LPolygon);
Vue.component('l-popup', LPopup);
Vue.component('l-circle', LCircle);

delete Icon.Default.prototype._getIconUrl;

Icon.Default.mergeOptions({
  iconRetinaUrl: require('leaflet/dist/images/marker-icon-2x.png'),
  iconUrl: require('leaflet/dist/images/marker-icon.png'),
  shadowUrl: require('leaflet/dist/images/marker-shadow.png')
});

Vue.use(VueRouter);
Vue.use(BootstrapVue);
Vue.use(require('vue-moment'));
Vue.use(Vuex);

Vue.use(AuthMixin);

Vue.use(GSignInButton);
Vue.use(FBSignInButton);

library.add(faSquare, faCheckSquare, faTable, faEdit, faSortAmountUp, faSortAmountDown, faTrash, faPlus, faGlobe, faFacebook, faGoogle, faRecycle);
//library.add(fas);

Vue.component('font-awesome-icon', FontAwesomeIcon);
Vue.component('page-loading', PageLoading);
Vue.component('page-title', PageTitle);
Vue.component('filtered-table', FilteredTable);
Vue.component('filtered-table-map', FilteredTableMap);
Vue.component('nearby-map', NearbyMap);

Vue.component('address-lookup', AddressLookup);
Vue.component('auth-form', AuthForm);
Vue.component('user-dispatch-call-subscription-list', UserDispatchCallSubscriptionList);
Vue.component('user-pickup-dates-subscription-list', UserPickupDatesSubscriptionList);
Vue.component('basic-map', BasicMap);

Vue.component('text-control', TextControl);
Vue.component('textarea-control', TextareaControl);
Vue.component('select-control', SelectControl);
Vue.component('checkbox-control', CheckboxControl);

Vue.component('application-user-fields', ApplicationUserFields);

Vue.config.productionTip = false;

const routes = [
  { path: '/', component: Home, meta: { public: true } },
  { path: '/about', component: About, meta: { public: true } },
  { path: '/dispatchCallTypes', component: DispatchCallTypes, meta: { public: true } },
  { path: '/developers', component: Developers, meta: { public: true } },
  { path: '/login', component: Login, meta: { public: true } },
  { path: '/logout', component: Logout, meta: { public: true } },
  { path: '/parcel', component: ParcelList, meta: { public: true } },
  { path: '/parcel/:id', component: ParcelView, meta: { public: true }, props: true },
  { path: '/policeDispatchCall', component: PoliceDispatchCallList, meta: { public: true } },
  { path: '/policeDispatchCall/:id', component: PoliceDispatchCallView, meta: { public: true }, props: true },
  { path: '/fireDispatchCall', component: FireDispatchCallList, meta: { public: true } },
  { path: '/fireDispatchCall/:id', component: FireDispatchCallView, meta: { public: true }, props: true },
  { path: '/crime', component: CrimeList, meta: { public: true } },
  { path: '/crime/:id', component: CrimeView, meta: { public: true }, props: true },
  { path: '/historicPhoto', component: HistoricPhotoList, meta: { public: true } },
  { path: '/applicationUser', component: ApplicationUserList },
  { path: '/applicationUser/:id', component: ApplicationUserEdit, props: true },
  { path: '/dispatchCallSubscription/unsubscribe', component: DispatchCallSubscriptionUnsubscribe, meta: {public: true }},
  { path: '/pickupDatesSubscription/unsubscribe', component: PickupDatesSubscriptionUnsubscribe, meta: {public: true }},
  { path: '/pickupDates', component: PickupDatesIndex, meta: { public: true } },
];

const router = new VueRouter({
  routes // short for `routes: routes`
});

router.beforeEach((to, from, next) => {
  if (router.app.$root.$data && router.app.$root.$data.authenticatedUser && router.app.$root.$data.authenticatedUser.id) {
    // Allow authenticated users through
    next();
  } else if (to.matched.some(record => record.meta.public)) {
    // Allow all users to hit any page marked as public
    next();
  } else {
    // Redirect to the login page, passing the requested page as the redirect param
    next({
      path: '/login',
      query: { redirect: to.fullPath }
    });
  }
});

router.afterEach((to, from) => {
  //ga('set', 'page', to.path);
  //ga('send', 'pageview');
  gtag('config', window.GA_TRACKING_ID, {
    page_path: to.fullPath,
    app_name: 'MkeAlerts',
    send_page_view: true,
  });
});

new Vue({
  store,
  router,
  render: h => h(App),
  data: {
    authenticatedUser: {
      username: null,
      id: null,
      roles: []
    }
  }
}).$mount('#app');
