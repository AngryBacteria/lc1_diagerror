<script setup lang="ts">
import { RouterView } from 'vue-router'
import GlobalSnackbar from './components/global/GlobalSnackbar.vue'
</script>

<template>
  <GlobalSnackbar></GlobalSnackbar>
  <Suspense>
    <RouterView />
  </Suspense>
</template>

<style scoped></style>
