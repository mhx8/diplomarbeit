<template>
  <v-container>
    <v-alert
      v-if="triggerDone"
      transition="fade-transition"
      dense
      text
      type="success"
    >
      Release triggered successfully. Releases will be shortly reloaded!
    </v-alert>
    <v-alert
      v-if="triggerError"
      transition="fade-transition"
      dense
      text
      type="error"
    >
      Unexpected error while trigger. {{ this.triggerErrorMessage }}
    </v-alert>
    <v-progress-circular
      v-if="isLoading"
      indeterminate
      color="primary"
      style="left: 50%"
    ></v-progress-circular>
    <v-card v-if="!isLoading">
      <template v-if="releases.length">
        <v-banner>Release Pipeline Overview</v-banner>
        <v-list>
          <v-list-item-group color="primary">
            <v-list-item v-for="release in releases" :key="release.id">
              <v-list-item-icon>
                <v-icon>mdi-rocket-launch</v-icon>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title v-text="release.name"></v-list-item-title>
              </v-list-item-content>
              <v-list-item-action>
                <div v-if="release.alreadyHigherVersionDeployedOrTriggered">
                  <v-btn
                    class="mx-2"
                    fab
                    x-small
                    @click="higherVersionDeployedDialog = true"
                  >
                    <v-icon color="orange"> mdi-alert</v-icon>
                  </v-btn>
                </div>
              </v-list-item-action>
              <v-list-item-action>
                <div v-if="release.status === 2 || release.status === 3">
                  <v-btn class="mx-2" fab x-small @click="successDialog = true">
                    <v-icon color="green"> mdi-check-bold</v-icon>
                  </v-btn>
                </div>
                <div v-else-if="release.status === 0 || release.status === 4">
                  <v-progress-circular
                    v-if="deploymentOnGoing"
                    :size="20"
                    color="primary"
                    indeterminate
                  ></v-progress-circular>
                  <v-btn
                    class="mx-2"
                    fab
                    dark
                    x-small
                    color="pink"
                    v-if="!deploymentOnGoing"
                    @click="
                      {
                        deployDialog = true;
                        deployReleaseId = release.id;
                        deployEnvironmentId = release.environmentId;
                      }
                    "
                  >
                    <v-icon dark> mdi-rocket-launch </v-icon>
                  </v-btn>
                </div>
                <div v-if="release.status === 1">
                  <v-btn
                    class="mx-2"
                    fab
                    x-small
                    @click="inApprovalDialog = true"
                  >
                    <v-icon color="green">mdi-check-underline</v-icon>
                  </v-btn>
                </div>
              </v-list-item-action>
              <v-list-item-action>
                <v-btn
                  class="mx-2"
                  fab
                  x-small
                  @click="goToDeployment(release.url)"
                >
                  <v-icon color="blue"> mdi-link-variant </v-icon>
                </v-btn>
              </v-list-item-action>
            </v-list-item>
          </v-list-item-group>
        </v-list>
      </template>
      <template v-else>
        <v-alert transition="fade-transition" dense text type="info">
          No Release Pipelines associated with this Deployment Task.
        </v-alert>
      </template>
    </v-card>
    <success-dialog-component
      :dialog="successDialog"
      @onSuccessDialogAction="successDialog = false"
    />
    <deploy-dialog-component
      v-show="false"
      :dialog="deployDialog"
      :releaseId="deployReleaseId"
      :environmentId="deployEnvironmentId"
      @onDeployDialogAction="handleDeploy"
    />
    <higher-version-deployed-dialog-component
      :dialog="higherVersionDeployedDialog"
      @onHigherVersionDeployedDialogAction="higherVersionDeployedDialog = false"
    />

    <in-approval-dialog-component
      :dialog="inApprovalDialog"
      @inApprovalDialogAction="inApprovalDialog = false"
    />
  </v-container>
</template>
<script>
import axios from "axios";
import SuccessDialogComponent from "./Dialogs/SuccessDialogComponent";
import DeployDialogComponent from "../components/Dialogs/DeployDialogComponent";
import HigherVersionDeployedDialogComponent from "../components/Dialogs/HigherVersionDeployedDialogComponent";
import InApprovalDialogComponent from "../components/Dialogs/InApprovalDialogComponent";

export default {
  components: {
    SuccessDialogComponent,
    DeployDialogComponent,
    HigherVersionDeployedDialogComponent,
    InApprovalDialogComponent,
  },
  data: () => ({
    releases: null,
    isLoading: null,
    triggerDone: false,
    triggerError: false,
    triggerErrorMessage: null,
    deploymentOnGoing: false,
    successDialog: false,
    deployDialog: false,
    inApprovalDialog: false,
    higherVersionDeployedDialog: false,
    deployReleaseId: null,
    deployEnvironmentId: null,
  }),
  created() {
    this.init();
  },
  methods: {
    init: function () {
      this.isLoading = true;
      axios
        .get(
          "https://devopscallerservice-prod-diplomarbeit.azurewebsites.net/DevOps/GetReleasesByWorkItemId?workItemId=" +
            this.$route.params.taskid
        )
        .then((response) => {
          this.releases = response.data;
          this.isLoading = false;
        })
        .catch((error) => console.log(error));
    },
    handleDeploy: function (args) {
      console.log(args);
      this.deployDialog = false;
      if (!args.deploy) {
        return;
      }
      this.deploymentOnGoing = true;
      const body = {
        releaseId: args.releaseId,
        environmentId: args.environmentId,
      };
      axios
        .post("https://devopscallerservice-prod-diplomarbeit.azurewebsites.net/DevOps/TriggerRelease", body)
        .then(() => {
          this.triggerDone = true;
          this.deploymentOnGoing = false;
          this.releases.find(
            (element) => (element.id = args.releaseId)
          ).status = 2;
          this.handleTriggerDone();
        })
        .catch((error) => {
          this.triggerError = true;
          this.triggerErrorMessage = error;
          this.deploymentOnGoing = false;
          this.handleTriggerError();
        });
    },
    handleTriggerDone: function (event) {
      window.setTimeout(() => {
        this.triggerDone = false;
      }, 3000);
    },
    handleTriggerError: function (event) {
      window.setTimeout(() => {
        this.triggerError = false;
      }, 3000);
    },
    goToDeployment: function (link) {
      window.open(link);
    },
  },
};
</script>
