<template>
  <v-card>
    <h1>FreeText Question</h1>
    <h4>Subtitle</h4>
    <v-textarea
      v-model="store.answers[props.index]"
      color="primary"
      :rules="[() => !!store.answers[props.index] || t('questionnaire.validation.fieldRequired')]"
      required
      label="Antwort"
      auto-grow
      rows="1"
    >
    </v-textarea>
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
