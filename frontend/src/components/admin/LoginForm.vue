<template>
  <v-card
    style="margin-left: auto; margin-right: auto; margin-top: 2rem;"
    width="700"
    title="Authentifikation"
    subtitle="Melden sie sich mit dem Admin-Konto an"
  >
    <v-card-text>
      <v-text-field v-model="email" label="Email" required></v-text-field>
      <v-text-field v-model="password" label="Passwort" required type="password"></v-text-field>
    </v-card-text>

    <v-card-actions>
      <v-btn
        variant="flat"
        color="primary"
        prepend-icon="mdi-login"
        elevation="3"
        rounded="false"
        size="large"
        block
        :loading="loginLoading"
        @click="login()"
      >
        Anmelden
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script setup lang="ts">
import { signInWithEmailAndPassword } from 'firebase/auth'
import { getCurrentUser, useFirebaseAuth } from 'vuefire'
import { useRouter } from 'vue-router'
import { ref, onMounted } from 'vue'
import { useUserStore } from '@/stores/user'

const router = useRouter()
const route = useRouter()
const email = ref('')
const password = ref('')
const store = useUserStore()
const loginLoading = ref(false)

/**
 * Hook to automatically restore the User from storage if its present
 */
onMounted(async () => {
  const currentUser = await getCurrentUser()
  if (currentUser) {
    console.log('User found in storage, restoring...')
    const to =
      route.currentRoute.value.query.redirect &&
      typeof route.currentRoute.value.query.redirect === 'string'
        ? route.currentRoute.value.query.redirect
        : '/admin'

    store.resetSnackbarConfig
    store.snackbarConfig.message = "Erfolgreich authentifiziert"
    store.snackbarConfig.color = 'primary'
    store.snackbarConfig.visible = true

    await router.push(to)
  }
})

/**
 * Initiates the firebase login with the provided credentials
 */
async function login() {
  loginLoading.value = true
  const auth = useFirebaseAuth()
  if (auth) {
    try {
      await signInWithEmailAndPassword(auth, email.value, password.value)

      store.resetSnackbarConfig
      store.snackbarConfig.message = "Erfolgreich authentifiziert"
      store.snackbarConfig.color = 'primary'
      store.snackbarConfig.visible = true

      const to =
        route.currentRoute.value.query.redirect &&
        typeof route.currentRoute.value.query.redirect === 'string'
          ? route.currentRoute.value.query.redirect
          : '/admin'
      await router.push(to)
    } catch (error: any) {
      const errorCode = error.code

      loginLoading.value = false

      store.resetSnackbarConfig
      store.snackbarConfig.message = errorCode.split('/')[1]
      store.snackbarConfig.color = 'error'
      store.snackbarConfig.visible = true
    }
  }
  loginLoading.value = false
}
</script>
<style scoped></style>
