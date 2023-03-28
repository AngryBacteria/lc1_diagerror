<script setup lang="ts">

const data = await fetch('https://localhost:7139/weatherforecast')
const json = data.json()
console.log(json)

</script>

<template>
  <main>
    <h1>Hello</h1>
  </main>
</template>
