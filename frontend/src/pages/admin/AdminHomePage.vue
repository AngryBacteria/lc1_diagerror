<script setup lang="ts">
import { useFirebaseAuth } from 'vuefire'

//const data = await fetch('https://localhost:7139/weatherforecast')
//const json = data.json()
//console.log(json)
const auth = useFirebaseAuth()
console.log(auth)
</script>

<template>
  <h1>Admin Home Page</h1>
  <router-link to="/admin">admin</router-link>
  <br />
  <router-link to="/questionnaire/test">questionnaire</router-link>
</template>
