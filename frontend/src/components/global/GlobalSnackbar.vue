<template>
  <v-snackbar
    v-model="store.snackbarConfig.visible"
    :color="store.snackbarConfig.color"
    :timeout="store.snackbarConfig.timeout"
    :location="store.snackbarConfig.location"
  >
    {{ store.snackbarConfig.message }}
    <template v-slot:actions>
      <v-btn
        color="white"
        variant="text"
        prepend-icon="mdi-close"
        @click="store.snackbarConfig.visible = false"
      >
        Close
      </v-btn>
    </template>
  </v-snackbar>
</template>

<script setup lang="ts">
import { useUserStore } from '@/stores/user'

export interface SnackbarConfig {
  visible: boolean
  message: string
  color: string
  timeout: string
  location: 'top' | 'bottom' | 'right' | 'left'
}

const store = useUserStore()
</script>
