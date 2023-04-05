<template>
  <v-card>
    <h1>Single-Choice Question</h1>
    <h4>Subtitle</h4>
    <v-radio-group
      density="compact"
      v-model="store.answers[props.index]"
      :rules="[() => !!store.answers[props.index] || t('questionnaire.validation.fieldRequired')]"
    >
      <v-radio
        v-for="item in optionsRef"
        :key="item.index"
        :label="item.label"
        :value="`${item.index}`"
        color="primary"
        density="compact"
      >
      </v-radio>
    </v-radio-group>

    <v-btn @click="usedOption = optionsGer">German</v-btn>
    <v-btn @click="usedOption = optionsEng">English</v-btn>
  </v-card>
</template>

<script setup lang="ts">
import { useTypedI18n } from '@/composables/useTypedI18n'
import { useUserStore } from '@/stores/user'
import { computed } from 'vue'
import { ref } from 'vue'

interface choice {
  index: number
  label: string
}

const { t } = useTypedI18n()
const store = useUserStore()
const props = defineProps({
  index: {
    type: Number,
    required: true
  }
})

const optionsEng = ['4 months ago', '2 months ago', 'recently']
const optionsGer = ['vor 4 monaten', 'vor 2 monaten', 'kürzlich']

const usedOption = ref(optionsEng)

const optionsRef = computed<choice[]>(() => {
  const output: choice[] = []
  usedOption.value.forEach((element, index) => {
    output.push({ index: index, label: element })
  })
  return output
})
</script>

<style scoped>
.v-card {
  padding: 1rem;
  margin: 1rem;
}
</style>
