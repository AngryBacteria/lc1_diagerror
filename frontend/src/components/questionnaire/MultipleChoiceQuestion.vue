<template>
  <v-card>
    <h1 style="display: inline">{{ props.question.text }}</h1>
    <h1 v-if="!question.optional" style="color: #1fa481; display: inline"><sup>*</sup></h1>

    <h4>{{ question.subtext }}</h4>
    <v-checkbox
      v-for="option in question.options"
      :key="option.index"
      v-model="answers"
      color="primary"
      :label="option.value"
      :value="option.index"
      :rules="rules"
      density="compact"
      hide-details="auto"
    ></v-checkbox>
  </v-card>
</template>

<script setup lang="ts">
import { useTypedI18n } from '@/composables/useTypedI18n'
import type { Question } from '@/data/interfaces'
import { useUserStore } from '@/stores/user'
import type { PropType } from 'vue'
import { onMounted } from 'vue'
import { watch } from 'vue'
import { ref } from 'vue'

const answers = ref([])
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

/**
 * Validation rules
 */
const rules = [
  () => {
    if (!props.question.optional) {
      if (!answers.value || answers.value.length == 0) {
        return t('questionnaire.validation.fieldRequired')
      } else {
        return true
      }
    } else {
      return true
    }
  }
]

/**
 * If answers are already present in pinia, restore them in the component
 */
onMounted(() => {
  if (store.answers[props.index]) {
    answers.value = store.answers[props.index]
  }
})

/**
 * If answers change, save them into the pinia store
 */
watch(answers, () => {
  store.answers[props.index] = answers.value
})
</script>

<style scoped>
.v-card {
  padding: 1rem;
  margin: 1rem;
}
</style>
