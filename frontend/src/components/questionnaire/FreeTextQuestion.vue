<template>
  <v-card>
    <h1>{{ props.question.text }}</h1>
    <h4>{{ props.question.subtext }}</h4>
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
import type { Question } from '@/data/interfaces'
import { useUserStore } from '@/stores/user'
import type { PropType } from 'vue'

const { t } = useTypedI18n()
const store = useUserStore()
const props = defineProps({
  index: {
    type: Number,
    required: true
  },
  question: {
    type: Object as PropType<Question>,
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
