<template>
  <v-card>
    <h1>Likert-Scale Question</h1>
    <h4>Subtitle</h4>
    <v-rating
      v-model="rating"
      color="primary"
      hover
    >  
  </v-rating>
  </v-card>
</template>

<script setup lang="ts">
import { useTypedI18n } from '@/composables/useTypedI18n'
import { useUserStore } from '@/stores/user'
import { ref } from 'vue';

const rating = ref()

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

.v-text-field {
  padding-top: 1rem;
}
</style>
