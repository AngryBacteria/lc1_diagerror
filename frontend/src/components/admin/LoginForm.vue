<template>
  <v-card
    style="margin-left: auto; margin-right: auto"
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
        @click="login()"
      >
        Anmelden
      </v-btn>
    </v-card-actions>
  </v-card>

  <v-snackbar color="success" close-delay="1000" v-model="alertOpen" location="top">
    {{ alertText }}
  </v-snackbar>

  <v-snackbar color="error" close-delay="2000" v-model="alertErrorOpen" location="top">
    {{ alertText }}
  </v-snackbar>
</template>

<script setup lang="ts">
import { signInWithEmailAndPassword } from 'firebase/auth'
import { getCurrentUser, useFirebaseAuth } from 'vuefire'
import { useRouter } from 'vue-router'
import { ref, onMounted } from 'vue'
import { useTypedI18n } from '@/composables/useTypedI18n'
const router = useRouter()
const route = useRouter()
const email = ref('')
const password = ref('')

//Snackbar/Alert Component
const alertText = ref('')
const alertOpen = ref(false)
const alertErrorOpen = ref(false)

// i18n
const { t } = useTypedI18n()

//Automatic restore from storage if present
onMounted(async () => {
  const currentUser = await getCurrentUser()
  if (currentUser) {
    console.log('User found in storage, restoring...')
    const to =
      route.currentRoute.value.query.redirect &&
      typeof route.currentRoute.value.query.redirect === 'string'
        ? route.currentRoute.value.query.redirect
        : '/admin'

    alertText.value = t('admin.alerts.authenticated')
    alertOpen.value = true

    await router.push(to)
  }
})

async function login() {
  const auth = useFirebaseAuth()
  if (auth) {
    try {
      await signInWithEmailAndPassword(auth, email.value, password.value)
      alertText.value = t('admin.alerts.authenticated')
      alertOpen.value = true
      const to =
        route.currentRoute.value.query.redirect &&
        typeof route.currentRoute.value.query.redirect === 'string'
          ? route.currentRoute.value.query.redirect
          : '/admin'
      await router.push(to)
    } catch (error: any) {
      const errorCode = error.code

      //TODO i18n
      alertText.value = errorCode.split('/')[1]
      alertErrorOpen.value = true
    }
  }
}
</script>
<style scoped></style>
