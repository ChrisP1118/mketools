<template>
    <b-form-group :label="label" :description="description" :invalid-feedback="validationError">
        <b-form-textarea v-model="content" @input="updateContent()" rows="4" max-rows="12" :state="state"></b-form-textarea>
    </b-form-group>
</template>

<script>
export default {
  name: "TextareaControl",
  props: {
    label: {},
    description: {},
    value: {
      required: true
    },
    required: {
      default: false
    },
    requiredError: {
      default: 'This field is required'
    },
    minLength: {
      default: null
    },
    minLengthError: {
      default: function () {
        return 'This field must be at least ' + this.minLength + ' characters long';
      }      
    },
    maxLength: {
      default: null
    },
    maxLengthError: {
      default: function () {
        return 'This field must not be more than ' + this.minLength + ' characters long';
      }
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
      else if (this.minLength && this.minLength > 0 && newValue.length < this.minLength)
        validationError = this.minLengthError;
      else if (this.maxLength && this.maxLength > 0 && newValue.length > this.maxLength)
        validationError = this.maxLengthError;
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