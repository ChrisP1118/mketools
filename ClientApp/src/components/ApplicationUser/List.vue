<template>
  <div>
    <page-title title="Users" />
    <b-row>
      <b-col>
        <filtered-table :settings="tableSettings">
        </filtered-table>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "ApplicationUserList",
  props: {},
  data() {
    let base = this;
    return {
      tableSettings: {
        endpoint: '/api/ApplicationUser',
        columns: [
          {
            key: 'Id',
            name: 'ID',
            visible: false,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'Email',
            name: 'Email Address',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'UserName',
            name: 'User Name',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'FirstName',
            name: 'First Name',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'LastName',
            name: 'Last Name',
            visible: true,
            sortable: true,
            filter: 'text'
          }
        ],
        getDefaultFilter: function () {
          return '';
        },
        rowClicked: function (item, context) {
          context.$router.push('/applicationUser/' + item.Id);
        }
      }
    }
  },
  methods: {
  },
  mounted () {
  }
};
</script>