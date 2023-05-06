<template>
  <h1>upload page</h1>
  <v-textarea auto-grow v-model="questionnaire"> </v-textarea>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const questionnaire = ref('')
</script>

<style></style>
