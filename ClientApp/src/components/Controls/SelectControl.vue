<template>
    <b-form-group :label="label" :description="description" :invalid-feedback="validationError">
        <b-form-select v-model="content" @input="updateContent()" :options="options" :state="state"></b-form-select>
    </b-form-group>
</template>

<script>
export default {
  name: "SelectControl",
  props: {
    label: {},
    description: {},
    options: {},
    value: {
      required: true
    },
    required: {
      default: false
    },
    requiredError: {
      default: 'This field is required'
    },
    customValidation: {}
  },
  data() {
    return {
      content: this.value,
      originalValue: this.value,
      state: null,
      validationError: null
    };
  },
  computed: {
    isValid: function () {
      return this.state === null || this.state === true;
    }
  },
  methods: {
    updateContent() {
      this.$emit('input', this.content);
    }
  },
  watch: {
    content: function (newValue, oldValue) {
      let validationError = null;

      if (this.required !== null && this.required !== false && newValue == '')
        validationError = this.requiredError;
      else if (typeof(this.customValidation) === 'function') {
        let retVal = this.customValidation(newValue, oldValue);
        if (retVal !== true && retVal !== null)
          validationError = retVal;
        else {
          this.state = retVal;
          validationError = null;
        }
      }

      this.validationError = validationError;
      if (this.validationError != null)
        this.state = false;
      else {
        if (this.originalValue == newValue)
          this.state = null;
        else
          this.state = true;
      }
    }
  },
  mounted () {
  }
};
</script>