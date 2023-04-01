<script setup lang="ts">
const navItems = [
  {
    title: 'Login',
    description: 'Description',
    icon: 'mdi-login'
  },
  {
    title: 'Logout',
    description: 'Description',
    icon: 'mdi-logout'
  }
]
</script>

<template>
  <p>todo admin-layout</p>
  <v-layout>
    <v-app-bar color="background" density="compact">
      <template v-slot:prepend> Admin-Tool </template>

      <template v-slot:append>
        <v-menu>
          <template v-slot:activator="{ props }">
            <v-btn variant="tonal" color="primary" v-bind="props"> Account </v-btn>
          </template>
          <v-list>
            <v-list-item v-for="(item, index) in navItems" :key="index" :value="index">
              <v-btn block :prepend-icon="item.icon"> {{ item.title }} </v-btn>
            </v-list-item>
          </v-list>
        </v-menu>
      </template>
    </v-app-bar>

    <v-main>
      <Suspense>
        <RouterView></RouterView>
      </Suspense>
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
