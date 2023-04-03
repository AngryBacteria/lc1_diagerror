<template>
    <v-card>
    <h1>FreeText Question</h1>
    <h4>Subtitle</h4>
    <v-text-field
        v-model="answer"
        color="primary"
        variant="underlined"
        :rules="[() => !!answer || 'This field is required']"
        required
        label="Antwort">
    </v-text-field>
</v-card>
</template>

<script setup lang="ts">
import { ref } from 'vue';


const answer = ref('')

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