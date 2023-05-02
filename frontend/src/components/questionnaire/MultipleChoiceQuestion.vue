<template>
  <v-card>
    <h1 style="display: inline">{{ props.question.text }}</h1>
    <h1 v-if="!question.optional" style="color: #1fa481; display: inline"><sup>*</sup></h1>

    <h4>{{ question.subtext }}</h4>
    <v-checkbox
      v-for="option in question.options"
      :key="option.index"
      :label="option.value"
      :value="`${option.index}`"
      v-model="store.answers[props.index]"
      :validate-on="validateOn"
      :rules="rules"
      color="primary"
      density="compact"
    ></v-checkbox>
  </v-card>
</template>

<script setup lang="ts">
import { useTypedI18n } from '@/composables/useTypedI18n'
import type { Question } from '@/data/interfaces'
import { useUserStore } from '@/stores/user'
import type { PropType } from 'vue'
import { ref } from 'vue'

const validateOn = ref<'input' | 'submit' | 'blur'>('submit')

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

//Init if empty and change validation to input
if (!store.answers[props.index]) {
  store.answers[props.index] = []
}
//TODO check if there is a better way to make this work
const delay = (ms: number | undefined) => new Promise((res) => setTimeout(res, ms))
const validationChange = async () => {
  await delay(2000)
  console.log('Changing validation to input')
  validateOn.value = 'input'
}
validationChange()
</script>

<style scoped>
.v-card {
  padding: 1rem;
  margin: 1rem;
}
</style>
