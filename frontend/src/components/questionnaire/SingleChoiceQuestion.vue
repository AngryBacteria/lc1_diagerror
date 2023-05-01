<template>
  <v-card>
    <h1 style="display: inline">{{ props.question.text }}</h1>
    <h1 v-if="!question.optional" style="color: #1fa481; display: inline"><sup>*</sup></h1>

    <h4>{{ question.subtext }}</h4>
    <v-radio-group density="compact" v-model="store.answers[props.index]" :rules="rules">
      <v-radio
        v-for="option in question.options"
        :key="option.index"
        :label="option.value"
        :value="`${option.index}`"
        color="primary"
        density="compact"
      >
      </v-radio>
    </v-radio-group>
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
      return !!store.answers[props.index] || t('questionnaire.validation.fieldRequired')
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
