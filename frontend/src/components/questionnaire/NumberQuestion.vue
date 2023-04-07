<template>
  <v-card>
    <h1>Number Question</h1>
    <h4>Subtitle</h4>
    <v-text-field
      v-model="store.answers[props.index]"
      color="primary"
      type="number"
      :rules="[
        () =>
          (Number.isInteger(Number(store.answers[props.index])) &&
            store.answers[props.index] > 0) ||
          t('questionnaire.validation.fieldIsNumber'),
        () => !!store.answers[props.index] || t('questionnaire.validation.fieldRequired')
      ]"
      required
      label="Antwort"
    >
    </v-text-field>
  </v-card>
</template>

<script setup lang="ts">
import { useTypedI18n } from '@/composables/useTypedI18n'
import { useUserStore } from '@/stores/user'

const { t } = useTypedI18n()
const store = useUserStore()
const props = defineProps({
  index: {
    type: Number,
    required: true
  }
})
</script>

<style scoped>
.v-card {
  padding: 1rem;
  margin: 1rem;
}
</style>
