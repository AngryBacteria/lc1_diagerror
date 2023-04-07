<template>
  <div v-if="mdAndUp">
    <v-card>
      <h1>Likert-Scale Question</h1>
      <h4>Subtitle</h4>
      <v-input
        :rules="[() => !!store.answers[props.index] || t('questionnaire.validation.fieldRequired')]"
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
  </div>

  <div v-else>
    <single-choice-question :index="props.index"></single-choice-question>
  </div>
</template>

<script setup lang="ts">
import { useTypedI18n } from '@/composables/useTypedI18n'
import { useUserStore } from '@/stores/user'
import { computed } from 'vue'
import { useDisplay } from 'vuetify'
import SingleChoiceQuestion from './SingleChoiceQuestion.vue'

const { mdAndUp } = useDisplay()

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

const i18n = useTypedI18n()
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
