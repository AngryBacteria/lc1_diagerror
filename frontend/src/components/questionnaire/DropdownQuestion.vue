<template>
  <v-card>
    <h1 style="display: inline">{{ props.question.text }}</h1>
    <h1 v-if="!question.optional" style="color: #1fa481; display: inline"><sup>*</sup></h1>

    <h4>{{ question.subtext }}</h4>
    <v-select
      v-model="store.answers[props.index]"
      :items="question.options"
      item-title="value"
      item-value="index"
      label="Select"
      multiple
      color="primary"
      :rules="rules"
    ></v-select>
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

const rules = [
  () => {
    if (!props.question.optional) {
      if (!store.answers[props.index] || store.answers[props.index].length == 0) {
        return t('questionnaire.validation.fieldRequired')
      } else {
        return true
      }
    } else {
      return true
    }
  }
]
</script>

<style scoped>
.v-card {
  padding: 1rem;
  margin: 1rem;
}
</style>
