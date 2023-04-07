<template>
  <v-row justify="center">
    <v-dialog v-model="dialog" max-width="290">
      <v-card>
        <v-card-title class="text-h5">
          <v-icon color="orange">mdi-alert</v-icon>
        </v-card-title>
        <v-card-text>
          There is already a higher version deployed. Normally deployment is not
          needed. Be carefull.
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="orange darken-1" text @click="onClick()"> OK </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-row>
</template>

<script>
export default {
  props: ["dialog"],
  methods: {
    onClick() {
      this.$emit("onHigherVersionDeployedDialogAction");
    },
  },
};
</script>