<template>
  <div>
    <page-loading v-if="!item" />
    <page-title v-if="item" :title="pageTitle" />
    <b-row v-if="item" class="mt-3">
      <b-col>
        <application-user-fields :item="item" v-on:submit="onSubmit" v-on:cancel="onCancel">
          <template v-slot:save>Save</template>
        </application-user-fields>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "ApplicationUserEdit",
  props: ['id'],
  data() {
    return {
      item: null
    };
  },
  computed: {
    pageTitle: function () {
      return 'User: ' + this.item.userName;
    }
  },
  methods: {
    load: function () {
      let url = '/api/applicationUser/' + this.id;

      axios
        .get(url)
        .then(response => {
          console.log(response);

          // TODO: Check for 200?

          this.item = response.data;
        })
        .catch(error => {
          console.log(error);
        });
    },
    onCancel(evt) {
      this.$router.push('/ApplicationUser');
    },
    onSubmit(evt) {
      console.log(evt);
      console.log(this);

      let url = '/api/ApplicationUser/' + this.id;

      axios
        .put(url, this.item)
        .then(response => {
          console.log(response);

          // TODO: Check for 200?

          this.item = response.data;

          this.$parent.showToast('Your changes to "' + this.item.userName + '" were saved successfully.', {
            title: 'Saved',
            variant: 'success',
            autoHideDelay: 5000
          });

          this.$router.push('/ApplicationUser');
        })
        .catch(error => {
          console.log(error);

          let errorMessage = 'Unable to save "' + this.item.name + '". An unknown error occurred.';

          if (error.response.status == 400)
            errorMessage = 'Unable to save "' + this.item.name + '". The data you provided could not be validated.';
          else if (error.response.status == 403)
            errorMessage = 'Unable to save "' + this.item.name + '". You are not logged in or do not have access.';
          else if (error.response.status == 409)
            errorMessage = 'Unable to save "' + this.item.name + '". The item has been edited since you loaded the page. Reload this page to get the latest changes.';

          this.$parent.showToast(errorMessage, {
            title: 'Error Saving',
            variant: 'danger',
            autoHideDelay: 5000
          });
        });
    }    
  },
  mounted () {
    this.load();
    // Use this instead of the previous line to test the "Loading" bar
    //window.setTimeout(this.load, 3000);
  }
};
</script>