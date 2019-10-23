<template>
  <div>
    <b-row>
      <b-col>
        <b-button-toolbar>
          <b-button-group v-if="settings.newUrl && typeof(settings.newUrl) !== 'function'">
            <b-button :to="settings.newUrl"><font-awesome-icon icon="plus"></font-awesome-icon> New</b-button>
          </b-button-group>
          <b-button-group v-if="settings.newUrl && typeof(settings.newUrl) === 'function'">
            <b-button :to="settings.newUrl()"><font-awesome-icon icon="plus"></font-awesome-icon> New</b-button>
          </b-button-group>
          <b-button-group class="mx-2">
            <b-dropdown>
              <template slot="button-content">
                <font-awesome-icon icon="globe" />
              </template>
              <b-dropdown-item-button @click="setMapView('top')">
                <font-awesome-icon icon="square" v-if="showMap != 'top'" />
                <font-awesome-icon icon="check-square" v-if="showMap == 'top'" />
                  Show map on top
              </b-dropdown-item-button>
              <b-dropdown-item-button @click="setMapView('right')">
                <font-awesome-icon icon="square" v-if="showMap != 'right'" />
                <font-awesome-icon icon="check-square" v-if="showMap == 'right'" />
                  Show map on right
              </b-dropdown-item-button>
              <b-dropdown-item-button @click="setMapView('')">
                <font-awesome-icon icon="square" v-if="showMap != ''" />
                <font-awesome-icon icon="check-square" v-if="showMap == ''" />
                  Hide map
              </b-dropdown-item-button>
              <b-dropdown-divider></b-dropdown-divider>
              <b-dropdown-item-button :disabled="!canFilterBasedOnMap" @click="filterBasedOnMap = !filterBasedOnMap; refreshData()">
                <font-awesome-icon icon="square" v-if="!filterBasedOnMap" />
                <font-awesome-icon icon="check-square" v-if="filterBasedOnMap" />
                  Filter based on map {{!canFilterBasedOnMap ? '(Zoom in to filter based on the map)' : ''}}
              </b-dropdown-item-button>
            </b-dropdown>
          </b-button-group>
          <b-button-group class="mx-2">
            <b-dropdown>
              <template slot="button-content">
                <font-awesome-icon icon="table" />
              </template>
              <b-dropdown-item-button v-for="column in settings.columns" v-bind:key="column.key" v-on:click="toggleColumn(column)">
                <font-awesome-icon icon="square" v-if="!column.visible" />
                <font-awesome-icon icon="check-square" v-if="column.visible" />
                {{column.name}}
                </b-dropdown-item-button>
            </b-dropdown>
            <b-dropdown :text="limit.toString()">
              <b-dropdown-item-button v-for="limit in limits" v-bind:key="limit" v-on:click="setLimit(limit)">{{limit}}</b-dropdown-item-button>
            </b-dropdown>
          </b-button-group>
        </b-button-toolbar>
      </b-col>
      <b-col class="text-right">
        <span v-if="total > 0">
          {{page * limit - limit + 1}}-{{page * limit > total ? total : page * limit}} of {{total}} results
        </span>
        <span v-if="total == 0">
          No results
        </span>
      </b-col>
    </b-row>
    <b-row v-if="showMap == 'top'">
      <b-col>
        <filtered-table-map class="mt-2" :items="items" @bounds-changed="boundsChanged" @zoom-changed="zoomChanged"
          :get-item-info-window-text="settings.getItemInfoWindowText"
          :get-item-polygon-geometry="settings.getItemPolygonGeometry"
          :get-item-marker-position="settings.getItemMarkerPosition"
          :get-item-icon="settings.getItemIcon"
          :get-item-id="settings.getItemId"
          :location-data="locationData"
          :info-message="infoMessage">
        </filtered-table-map>
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <b-table striped hover :items="items" :fields="visibleFields" :busy="refreshingData" caption-top thead-class="hidden_header" responsive="md" @row-clicked="rowClicked" class="mt-2">
          <template slot="table-caption">
          </template>
          <template slot="thead-top">
            <tr>
              <th v-for="column in visibleColumns" v-bind:key="column.key" v-on:click="setSortColumn(column)">
                {{column.name}}
                <span v-if="column.key == sortColumn">
                  <font-awesome-icon :icon="sortOrder == 'asc' ? 'sort-amount-down' : 'sort-amount-up'" />
                </span>
              </th>
            </tr>
            <tr>
              <th v-for="column in visibleColumns" v-bind:key="column.key">
                <b-form-input v-model="filters[column.key]" v-if="column.filter == 'text'" @keyup="refreshData(true)"></b-form-input>
                <b-form-input v-model="filters[column.key]" v-if="column.filter == 'number'" type="number" @keyup="refreshData(true)"></b-form-input>
                <b-form-input v-model="filters[column.key]" v-if="column.filter == 'date'" type="date" @change="refreshData(true)"></b-form-input>
                <b-form-select v-model="filters[column.key]" v-if="column.filter == 'select'" :options="column.selectOptions" @change="refreshData()"></b-form-select>
              </th>
            </tr>
          </template>
          <template slot="actions" slot-scope="data">
            <b-button-group>
              <b-button variant="default" v-for="action in data.value.actions" v-bind:key="action.key">
                <router-link :to="action.getUrl(rawItems[data.index])"><font-awesome-icon :icon="action.icon" /></router-link>
              </b-button>
            </b-button-group>
          </template>
          <template v-slot:table-busy>
            <div class="text-center text-danger my-2">
              <b-spinner class="align-middle"></b-spinner>
              <strong>Loading...</strong>
            </div>
          </template>
        </b-table>
        <span v-if="total > 0">
          {{page * limit - limit + 1}}-{{page * limit > total ? total : page * limit}} of {{total}} results
        </span>
        <span v-if="total == 0">
          No results
        </span>
        <b-pagination v-model="page" :total-rows="total" :per-page="limit" @input="refreshData"></b-pagination>
      </b-col>
      <b-col v-if="showMap == 'right'">
        <filtered-table-map :items="items" @bounds-changed="boundsChanged" @zoom-changed="zoomChanged"
          :get-item-info-window-text="settings.getItemInfoWindowText"
          :get-item-polygon-geometry="settings.getItemPolygonGeometry"
          :get-item-marker-position="settings.getItemMarkerPosition"
          :get-item-icon="settings.getItemIcon"
          :get-item-id="settings.getItemId"
          :location-data="locationData"
          :info-message="infoMessage">
        </filtered-table-map>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import axios from "axios";
import moment from 'moment'

export default {
  name: "FilteredTable",
  props: [
    'settings',
    'locationData'
  ],
  data() {
    return {
      limit: 10,
      limits: [1, 2, 5, 10, 25, 50, 100, 1000],
      filters: {},
      rawItems: [],
      items: [],
      total: 0,
      page: 1,
      sortColumn: null,
      sortOrder: 'asc',
      refreshDataTimeout: null,
      bounds: null,
      filterBasedOnMap: false,
      canFilterBasedOnMap: true,
      showMap: 'right',
      refreshingData: false,
      refreshDataTime: null,
      zoom: null,
      infoMessage: null
    }
  },
  computed: {
    visibleColumns: function () {
      return this.settings.columns.filter(c => c.visible);
    },
    visibleFields: function () {
      return this.settings.columns.filter(c => c.visible && !c.key.startsWith('_')).map(c => c.key);
    }
  },
  methods: {
    setMapView: function (view) {
      this.showMap = view;
      this.filterBasedOnMap = false;
    },
    boundsChanged: function (bounds) {
      this.bounds = bounds;

      if (this.filterBasedOnMap)
        this.refreshData();
    },
    zoomChanged: function (zoom) {
      this.zoom = zoom;

      this.canFilterBasedOnMap = zoom >= 15;
      if (!this.canFilterBasedOnMap && this.filterBasedOnMap) {
        this.infoMessage = 'Data is no longer filtered based on the map. Zoom back in and check the "Filter based on map" option to filter based on the map.';
        this.filterBasedOnMap = false;
        this.refreshData();
      }

      if (this.canFilterBasedOnMap && this.filterBasedOnMap) {
        this.infoMessage = null;
      }
    },
    rowClicked: function (item, index, event) {
      let rawItem = this.rawItems[index];
      this.$emit('rowClicked', rawItem);
    },
    toggleColumn: function (column) {
      column.visible = !column.visible;
      this.refreshItems();
    },
    setLimit: function (newLimit) {
      this.limit = newLimit;
      this.refreshData();
    },
    setSortColumn: function (column) {
      if (!column.sortable)
        return;

      if (this.sortColumn == column.key)
        this.sortOrder = this.sortOrder == 'asc' ? 'desc' : 'asc';
      else {
        this.sortColumn = column.key;
        this.sortOrder = 'asc';
      }

      this.refreshData();
    },
    refreshItems: function () {
      // Recreates items from rawItems - doesn't actually go back to server for different data
      this.items = [];
      this.rawItems.forEach(row => {
        let item = {};
        this.settings.columns.forEach(col => {
          if (col.visible) {
            if (col.key == 'actions')
              item[col.key] = col;
            else {
              let p = col.key.split('.');
              let v = row;
              p.forEach(k => {
                v = v[k];
              });

              if (col.render)
                item[col.key] = col.render(v);
              else if (col.filter == 'select') {
                let selectItem = col.selectOptions.find(c => c.value == v);
                if (selectItem)
                  item[col.key] = selectItem.text;
              } else if (col.filter == 'date') {
                item[col.key] = moment(v).format('llll');
              }
              else
                item[col.key] = v;
            }
          }
          item['_raw'] = row;
          row['_item'] = item;
        });

        this.items.push(item);

        //console.log(this.items);
      });
    },
    refreshData: function (wait) {
      if (wait) {
        if (this.refreshDataTimeout != null)
          clearTimeout(this.refreshDataTimeout);

        this.refreshDataTimeout = setTimeout(() => {
          this.refreshData();
        }, 350);
        return;
      } else {
        this.refreshDataTimeout = null;
      }

      this.refreshingData = true;
      let refreshDataTime = new Date().getTime();
      this.refreshDataTime = refreshDataTime;

      // Refreshes rawItems -- hits the server to get new data
      let url = this.settings.endpoint + '?';
      let offset = (this.page * this.limit) - this.limit;

      url += '&offset=' + offset;
      url += '&limit=' + this.limit;

      if (this.sortColumn)
        url += '&order=' + encodeURIComponent(this.sortColumn + ' ' + this.sortOrder);

      let filters = [];
      let defaultFilter = this.settings.getDefaultFilter();
      if (defaultFilter)
        filters.push(defaultFilter);

      this.settings.columns.forEach(col => {
        let f = this.filters[col.key];
        if (f && f.length > 0) {
          if (col.filter == 'text')
            //filters.push('DbFunctionsExtensions.Like(EF.Functions, ' + col.key +', "%' + f + '%")');
            filters.push(col.key + '.Contains("' + f + '")');
          else if (col.filter == 'number')
            filters.push(col.key + '=' + f);
          else if (col.filter == 'select')
            filters.push(col.key + '="' + f + '"');
          else if (col.filter == 'date') {
            let d = moment(f).add(1, 'days');
            let e = moment(f);
            filters.push(col.key + '>="' + e.format('YYYY-MM-DD') + '" and ' + col.key + '<"' + d.format('YYYY-MM-DD') + '"');
          }
        }
      });

      let filter = filters.join(' AND ');
      if (filter.length > 0)
        url += '&filter=' + encodeURIComponent(filter);

      if (this.filterBasedOnMap && this.bounds) {
        url += 
          '&northBound=' + this.bounds.ne.lat +
          '&southBound=' + this.bounds.sw.lat +
          '&eastBound=' + this.bounds.ne.lng +
          '&westBound=' + this.bounds.sw.lng
      }

      axios
        .get(url)
        .then(response => {
          // This isn't the most recent refresh request, so don't bother doing anything with it
          if (this.refreshDataTime != refreshDataTime)
            return;

          this.rawItems = response.data;
          this.total = response.headers['x-total-count'];
          this.refreshItems();

          this.refreshingData = false;
        })
        .catch(error => {
          console.log(error);
        });
    }
  },
  watch: {
    locationData: function (newValue, oldValue) {
      if (newValue)
        this.filterBasedOnMap = true;
      else {
        this.filterBasedOnMap = false;
        this.refreshData();
      }
    }
  },
  mounted () {
    if (this.settings.defaultSortColumn)
      this.sortColumn = this.settings.defaultSortColumn;
    if (this.settings.defaultSortOrder)
      this.sortOrder = this.settings.defaultSortOrder;
    
    this.refreshData();
  },
  activated () {
    this.refreshData();
  }
};
</script>