<template>
  <v-row justify="center">
    <v-dialog v-model="dialog" max-width="290">
      <v-card>
        <v-card-title class="text-h5">
          <v-icon color="green">mdi-check-underline</v-icon>
        </v-card-title>
        <v-card-text>
          This releases pipeline is already triggered and now waiting for approval.
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="green darken-1" text @click="onClick()"> OK </v-btn>
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
      this.$emit("inApprovalDialogAction");
    },
  },
};
</script>