<template>
  <v-card>
    <h1>Multiple-Choice Question</h1>
    <h4>Subtitle</h4>
    <v-checkbox 
    density="compact" 
    multiple 
    v-model="store.answers[props.index]" 
    v-for="option in optionsFixed" :key="option" 
    :label="option" 
    :value="option"
    :rules="[() => !!store.answers[props.index] || t('questionnaire.validation.fieldRequired')]">
  
  </v-checkbox>
  </v-card>
</template>

<script setup lang="ts">
import { useTypedI18n } from '@/composables/useTypedI18n'
import { useUserStore } from '@/stores/user'
import { ref } from 'vue';

const { t } = useTypedI18n()
const store = useUserStore()
const props = defineProps({
  index: {
    type: Number,
    required: true
  }
})

const optionsFixed = ["Vor 3 monaten", "Vor 2 Monaten", "Vor 1 Monat", "Kürzlich"]
const answer = ref([])

store.answers[props.index] = []

</script>

<style scoped>
.v-card {
  padding: 1rem;
  margin: 1rem;
}
</style>
