<template>
  <div>
    <b-form novalidate @submit.prevent="onSubmit">
      <b-row>
        <b-col xs="12" md="6">
          <b-card class="mt-3">
            <h4 slot="header">Account</h4>
            <text-control label="User Name" v-model="item.UserName" disabled></text-control>
            <text-control label="Email Address" v-model="item.Email" required></text-control>
          </b-card>
        </b-col>
        <b-col xs="12" md="6">
          <b-card class="mt-3">
            <h4 slot="header">Basic</h4>
            <text-control label="First Name" v-model="item.FirstName"></text-control>
            <text-control label="Last Name" v-model="item.LastName"></text-control>
          </b-card>
        </b-col>
      </b-row>

      <b-row class="mt-3">
        <b-col>
          <b-button-toolbar key-nav>
            <b-button-group>
              <b-button type="submit" variant="primary">
                <slot name="save"></slot>
              </b-button>
            </b-button-group>
            <b-button-group class="mx-2">
              <b-button type="button" @click="onCancel">Cancel</b-button>
            </b-button-group>
          </b-button-toolbar>
        </b-col>
      </b-row>
          
    </b-form>
  </div>
</template>

<script>
export default {
  name: "ApplicationUserFields",
  props: [
      'item'
  ],
  data() {
    return {
    };
  },
  methods: {
    onCancel(evt) {
      this.$emit('cancel');
    },
    onSubmit(evt) {
      // Make sure all the child components are valid
      if (this.$children.every(childComponent => { return typeof(childComponent.isValid) === 'undefined' || childComponent.isValid; }))
        this.$emit('submit');
    }
  }
};
</script>