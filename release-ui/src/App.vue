<template>
  <v-app id="inspire">
    <v-navigation-drawer app v-model="drawer">
      <v-list-item>
        <v-list-item-content>
          <v-list-item-title class="text-h6">
            Iterations
          </v-list-item-title>
        </v-list-item-content>
      </v-list-item>

      <v-divider></v-divider>

      <v-progress-circular
          v-if="isLoading"
          indeterminate
          color="primary"
          style="left: 50%"
        ></v-progress-circular>

      <v-list dense nav>
        <v-list-item
          v-for="item in items"
          :to="{ name: 'Release', params: { iterationid: item.id } }"
          :key="item.title"
          link
        >
          <v-list-item-icon>
            <v-icon>mdi-sync</v-icon>
          </v-list-item-icon>

          <v-list-item-content>
            <v-list-item-title>{{ item.name }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar app>
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>

      <v-toolbar-title>
        <v-icon color="red"> mdi-rocket-launch </v-icon> Release
        UI</v-toolbar-title
      >
    </v-app-bar>

    <v-main>
      <router-view :key="$route.fullPath"></router-view>
    </v-main>
  </v-app>
</template>

<script>
import axios from "axios";

export default {
  data: () => ({
    drawer: null,
    items: null,
    isLoading: true
  }),
  created() {
    axios
      .get("https://devopscallerservice-prod-diplomarbeit.azurewebsites.net/DevOps/GetReleaseIterations")
      .then((response) => {
        this.items = response.data;
        this.isLoading = false;
        })
      .catch((error) => console.log(error));
  },
};
</script>