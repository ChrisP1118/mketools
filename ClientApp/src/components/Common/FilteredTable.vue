<template>
  <div>
    <hr />
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
              <b-dropdown-item-button :disabled="!canFilterBasedOnMap" @click="filterBasedOnMap = !filterBasedOnMap">
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
        <filtered-table-map class="mt-2" :items="items" @bounds-changed="boundsChanged" 
          :get-item-info-window-text="settings.getItemInfoWindowText"
          :get-item-polygon-geometry="settings.getItemPolygonGeometry"
          :get-item-marker-geometry="settings.getItemMarkerGeometry"
          :get-item-icon="settings.getItemIcon"
          :get-item-id="settings.getItemId"
          :open-info-window-item="openInfoWindowItem">
        </filtered-table-map>
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <b-table striped hover :items="items" :fields="visibleFields" caption-top thead-class="hidden_header" responsive="md" @row-clicked="rowClicked" class="mt-2">
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
        <filtered-table-map :items="items" @bounds-changed="boundsChanged" 
          :get-item-info-window-text="settings.getItemInfoWindowText"
          :get-item-polygon-geometry="settings.getItemPolygonGeometry"
          :get-item-marker-geometry="settings.getItemMarkerGeometry"
          :get-item-icon="settings.getItemIcon"
          :get-item-id="settings.getItemId"
          :open-info-window-item="openInfoWindowItem">
        </filtered-table-map>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "FilteredTable",
  props: [
    'settings'
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
      openInfoWindowItem: null
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
      this.canFilterBasedOnMap = Math.abs(bounds.sw.lat - bounds.ne.lat) < 0.015 && Math.abs(bounds.ne.lng - bounds.sw.lng) < 0.030;
      if (!this.canFilterBasedOnMap)
        this.filterBasedOnMap = false;
      this.bounds = bounds;

      if (this.filterBasedOnMap)
        this.refreshData();
    },
    rowClicked: function (item, index, event) {
      // if (!this.settings.rowClicked)
      //   return;

      
      let rawItem = this.rawItems[index];
      this.$emit('rowClicked', rawItem);

      // if (this.settings.rowClicked)
      //   this.settings.rowClicked(rawItem, this);
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
        }, 250);
        return;
      } else {
        this.refreshDataTimeout = null;
      }

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
          else if (col.filter == 'date')
            filters.push(col.key + '="' + f + '"');
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
          //console.log(response);

          // TODO: Check for 200?

          this.rawItems = response.data;
          this.total = response.headers['x-total-count'];
          this.refreshItems();
        })
        .catch(error => {
          console.log(error);
        });
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