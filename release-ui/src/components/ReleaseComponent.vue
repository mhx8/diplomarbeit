
<template>
  <v-container>
    <v-progress-circular
      v-if="isLoading"
      indeterminate
      color="primary"
      style="left: 50%"
    ></v-progress-circular>
    <v-card v-if="!isLoading">
      <v-banner>Feature Overview</v-banner>
      <template v-if="items.length">
        <v-tabs v-model="tab" light icons-and-text>
          <v-tabs-slider></v-tabs-slider>
          <v-tab v-for="item in items" :key="item.id" :href="'#tab-' + item.id">
            {{ item.name }}
            <v-icon color="purple">mdi-trophy</v-icon>
          </v-tab>
        </v-tabs>
        <v-tabs-items v-model="tab">
          <v-tab-item
            v-for="item in items"
            :key="item.id"
            :value="'tab-' + item.id"
          >
            <v-card>
              <v-list>
                <v-list-item-group color="primary">
                  <v-list-item
                    v-for="task in item.tasks"
                    :key="task.id"
                    :to="{
                      name: 'ReleasePipeline',
                      params: { taskid: task.id },
                    }"
                  >
                    <v-list-item-icon>
                      <v-icon color="yellow">mdi-animation</v-icon>
                    </v-list-item-icon>
                    <v-list-item-content>
                      <v-list-item-title v-text="task.name"></v-list-item-title>
                    </v-list-item-content>
                  </v-list-item>
                </v-list-item-group>
              </v-list>
            </v-card>
          </v-tab-item>
        </v-tabs-items>
      </template>
      <template v-else>
        <v-alert transition="fade-transition" dense text type="info">
          No Features associated with this Release Iteration.
        </v-alert>
      </template>
    </v-card>
  </v-container>
</template>
<script>
import axios from "axios";

export default {
  data: () => ({
    tab: null,
    items: null,
    isLoading: true,
  }),
  created() {
    axios
      .get(
        "https://devopscallerservice-prod-diplomarbeit.azurewebsites.net/DevOps/GetFeaturesByReleaseIteration?iterationId=" +
          this.$route.params.iterationid
      )
      .then((response) => {
        this.items = response.data;
        this.isLoading = false;
      })
      .catch((error) => console.log(error));
  },
};
</script>
