<template>
  <div>
    <v-card v-if="mdAndUp">
      <h1 style="display: inline;">{{ props.question.text }}</h1>
      <h1 v-if="!question.optional" style="color: #1fa481; display: inline;"><sup>*</sup></h1>

      <h4>{{ props.question.subtext }}</h4>
      <v-input
        :model-value="store.answers[props.index]"
        :rules="rules"
      >
        <v-btn-toggle elevation="1" divided density="compact" v-model="store.answers[props.index]">
          <v-btn
            v-for="(item, index) in labels"
            :key="item"
            density="compact"
            :value="`${index}`"
            color="primary"
          >
            {{ item }}
          </v-btn>
        </v-btn-toggle>
      </v-input>
    </v-card>

    <v-card v-else>
      <h1>{{ question.text }}</h1>
      <h4>{{ question.subtext }}</h4>
      <v-radio-group
        density="compact"
        v-model="store.answers[props.index]"
        :rules="rules"
      >
        <v-radio
          v-for="(option, index) in labels"
          :key="index"
          :label="option"
          :value="`${index}`"
          color="primary"
          density="compact"
        >
        </v-radio>
      </v-radio-group>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import { useTypedI18n } from '@/composables/useTypedI18n'
import type { Question } from '@/data/interfaces'
import { useUserStore } from '@/stores/user'
import { computed } from 'vue'
import type { PropType } from 'vue'
import { useDisplay } from 'vuetify'

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
    }
    else {
      return true
    }
  }
]

const labels = computed(() => {
  if (i18n.locale.value === 'de') {
    return labelsGerman
  }
  if (i18n.locale.value === 'en') {
    return labelsEnglish
  } else {
    return labelsFrench
  }
})

const labelsEnglish = ['Strongly disagree', 'Disagree', 'Neutral', 'Agree', 'Strongly agree']
const labelsGerman = [
  'Trifft nicht zu',
  'Trifft eher nicht zu',
  'Neutral',
  'Trifft eher zu',
  'Trifft zu'
]
const labelsFrench = [
  "Pas du tout d'accord",
  "Pas d'accord",
  'neutre',
  "D'accord",
  "Tout à fait d'accord"
]

const i18n = useTypedI18n()
const { t } = useTypedI18n()
const { mdAndUp } = useDisplay()
const store = useUserStore()
</script>

<style scoped>
.v-card {
  padding: 1rem;
  margin: 1rem;
}
</style>
