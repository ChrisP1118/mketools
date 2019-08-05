<template>
    <b-form-group :description="description" :invalid-feedback="validationError">
        <b-form-checkbox v-model="content" @input="updateContent()" :value="checkedValue" :unchecked-value="uncheckedValue" :state="state">{{label}}</b-form-checkbox>
    </b-form-group>
</template>

<script>
export default {
  name: "CheckboxControl",
  props: {
    label: {},
    description: {},
    value: {
      required: true
    },
    checkedValue: {
      default: true
    },
    uncheckedValue: {
      default: false
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