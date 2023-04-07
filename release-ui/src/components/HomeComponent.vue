<template>
  <v-card class="ml-10 mt-10" max-width="1200">
    <v-list-item three-line>
      <v-list-item-content>
        <v-list-item-title class="text-h5 mb-1"
          >Welcome to Release UI!
        </v-list-item-title>
        <v-list-item-subtitle
          >Please choose an Release Iteration.</v-list-item-subtitle
        >
      </v-list-item-content>

      <v-list-item-icon>
        <v-icon color="blue" x-large>mdi-microsoft-azure</v-icon>
      </v-list-item-icon>
    </v-list-item>
  </v-card>
</template>

<script>
export default {
  name: "HomeComponent",
};
</script>