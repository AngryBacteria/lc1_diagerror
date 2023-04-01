<script setup lang="ts">
import { useUserStore } from '@/stores/user'
import { useRouter } from 'vue-router'
import { useFirebaseAuth } from 'vuefire'
import { signOut } from 'firebase/auth'

const store = useUserStore()
const router = useRouter()

async function logout() {
  try {
    const auth = useFirebaseAuth()
    if (auth) {
      await signOut(auth)
      //TODO react on failure
    }
    await router.push('/admin/login')
  } catch (error) {
    //TODO react on failure
    console.log(`Error while attempting to Log-Out: ${error}`)
  }
}
</script>

<template>
  <v-layout>
    <v-app-bar color="background" density="compact">
      <template v-slot:prepend> Admin-Tool </template>

      <template v-slot:append>
        <v-menu>
          <template v-slot:activator="{ props }">
            <v-btn v-if="store.isLoggedIn" variant="tonal" color="primary" v-bind="props">
              {{ store.refUser?.email }}
            </v-btn>
            <v-btn v-if="!store.isLoggedIn" variant="tonal" color="primary" v-bind="props">
              Nicht angemeldet
            </v-btn>
          </template>
          <v-list>
            <v-list-item v-if="!store.isLoggedIn">
              <v-btn @click="router.push('/admin/login')" block prepend-icon="mdi-login">
                Login
              </v-btn>
            </v-list-item>

            <v-list-item v-if="store.isLoggedIn">
              <v-btn @click="logout()" block prepend-icon="mdi-logout"> Logout </v-btn>
            </v-list-item>
          </v-list>
        </v-menu>
      </template>
    </v-app-bar>

    <v-main>
      <RouterView></RouterView>
    </v-main>
  </v-layout>
</template>

<style>
.v-main {
  margin-top: 0;
  margin-left: 5%;
  margin-right: 5%;
}
</style>
