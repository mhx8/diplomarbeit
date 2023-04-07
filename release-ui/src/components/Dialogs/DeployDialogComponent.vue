<template>
  <v-row justify="center">
    <v-dialog v-model="dialog" max-width="290">
      <v-card>
        <v-card-title class="text-h5">
          <v-icon color="red">mdi-rocket-launch</v-icon>
        </v-card-title>
        <v-card-text>
          Are you sure you want to trigger this release pipeline?
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="red darken-1" text @click="onClick(false)">
            No
          </v-btn>
          <v-btn color="green darken-1" text @click="onClick(true)">
            Yes
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-row>
</template>

<script>
export default {
  props: ["dialog", "releaseId", "environmentId"],
  methods: {
    onClick(deploy) {
      this.$emit("onDeployDialogAction", {
        deploy: deploy,
        releaseId: this.releaseId,
        environmentId: this.environmentId
      });
    },
  },
};
</script>